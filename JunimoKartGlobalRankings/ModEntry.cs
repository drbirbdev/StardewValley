using BirbCore.APIs;
using BirbCore.Attributes;
using StardewModdingAPI;

namespace JunimoKartGlobalRankings;

[SMod]
public class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    [SMod.Api("drbirbdev.LeaderboardLibrary")]
    internal static ILeaderboard LeaderboardApi;

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}
