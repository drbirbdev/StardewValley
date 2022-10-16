using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirbShared.Command;
using StardewValley;

namespace BinningSkill.Core
{
    [CommandClass]
    public class Command
    {

        [CommandMethod("Get a random drop from the loot table for given location, trash can, and rarity")]
        public static void GetRandomTrashDrop(string location, string whichCan, int rarity = -1)
        {
            if (rarity == -1)
            {
                rarity = HarmonyPatches.GetRarity(HarmonyPatches.GetRarityLevels());
            }

            Game1.player.addItemByMenuIfNecessary(HarmonyPatches.GetRandomDropFromLootTable(location, whichCan, rarity));
        }

        [CommandMethod("Get a particular drop for a given item id in the trashdrops json format")]
        public static void GetTrashDrop(string id)
        {
            Game1.player.addItemByMenuIfNecessary(HarmonyPatches.ParseTrashDropId(id));
        }
    }
}
