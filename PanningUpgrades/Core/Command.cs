using BirbShared;
using BirbShared.Command;
using StardewValley;
using StardewValley.Tools;

namespace PanningUpgrades
{
    [CommandClass]
    public class Command
    {
        [CommandMethod("Add a pan to the player inventory")]
        public static void GivePan(int level = 0)
        {
            Game1.player.addItemToInventory(new UpgradeablePan(level));
        }

        [CommandMethod("Remove a pan from the player inventory")]
        public static void RemovePan()
        {
            Item pan = Game1.player.getToolFromName("Pan");
            if (pan is not null)
            {
                Log.Info("Found pan to remove");
                Game1.player.removeItemFromInventory(pan);
            } else
            {
                Log.Info("Found no pan to remove");
            }
        }

        [CommandMethod("Add the original Copper Pan item to the player inventory")]
        public static void GiveOriginalPan()
        {
            Game1.player.addItemToInventory(new Pan());
        }
    }
}
