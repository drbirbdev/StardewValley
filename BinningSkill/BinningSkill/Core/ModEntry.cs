using System;
using BirbShared;
using BirbShared.APIs;
using BirbShared.Mod;
using SpaceCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Constants;

namespace BinningSkill
{
    public class ModEntry : Mod
    {
        [SmapiInstance]
        internal static ModEntry Instance;
        [SmapiConfig]
        internal static Config Config;
        [SmapiAsset]
        internal static Assets Assets;

        [SmapiApi(UniqueID = "DaLion.Overhaul", IsRequired = false)]
        internal static IMargo MargoAPI;
        internal static bool MargoLoaded
        {
            get
            {
                if (MargoAPI is null)
                {
                    return false;
                }
                IMargo.IModConfig config = MargoAPI.GetConfig();
                return config.EnableProfessions;
            }
        }

        internal ITranslationHelper I18n => this.Helper.Translation;

        public override void Entry(IModHelper helper)
        {
            ModClass mod = new ModClass();
            mod.Parse(this, true);
            mod.ApisLoaded += this.ModClassParser_ApisLoaded;
            this.Helper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;

            // TODO: I don't think this includes buffs.  I don't know if buffs can affect custom skills.
            GameStateQuery.Register("drbirbdev.BinningSkill_LEVEL", (string[] query, GameLocation location, Farmer player, Item targetItem, Item inputItem, Random random) =>
            {
                return GameStateQuery.Helpers.PlayerSkillLevelImpl(query, player, (Farmer target) => SpaceCore.Skills.GetSkillLevel(target, "drbirbdev.Binning"));
            });

            GameStateQuery.Register("drbirbdev.BinningSkill_RANDOM", (string[] query, GameLocation location, Farmer player, Item targetItem, Item inputItem, Random random) =>
            {
                if (!ArgUtility.TryGetFloat(query, 1, out float chance, out string error))
                {
                    return GameStateQuery.Helpers.ErrorResult(query, error);
                }
                bool addDailyLuck = false;
                for (int i = 2; i < query.Length; i++)
                {
                    if (string.Equals(query[i], "@addDailyLuck", StringComparison.OrdinalIgnoreCase))
                    {
                        addDailyLuck = true;
                    }
                }
                if (addDailyLuck)
                {
                    chance += (float)Game1.player.DailyLuck;
                }
                chance += Config.PerLevelRareDropChanceBonus * player.GetCustomSkillLevel("drbirbdev.Binning");
                return random.NextDouble() < (double)chance;
            });

            this.Helper.Events.GameLoop.TimeChanged += this.GameLoop_TimeChanged;
        }


        // TODO: Move stat tracking
        private uint PreviousTrashCansChecked;
        private int PiecesOfTrashUntilFriendshipIncrease;
        private uint PreviousPiecesOfTrashRecycled;
        private uint PreviousRecyclingBinsChecked;
        private uint PreviousFoodComposted;
        private void GameLoop_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            int recycled = (int)(Game1.stats.Get(StatKeys.PiecesOfTrashRecycled) - PreviousPiecesOfTrashRecycled);
            PreviousPiecesOfTrashRecycled = Game1.stats.Get(StatKeys.PiecesOfTrashRecycled);
            if (recycled > 0)
            {
                SpaceCore.Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromRecycling * recycled);
            }

            PiecesOfTrashUntilFriendshipIncrease -= recycled;
            if (Game1.player.HasCustomProfession(BinningSkill.Environmentalist) && PiecesOfTrashUntilFriendshipIncrease < 0)
            {
                PiecesOfTrashUntilFriendshipIncrease += Config.RecyclingCountToGainFriendship;

                int friendship = Config.RecyclingFriendshipGain;
                if (Game1.player.HasCustomPrestigeProfession(BinningSkill.Environmentalist))
                {
                    friendship += Config.RecyclingPrestigeFriendshipGain;
                }
                // TODO: figure out better region than hard-coding Town
                Utility.improveFriendshipWithEveryoneInRegion(Game1.player, friendship, "Town");
            }

            int trashChecked = (int)(Game1.player.stats.Get(StatKeys.TrashCansChecked) - PreviousTrashCansChecked);
            PreviousTrashCansChecked = Game1.stats.Get(StatKeys.TrashCansChecked);
            if (trashChecked > 0)
            {
                SpaceCore.Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromCheckingTrash * trashChecked);
            }

            int recyclingChecked = (int)(Game1.stats.Get("drbirbdev.BinningSkill_RecyclingBinsChecked") - PreviousRecyclingBinsChecked);
            PreviousRecyclingBinsChecked = Game1.stats.Get("drbirbdev.BinningSkill_RecyclingBinsChecked");
            if (recyclingChecked > 0)
            {
                SpaceCore.Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromCheckingRecycling * recyclingChecked);
            }

            int compostingChecked = (int)(Game1.stats.Get("drbirbdev.BinningSkill_FoodComposted") - PreviousFoodComposted);
            PreviousFoodComposted = Game1.stats.Get("drbirbdev.BinningSkill_FoodComposted");
            if (compostingChecked > 0)
            {
                SpaceCore.Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromComposting * compostingChecked);
            }
        }

        private void ModClassParser_ApisLoaded(object sender, OneSecondUpdateTickedEventArgs e)
        {
            SpaceCore.Skills.RegisterSkill(new BinningSkill());
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (MargoLoaded)
            {
                string id = SpaceCore.Skills.GetSkill("drbirbdev.Binning").Id;
                MargoAPI.RegisterCustomSkillForPrestige(id);
            }
        }
    }
}
