using BirbCore.Attributes;
using StardewModdingAPI;

namespace RealtimeFramework;

[SMod]
public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    internal static Assets Assets;

    internal ITranslationHelper I18N => this.Helper.Translation;

    internal static IRealtimeApi Api = new RealtimeApi();

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }

    public override object GetApi()
    {
        return Api;
    }
}
