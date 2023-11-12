using BirbCore.Annotations;
using StardewModdingAPI;

namespace BetterFestivalNotifications
{
    public class ModEntry : Mod
    {
        internal static ModEntry Instance;
        internal static Config Config;

        internal ITranslationHelper I18n => this.Helper.Translation;

        public override void Entry(IModHelper helper)
        {
            Parser.ParseAll(this);
        }
    }
}
