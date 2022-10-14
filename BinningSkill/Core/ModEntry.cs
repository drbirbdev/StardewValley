using BirbShared;
using BirbShared.Asset;
using BirbShared.Config;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace BinningSkill.Core
{
    public class ModEntry : Mod
    {
        public static ModEntry Instance;
        public static Config Config;
        public static Assets Assets;

        internal ITranslationHelper I18n => this.Helper.Translation;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            Log.Init(this.Monitor);

            Config = helper.ReadConfig<Config>();

            Assets = new Assets();
            new AssetClassParser(this, Assets).ParseAssets();

            this.Helper.Events.GameLoop.GameLaunched += this.Event_GameLaunched;
        }

        private void Event_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            new ConfigClassParser(this, Config).ParseConfigs();
            HarmonyPatches.Patch(this.ModManifest.UniqueID);
            SpaceCore.Skills.RegisterSkill(new BinningSkill());
        }
    }
}
