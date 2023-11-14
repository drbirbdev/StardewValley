global using SObject = StardewValley.Object;
using BirbCore.Annotations;
using StardewModdingAPI;

namespace RanchingToolUpgrades;

[SMod]
internal class ModEntry : Mod
{
    [SMod.Instance]
    public static ModEntry Instance;
    public static Config Config;

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
