using BirbShared;
using BirbShared.Config;
using BirbShared.Command;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using BirbShared.Asset;

namespace RanchingToolUpgrades
{
    internal class ModEntry : Mod
    {
        public static ModEntry Instance;
        public static Config Config;
        public static Assets Assets;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            Log.Init(this.Monitor);

            Config = helper.ReadConfig<Config>();

            Assets = new Assets();
            new AssetClassParser(this, Assets).ParseAssets();

            this.Helper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;
        }

        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            new ConfigClassParser(this, Config).ParseConfigs();
            HarmonyPatches.Patch(this.ModManifest.UniqueID);
            new CommandClassParser(this.Helper.ConsoleCommands, new Command()).ParseCommands();
        }
    }
}
