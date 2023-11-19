using System.Collections.Generic;
using BirbCore.Attributes;
using BirbShared;

namespace SlimingSkill;
internal class Events
{

    [SEvent.ApisLoaded]
    private void ModClassParser_ApisLoaded(object sender, StardewModdingAPI.Events.OneSecondUpdateTickedEventArgs e)
    {
        BirbSkill.Register("drbirbdev.Sliming", ModEntry.Assets.SkillTexture, ModEntry.Instance.Helper, new Dictionary<string, object>()
        {
            {"Rancher", null },
            {"Hunter", null },
            {"Breeder", null },
            {"Hatcher", null },
            {"Poacher", null },
            {"Tamer", null }
        }, PerkText, HoverText);
    }

    private static List<string> PerkText(int level)
    {
        List<string> result = new()
        {
            ModEntry.Instance.I18n.Get("skill.perk", new { bonus = 0 })
        };

        return result;
    }

    private static string HoverText(int level)
    {
        return ModEntry.Instance.I18n.Get("skill.perk", new { bonus = level * 0 });
    }

    [SEvent.SaveLoaded]
    private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
    {

    }
}
