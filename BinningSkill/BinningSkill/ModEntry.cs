using System.Collections.Generic;
using BirbCore.Attributes;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace BinningSkill;

[SMod]
public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;

    internal static Config Config;
    internal static Assets Assets;

    internal ITranslationHelper I18N => this.Helper.Translation;

    public static readonly PerScreen<ISet<string>> UnderleveledCheckedGarbage = new();

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
