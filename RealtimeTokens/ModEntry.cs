using BirbCore.Annotations;
using StardewModdingAPI;

namespace RealtimeFramework;

[SMod]
public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    internal static Assets Assets;

    internal ITranslationHelper I18n => this.Helper.Translation;

    internal static IRealtimeAPI API = new RealtimeAPI();

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }

    public override object GetApi()
    {
        return API;
    }
}
