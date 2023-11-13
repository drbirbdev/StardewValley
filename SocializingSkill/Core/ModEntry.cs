using System.Collections.Generic;
using BirbCore.Annotations;
using BirbCore.APIs;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace SocializingSkill;

public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    internal static Config Config;
    internal static Assets Assets;
    [SMod.Api("DaLion.Overhaul", false)]
    internal static IMargo MargoAPI;
    internal static bool MargoLoaded
    {
        get
        {
            if (MargoAPI is null)
            {
                return false;
            }
            IMargo.IModConfig config = MargoAPI.GetConfig();
            return config.EnableProfessions;
        }
    }

    internal ITranslationHelper I18n => this.Helper.Translation;

    internal static readonly PerScreen<List<string>> BelovedCheckedToday = new();

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
