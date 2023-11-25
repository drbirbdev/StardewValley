using System;
using BirbCore.Attributes;
using SpaceCore;
using StardewValley;

namespace BinningSkill;

[SDelegate]
internal class Delegates
{
    [SDelegate.GameStateQuery]
    public static bool Level(string[] query, GameLocation location, Farmer player, Item targetItem, Item inputItem, Random random)
    {
        // TODO: I don't think this includes buffs.  I don't know if buffs can affect custom skills.
        return GameStateQuery.Helpers.PlayerSkillLevelImpl(query, player, (target) => Skills.GetSkillLevel(target, "drbirbdev.Binning"));
    }

    [SDelegate.GameStateQuery]
    public static bool Random(string[] query, GameLocation location, Farmer player, Item targetItem, Item inputItem, Random random)
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
        chance += ModEntry.Config.PerLevelRareDropChanceBonus * player.GetCustomSkillLevel("drbirbdev.Binning");
        return random.NextDouble() < (double)chance;
    }
}
