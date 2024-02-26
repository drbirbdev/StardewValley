using System.Reflection;
using BirbCore.Attributes;
using StardewModdingAPI;

namespace PuzzleFloor;

[SMod]
public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
