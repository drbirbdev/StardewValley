using System.Collections.Generic;
using BirbCore.Annotations;
using StardewModdingAPI;

namespace LookToTheSky
{
    // TODOs:
    // Multiplayer compatibility (shared animations/controls/sound)
    // Text notifications option
    // More items (balloon)
    // Screen resize functionality
    // Make sure sprites are centered correctly
    // Loot tables in JSON
    // Slingshot charge sound/cooldown???
    // Content pack functionality
    public class ModEntry : Mod
    {
        internal static ModEntry Instance;
        internal static Config Config;
        internal static Assets Assets;

        public readonly List<SkyObject> SkyObjects = new();
        public readonly List<SkyProjectile> Projectiles = new();

        public override void Entry(IModHelper helper)
        {
            Parser.ParseAll(this);
        }
    }
}
