using BirbCore.Annotations;
using SpaceCore;

namespace SlimingSkill;
internal class Events
{

    [SEvent.ApisLoaded]
    private void ModClassParser_ApisLoaded(object sender, StardewModdingAPI.Events.OneSecondUpdateTickedEventArgs e)
    {
        Skills.RegisterSkill(new SlimingSkill());
    }

    [SEvent.SaveLoaded]
    private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
    {
        if (ModEntry.MargoLoaded)
        {
            string id = Skills.GetSkill(SlimingSkill.ID).Id;
            ModEntry.MargoAPI.RegisterCustomSkillForPrestige(id);
        }
    }
}
