using BirbCore.Annotations;
using BirbCore.APIs;
using StardewModdingAPI;

namespace BinningSkill;

public class ModEntry : Mod
{
    internal static ModEntry Instance;
    internal static Config Config;
    internal static Assets Assets;

    [SMod.Api("DaLion.Overhaul", false)]
    internal static IMargo MargoAPI { get; set; }
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

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }

}
