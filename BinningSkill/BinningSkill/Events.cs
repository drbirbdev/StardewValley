using BirbCore.Annotations;
using BirbShared;
using SpaceCore;
using StardewModdingAPI.Events;
using StardewValley.Constants;
using StardewValley;

namespace BinningSkill;

[SEvent]
internal class Events
{

    [SEvent.ApisLoaded]
    private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
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
    private void GameLoop_TimeChanged(object sender, TimeChangedEventArgs e)
    {
        int recycled = (int)(Game1.stats.Get(StatKeys.PiecesOfTrashRecycled) - PreviousPiecesOfTrashRecycled);
        PreviousPiecesOfTrashRecycled = Game1.stats.Get(StatKeys.PiecesOfTrashRecycled);
        if (recycled > 0)
        {
            Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromRecycling * recycled);
        }

        PiecesOfTrashUntilFriendshipIncrease -= recycled;
        if (Game1.player.HasCustomProfession(BinningSkill.Environmentalist) && PiecesOfTrashUntilFriendshipIncrease < 0)
        {
            PiecesOfTrashUntilFriendshipIncrease += ModEntry.Config.RecyclingCountToGainFriendship;

            int friendship = ModEntry.Config.RecyclingFriendshipGain;
            if (Game1.player.HasCustomPrestigeProfession(BinningSkill.Environmentalist))
            {
                friendship += ModEntry.Config.RecyclingPrestigeFriendshipGain;
            }
            // TODO: figure out better region than hard-coding Town
            Utility.improveFriendshipWithEveryoneInRegion(Game1.player, friendship, "Town");
        }

        int trashChecked = (int)(Game1.player.stats.Get(StatKeys.TrashCansChecked) - PreviousTrashCansChecked);
        PreviousTrashCansChecked = Game1.stats.Get(StatKeys.TrashCansChecked);
        if (trashChecked > 0)
        {
            Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromCheckingTrash * trashChecked);
        }

        int recyclingChecked = (int)(Game1.stats.Get("drbirbdev.BinningSkill_RecyclingBinsChecked") - PreviousRecyclingBinsChecked);
        PreviousRecyclingBinsChecked = Game1.stats.Get("drbirbdev.BinningSkill_RecyclingBinsChecked");
        if (recyclingChecked > 0)
        {
            Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromCheckingRecycling * recyclingChecked);
        }

        int compostingChecked = (int)(Game1.stats.Get("drbirbdev.BinningSkill_FoodComposted") - PreviousFoodComposted);
        PreviousFoodComposted = Game1.stats.Get("drbirbdev.BinningSkill_FoodComposted");
        if (compostingChecked > 0)
        {
            Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromComposting * compostingChecked);
        }
    }
}
