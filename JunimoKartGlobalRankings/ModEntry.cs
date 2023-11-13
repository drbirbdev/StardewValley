using BirbCore.Annotations;
using BirbCore.APIs;
using StardewModdingAPI;

namespace JunimoKartGlobalRankings;

public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    [SMod.Api("drbirbdev.LeaderboardLibrary")]
    internal static ILeaderboard LeaderboardAPI;

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
