using System.Collections.Generic;
using BirbCore.Annotations;
using BirbShared;
using SpaceCore;
using StardewValley;

namespace SocializingSkill;

[SEvent]
internal class Events
{
    [SEvent.ApisLoaded]
    private void ModClassParser_ApisLoaded(object sender, StardewModdingAPI.Events.OneSecondUpdateTickedEventArgs e)
    {
        Skills.RegisterSkill(new SocializingSkill());
        SpaceCore.Events.SpaceEvents.AfterGiftGiven += this.SpaceEvents_AfterGiftGiven;
    }

    [SEvent.SaveLoaded]
    private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
    {
        if (ModEntry.MargoLoaded)
        {
            string id = Skills.GetSkill("drbirbdev.Socializing").Id;
            ModEntry.MargoAPI.RegisterCustomSkillForPrestige(id);
        }
    }

    // Beloved Profession
    //  - reset which villagers have been checked for bonus gifts today for each player.
    [SEvent.DayStarted]
    private void GameLoop_DayStarted(object sender, StardewModdingAPI.Events.DayStartedEventArgs e)
    {
        ModEntry.BelovedCheckedToday.Value = new List<string>();
    }

    // Grant XP
    // Gifter Profession
    //  - Give extra friendship
    private void SpaceEvents_AfterGiftGiven(object sender, SpaceCore.Events.EventArgsGiftGiven e)
    {
        int taste = e.Npc.getGiftTasteForThisItem(e.Gift);
        if (Game1.player.HasCustomProfession(SocializingSkill.Gifter))
        {
            int extraFriendship = 0;
            if (Game1.player.HasCustomPrestigeProfession(SocializingSkill.Gifter))
            {
                extraFriendship += 20;
            }
            switch (taste)
            {
                case 0:
                    extraFriendship += ModEntry.Config.GifterLovedGiftExtraFriendship;
                    break;
                case 2:
                    extraFriendship += ModEntry.Config.GifterLikedGiftExtraFriendship;
                    break;
                case 8:
                    extraFriendship += ModEntry.Config.GifterNeutralGiftExtraFriendship;
                    break;
            }
            Game1.player.changeFriendship(extraFriendship, e.Npc);
        }

        if (taste <= 2)
        {
            float exp = ModEntry.Config.ExperienceFromGifts;
            if (taste == 0)
            {
                exp *= ModEntry.Config.LovedGiftExpMultiplier;
            }
            if (e.Npc.isBirthday())
            {
                exp *= ModEntry.Config.BirthdayGiftExpMultiplier;
            }
            Skills.AddExperience(Game1.player, "drbirbdev.Socializing", (int)exp);
        }
    }
}