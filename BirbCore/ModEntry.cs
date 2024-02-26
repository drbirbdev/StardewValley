using System.Reflection;
using BirbCore.Attributes;
using StardewModdingAPI;

namespace BirbCore;
internal class ModEntry : Mod
{
    // bootstrapping issue, cannot use SMod.Instance here...
    internal static ModEntry Instance;

    public override void Entry(IModHelper helper)
    {
        Instance = this;
        Parser.InitEvents();
        Parser.ParseAll(this);
    }
}
