using System.Threading;
using CoreBoy.memory.cart.battery;
using StardewModdingAPI;
using StardewValley;

namespace GameboyArcade
{
    class GameboySharedBattery : IBattery
    {
        private readonly string MinigameId;
        private readonly bool RemotePlayer = false;

        private bool AwaitingMessage = false;
        private SaveState Save;

        public GameboySharedBattery(string minigameId)
        {
            this.MinigameId = minigameId;

            if (Context.IsMainPlayer)
            {

            }
            else if (!Context.IsOnHostComputer)
            {
                this.RemotePlayer = true;
            }
        }

        private void Multiplayer_ModMessageReceived_LoadReceive(object sender, StardewModdingAPI.Events.ModMessageReceivedEventArgs e)
        {
            if (e.FromModID == ModEntry.Instance.ModManifest.UniqueID && e.Type == "LoadReceive")
            {
                this.Save = e.ReadAs<SaveState>();
                this.AwaitingMessage = false;
                ModEntry.Instance.Helper.Events.Multiplayer.ModMessageReceived -= this.Multiplayer_ModMessageReceived_LoadReceive;
            }
        }

        public void LoadRam(int[] ram)
        {
            this.LoadRamWithClock(ram, null);
        }

        public void LoadRamWithClock(int[] ram, long[] clockData)
        {
            if (this.RemotePlayer)
            {
                this.AwaitingMessage = true;
                ModEntry.Instance.Helper.Events.Multiplayer.ModMessageReceived += this.Multiplayer_ModMessageReceived_LoadReceive;
                ModEntry.Instance.Helper.Multiplayer.SendMessage<string>(this.MinigameId, "LoadRequest", new string[] { ModEntry.Instance.ModManifest.UniqueID }, new long[] { Game1.MasterPlayer.UniqueMultiplayerID });
                while (this.AwaitingMessage)
                {
                    Thread.Sleep(1);
                }
                this.Save.RAM.CopyTo(ram, 0);
                if (clockData is not null)
                {
                    this.Save.ClockData.CopyTo(clockData, 0);
                }
            }
            else
            {
                SaveState loaded = ModEntry.Instance.Helper.Data.ReadJsonFile<SaveState>($"data/{this.MinigameId}/{Constants.SaveFolderName}/file.json");
                if (loaded is null)
                {
                    return;
                }
                loaded.RAM.CopyTo(ram, 0);
                if (clockData is not null)
                {
                    loaded.ClockData.CopyTo(clockData, 0);
                }
            }
        }

        public void SaveRam(int[] ram)
        {
            this.SaveRamWithClock(ram, null);
        }

        public void SaveRamWithClock(int[] ram, long[] clockData)
        {
            SaveState save = new SaveState { RAM = ram, ClockData = clockData };
            if (this.RemotePlayer)
            {
                ModEntry.Instance.Helper.Multiplayer.SendMessage<SaveState>(save, $"SaveRequest {this.MinigameId}", new string[] { ModEntry.Instance.ModManifest.UniqueID }, new long[] { Game1.MasterPlayer.UniqueMultiplayerID });
            }
            else
            {
                ModEntry.Instance.Helper.Data.WriteJsonFile<SaveState>($"data/{this.MinigameId}/{Constants.SaveFolderName}/file.json", save);
            }
        }
    }
}
