using BirbCore.Annotations;
using StardewModdingAPI;

namespace BetterFestivalNotifications;

[SMod]
public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    internal static Config Config;

    internal ITranslationHelper I18n => this.Helper.Translation;

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
