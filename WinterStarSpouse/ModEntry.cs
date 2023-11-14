using BirbCore.Annotations;
using StardewModdingAPI;

namespace WinterStarSpouse;

[SMod]
public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    internal static Config Config;
    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
