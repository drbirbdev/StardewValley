using System.Collections.Generic;
using BirbCore.Annotations;
using BirbShared;
using SpaceCore;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Constants;

namespace BinningSkill;

[SEvent]
internal class Events
{

    [SEvent.GameLaunchedLate]
    private void GameLaunched(object sender, GameLaunchedEventArgs e)
    {
        Skills.RegisterSkill(new BirbSkill("drbirbdev.Binning", ModEntry.Assets.SkillTexture, ModEntry.Instance.Helper, ModEntry.MargoLoaded, new Dictionary<string, object>()
        {
            {"Recycler", null },
            {"Sneak", null },
            {"Environmentalist", null },
            {"Salvager", null },
            {"Upseller", null },
            {"Reclaimer", new {
                extra = ModEntry.Config.ReclaimerExtraValuePercent * 100,
                pExtra = (ModEntry.Config.ReclaimerPrestigeExtraValuePercent + ModEntry.Config.ReclaimerExtraValuePercent) * 100
            } }
        })
        {
            
            ExtraInfo = (level) =>
            {
                List<string> result = new()
                    {
                        ModEntry.Instance.I18n.Get("skill.perk.base", new { bonusPercent = ModEntry.Config.PerLevelBaseDropChanceBonus * 100 }),
                        ModEntry.Instance.I18n.Get("skill.perk.rare", new { bonusPercent = ModEntry.Config.PerLevelRareDropChanceBonus * 100 })
                    };
                if (level == ModEntry.Config.MegaMinLevel)
                {
                    result.Add(ModEntry.Instance.I18n.Get("skill.perk.mega_drops"));
                }
                if (level == ModEntry.Config.DoubleMegaMinLevel)
                {
                    result.Add(ModEntry.Instance.I18n.Get("skill.perk.double_mega_drops"));
                }

                return result;
            },
            HoverText = (level) =>
            {
                return ModEntry.Instance.I18n.Get("skill.perk.base", new { bonusPercent = level * ModEntry.Config.PerLevelBaseDropChanceBonus * 100 });
            }
        });

        if (ModEntry.MargoLoaded)
        {
            string id = Skills.GetSkill("drbirbdev.Binning").Id;
            ModEntry.MargoAPI.RegisterCustomSkillForPrestige(id);
        }
    }

    private uint PreviousTrashCansChecked;
    private int PiecesOfTrashUntilFriendshipIncrease;
    private uint PreviousPiecesOfTrashRecycled;
    private uint PreviousRecyclingBinsChecked;
    private uint PreviousFoodComposted;
    [SEvent.TimeChanged]
    private void TimeChanged(object sender, TimeChangedEventArgs e)
    {
        int recycled = (int)(Game1.stats.Get(StatKeys.PiecesOfTrashRecycled) - this.PreviousPiecesOfTrashRecycled);
        this.PreviousPiecesOfTrashRecycled = Game1.stats.Get(StatKeys.PiecesOfTrashRecycled);
        if (recycled > 0)
        {
            Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromRecycling * recycled);
        }

        this.PiecesOfTrashUntilFriendshipIncrease -= recycled;
        if (Game1.player.HasProfession("Environmentalist") && this.PiecesOfTrashUntilFriendshipIncrease < 0)
        {
            this.PiecesOfTrashUntilFriendshipIncrease += ModEntry.Config.RecyclingCountToGainFriendship;

            int friendship = ModEntry.Config.RecyclingFriendshipGain;
            if (Game1.player.HasProfession("Environmentalist", true))
            {
                friendship += ModEntry.Config.RecyclingPrestigeFriendshipGain;
            }
            // TODO: figure out better region than hard-coding Town
            Utility.improveFriendshipWithEveryoneInRegion(Game1.player, friendship, "Town");
        }

        int trashChecked = (int)(Game1.player.stats.Get(StatKeys.TrashCansChecked) - this.PreviousTrashCansChecked);
        this.PreviousTrashCansChecked = Game1.stats.Get(StatKeys.TrashCansChecked);
        if (trashChecked > 0)
        {
            Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromCheckingTrash * trashChecked);
        }

        int recyclingChecked = (int)(Game1.stats.Get("drbirbdev.BinningSkill_RecyclingBinsChecked") - this.PreviousRecyclingBinsChecked);
        this.PreviousRecyclingBinsChecked = Game1.stats.Get("drbirbdev.BinningSkill_RecyclingBinsChecked");
        if (recyclingChecked > 0)
        {
            Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromCheckingRecycling * recyclingChecked);
        }

        int compostingChecked = (int)(Game1.stats.Get("drbirbdev.BinningSkill_FoodComposted") - this.PreviousFoodComposted);
        this.PreviousFoodComposted = Game1.stats.Get("drbirbdev.BinningSkill_FoodComposted");
        if (compostingChecked > 0)
        {
            Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromComposting * compostingChecked);
        }
    }
}
