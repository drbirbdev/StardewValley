using StardewModdingAPI;
using SpaceCore;
using BirbCore.Annotations;
using BirbCore.APIs;

namespace SlimingSkill;

public class ModEntry : Mod
{
    internal static ModEntry Instance;
    internal static Config Config;
    internal static Command Command;
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

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
