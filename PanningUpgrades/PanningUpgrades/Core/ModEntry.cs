using BirbCore.Annotations;
using StardewModdingAPI;

namespace PanningUpgrades;

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
