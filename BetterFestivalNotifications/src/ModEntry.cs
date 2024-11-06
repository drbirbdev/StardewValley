using BirbCore.Attributes;
using StardewModdingAPI;

namespace BetterFestivalNotifications;

[SMod]
public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    internal static Config Config;

    internal ITranslationHelper I18N => this.Helper.Translation;

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
