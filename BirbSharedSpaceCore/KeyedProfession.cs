using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
using SpaceCore.Interface;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace BirbShared
{
    internal class KeyedProfession : Skills.Skill.Profession
    {
        readonly object Tokens;
        readonly ITranslationHelper I18n;

        bool PrestigeEnabled => this.PrestigeIcon != null;
        readonly Texture2D PrestigeIcon;
        readonly Texture2D NormalIcon;
        private bool IsPrestiged = false;
        readonly IModHelper ModHelper;

        public KeyedProfession(Skills.Skill skill, string id, Texture2D icon, IModHelper modHelper, object tokens = null, Texture2D prestigeIcon = null) : base(skill, id)
        {
            this.Icon = icon;
            this.I18n = modHelper.Translation;
            this.Tokens = tokens;
            this.ModHelper = modHelper;

            if (prestigeIcon != null)
            {
                this.PrestigeIcon = prestigeIcon;
                this.NormalIcon = icon;

                modHelper.Events.Display.MenuChanged += this.DisplayEvents_MenuChanged_MARGO;
                modHelper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded_MARGO;
            }

            modHelper.Events.Display.MenuChanged += this.DisplayEvents_MenuChanged;
        }

        private void DisplayEvents_MenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is not SkillLevelUpMenu levelUpMenu)
            {
                return;
            }

            string skill = this.ModHelper.Reflection.GetField<string>(levelUpMenu, "currentSkill").GetValue();
            if (skill != "drbirbdev.Binning")
            {
                return;
            }

            int level = this.ModHelper.Reflection.GetField<int>(levelUpMenu, "currentLevel").GetValue();

            List<CraftingRecipe> newRecipes = new List<CraftingRecipe>();

            int menuHeight = 0;
            foreach (KeyValuePair<string, string> recipePair in CraftingRecipe.craftingRecipes)
            {
                string conditions = ArgUtility.Get(recipePair.Value.Split('/'), 4, "");
                if (conditions.Contains(skill) && conditions.Contains(level.ToString() ?? ""))
                {
                    CraftingRecipe recipe = new CraftingRecipe(recipePair.Key, isCookingRecipe: false);
                    newRecipes.Add(recipe);
                    Game1.player.craftingRecipes.TryAdd(recipePair.Key, 0);
                    menuHeight += recipe.bigCraftable ? 128 : 64;
                }
            }
            foreach (KeyValuePair<string, string> recipePair in CraftingRecipe.cookingRecipes)
            {
                string conditions = ArgUtility.Get(recipePair.Value.Split('/'), 3, "");
                if (conditions.Contains(skill) && conditions.Contains(level.ToString() ?? ""))
                {
                    CraftingRecipe recipe = new CraftingRecipe(recipePair.Key, isCookingRecipe: true);
                    newRecipes.Add(recipe);
                    if (Game1.player.cookingRecipes.TryAdd(recipePair.Key, 0) && !Game1.player.hasOrWillReceiveMail("robinKitchenLetter"))
                    {
                        Game1.mailbox.Add("robinKitchenLetter");
                    }
                    menuHeight += recipe.bigCraftable ? 128 : 64;
                }
            }

            this.ModHelper.Reflection.GetField<List<CraftingRecipe>>(levelUpMenu, "newCraftingRecipes").SetValue(newRecipes);

            levelUpMenu.height = menuHeight + 256 + (levelUpMenu.getExtraInfoForLevel(skill, level).Count * 64 * 3 / 4);
        }

        private void GameLoop_SaveLoaded_MARGO(object sender, SaveLoadedEventArgs e)
        {
            if (Game1.player.HasCustomPrestigeProfession(this))
            {
                this.Icon = this.PrestigeIcon;
                this.IsPrestiged = true;
            }
        }

        private void DisplayEvents_MenuChanged_MARGO(object sender, MenuChangedEventArgs e)
        {
            // After the upgrade selection menu, unset the prestige description and icon of the profession that wasn't chosen.
            if (e.OldMenu is SkillLevelUpMenu oldMenu && oldMenu.isProfessionChooser)
            {
                if (Game1.player.HasCustomPrestigeProfession(this))
                {
                    return;
                }
                this.Icon = this.NormalIcon;
                this.IsPrestiged = false;
            }
        }

        public override string GetDescription()
        {
            if (this.CheckPrestigeMenu())
            {
                return this.I18n.Get($"{this.Id}.pdesc", this.Tokens);
            }
            else
            {
                return this.I18n.Get($"{this.Id}.desc", this.Tokens);
            }
        }

        private bool CheckPrestigeMenu()
        {
            if (!this.PrestigeEnabled)
            {
                return false;
            }
            if (this.IsPrestiged)
            {
                return true;
            }
            if (Game1.activeClickableMenu is not SkillLevelUpMenu currMenu)
            {
                return false;
            }
            if (!currMenu.isProfessionChooser)
            {
                return false;
            }
            string currSkill = this.ModHelper.Reflection.GetField<string>(currMenu, "currentSkill").GetValue();
            if (currSkill != this.Skill.Id)
            {
                return false;
            }
            int currentLevel = this.ModHelper.Reflection.GetField<int>(currMenu, "currentLevel").GetValue();
            if (currentLevel <= 10)
            {
                return false;
            }

            // All checks pass, we are in or after the prestiged skill select menu.
            // Set our description and icon to prestiged variants.
            // It's a bit weird to do this in GetDescription, but there's no earlier place.
            this.Icon = this.PrestigeIcon;
            this.IsPrestiged = true;

            return true;
        }

        public override string GetName()
        {
            return this.I18n.Get($"{this.Id}.name", this.Tokens);
        }
    }
}
