using System.Collections.Generic;
using BirbCore.Attributes;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace SocializingSkill;

[SMod]
public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    internal static Config Config;
    internal static Assets Assets;

    internal ITranslationHelper I18N => this.Helper.Translation;

    internal static readonly PerScreen<List<string>> BelovedCheckedToday = new();

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
