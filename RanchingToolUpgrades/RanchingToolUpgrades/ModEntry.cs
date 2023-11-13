global using SObject = StardewValley.Object;

using StardewModdingAPI;
using BirbCore.Annotations;

namespace RanchingToolUpgrades;

internal class ModEntry : Mod
{
    public static ModEntry Instance;
    public static Config Config;
    public static Command Command;

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
