using StardewModdingAPI;
using BirbShared.Mod;

namespace PanningUpgrades
{
    internal class ModEntry : Mod
    {
        [SmapiInstance]
        public static ModEntry Instance;
        [SmapiConfig]
        public static Config Config;
        [SmapiCommand]
        public static Command Command;

        public override void Entry(IModHelper helper)
        {
            ModClass mod = new ModClass();
            mod.Parse(this, true);
        }
    }
}
