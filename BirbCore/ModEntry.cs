using BirbCore.Annotations;
using StardewModdingAPI;

namespace BirbCore;
internal class ModEntry : Mod
{
    internal static ModEntry Instance;

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
