using BirbShared;
using BirbShared.APIs;
using BirbShared.Asset;
using BirbShared.Command;
using BirbShared.Config;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace BinningSkill
{
    public class ModEntry : Mod
    {
        internal static ModEntry Instance;
        internal static Config Config;
        internal static Assets Assets;

        internal static bool RSVLoaded => ModEntry.Instance.Helper.ModRegistry.IsLoaded("Rafseazz.RidgesideVillage");
        internal static bool AutomateLoaded => ModEntry.Instance.Helper.ModRegistry.IsLoaded("Pathoschild.Automate");
        internal static bool JALoaded => ModEntry.Instance.Helper.ModRegistry.IsLoaded("spacechase0.JsonAssets");
        internal static bool DGALoaded => ModEntry.Instance.Helper.ModRegistry.IsLoaded("spacechase0.DynamicGameAssets");
        internal static bool MargoLoaded => ModEntry.Instance.Helper.ModRegistry.IsLoaded("DaLion.Overhaul");

        internal static IJsonAssetsApi JsonAssets;
        internal static IDynamicGameAssetsApi DynamicGameAssets;
        internal static IMargo MargoAPI;

        internal ITranslationHelper I18n => this.Helper.Translation;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            Log.Init(this.Monitor);

            Config = helper.ReadConfig<Config>();

            Assets = new Assets();
            new AssetClassParser(this, Assets).ParseAssets();

            this.Helper.Events.GameLoop.GameLaunched += this.Event_GameLaunched;
            this.Helper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;
        }

        private void Event_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            new ConfigClassParser(this, Config).ParseConfigs();
            new Harmony(this.ModManifest.UniqueID).PatchAll();
            new CommandClassParser(this.Helper.ConsoleCommands, new Command()).ParseCommands();


            // Register binning skill after checking if MARGO is loaded.
            SpaceCore.Skills.RegisterSkill(new BinningSkill());

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

            if (MargoLoaded)
            {
                MargoAPI = this.Helper.ModRegistry.GetApi<IMargo>("DaLion.Overhaul");
                if (MargoAPI is null)
                {
                    Log.Error("Can't access the MARGO API. Is the mod installed correctly?");
                }
            }
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (MargoAPI is not null)
            {
                string id = SpaceCore.Skills.GetSkill("drbirbdev.Binning").Id;
                MargoAPI.RegisterCustomSkillForPrestige(id);
            }
        }
    }
}
