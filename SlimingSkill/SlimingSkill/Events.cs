using System.Collections.Generic;
using BirbCore.Annotations;
using BirbShared;
using SpaceCore;

namespace SlimingSkill;
internal class Events
{

    [SEvent.ApisLoaded]
    private void ModClassParser_ApisLoaded(object sender, StardewModdingAPI.Events.OneSecondUpdateTickedEventArgs e)
    {
        Skills.RegisterSkill(new BirbSkill("drbirbdev.Sliming", ModEntry.Assets.SkillTexture, ModEntry.Instance.Helper, ModEntry.MargoLoaded, new Dictionary<string, object>()
        {
            {"Rancher", null },
            {"Hunter", null },
            {"Breeder", null },
            {"Hatcher", null },
            {"Poacher", null },
            {"Tamer", null }
        })
        {
            ExtraInfo = (level) =>
            {
                List<string> result = new()
                {
                    ModEntry.Instance.I18n.Get("skill.perk", new { bonus = 0 })
                };

                return result;
            },
            HoverText = (level) =>
            {
                return ModEntry.Instance.I18n.Get("skill.perk", new { bonus = level * 0 });
            }
        });
        if (ModEntry.MargoLoaded)
        {
            string id = Skills.GetSkill("drbirbdev.Sliming").Id;
            ModEntry.MargoAPI.RegisterCustomSkillForPrestige(id);
        }
    }

    [SEvent.SaveLoaded]
    private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
    {

    }
}
