using BirbCore.Annotations;
using StardewModdingAPI;

namespace WinterStarSpouse;

public class ModEntry : Mod
{
    internal static ModEntry Instance;
    internal static Config Config;
    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
