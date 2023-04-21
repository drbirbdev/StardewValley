using StardewModdingAPI;
using BirbShared.APIs;
using BirbShared.Mod;

namespace JunimoKartGlobalRankings
{
    public class ModEntry : Mod
    {
        [SmapiInstance]
        internal static ModEntry Instance;
        [SmapiApi(UniqueID = "drbirbdev.LeaderboardLibrary")]
        internal static ILeaderboard LeaderboardAPI;

        public override void Entry(IModHelper helper)
        {
            ModClass mod = new ModClass();
            mod.Parse(this, true);
        }
    }
}
