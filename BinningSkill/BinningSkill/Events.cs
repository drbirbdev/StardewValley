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

    [SEvent.ApisLoaded]
    private void ApiLoaded(object sender, OneSecondUpdateTickedEventArgs e)
    {
        Skills.RegisterSkill(new BinningSkill());

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
        if (Game1.player.HasCustomProfession(BinningSkill.Environmentalist) && this.PiecesOfTrashUntilFriendshipIncrease < 0)
        {
            this.PiecesOfTrashUntilFriendshipIncrease += ModEntry.Config.RecyclingCountToGainFriendship;

            int friendship = ModEntry.Config.RecyclingFriendshipGain;
            if (Game1.player.HasCustomPrestigeProfession(BinningSkill.Environmentalist))
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
