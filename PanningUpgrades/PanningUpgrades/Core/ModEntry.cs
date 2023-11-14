using BirbCore.Annotations;
using StardewModdingAPI;

namespace PanningUpgrades;

[SMod]
internal class ModEntry : Mod
{
    [SMod.Instance]
    public static ModEntry Instance;
    public static Config Config;
    public static Command Command;

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
