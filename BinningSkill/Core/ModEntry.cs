using BirbShared;
using BirbShared.APIs;
using BirbShared.Asset;
using BirbShared.Command;
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

        public static bool RSVLoaded;
        public static bool JALoaded;
        public static bool DGALoaded;

        public static IJsonAssetsApi JsonAssets;
        public static IDynamicGameAssetsApi DynamicGameAssets;

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
            RSVLoaded = this.Helper.ModRegistry.IsLoaded("Rafseazz.RidgesideVillage");
            JALoaded = this.Helper.ModRegistry.IsLoaded("spacechase0.JsonAssets");
            DGALoaded = this.Helper.ModRegistry.IsLoaded("spacechase0.DynamicGameAssets");

            if (JALoaded)
            {
                JsonAssets = this.Helper.ModRegistry
                    .GetApi<IJsonAssetsApi>
                    ("spacechase0.JsonAssets");
                if (JsonAssets is null)
                {
                    Log.Error("Can't access the Json Assets API. Is the mod installed correctly?");
                }
            }

            if (DGALoaded)
            {
                DynamicGameAssets = this.Helper.ModRegistry
                    .GetApi<IDynamicGameAssetsApi>
                    ("spacechase0.DynamicGameAssets");
                if (DynamicGameAssets is null)
                {
                    Log.Error("Can't access the Dynamic Game Assets API. Is the mod installed correctly?");
                }
            }


            new ConfigClassParser(this, Config).ParseConfigs();
            HarmonyPatches.Patch(this.ModManifest.UniqueID);
            new CommandClassParser(this.Helper.ConsoleCommands, new Command()).ParseCommands();
            SpaceCore.Skills.RegisterSkill(new BinningSkill());

        }
    }
}
