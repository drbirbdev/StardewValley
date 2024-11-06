using System.Collections.Generic;

namespace LeaderboardLibrary;

public sealed class LocalModData
{
    public Dictionary<string, Dictionary<string, List<LeaderboardStat>>> LocalLeaderboards { get; } = new();
    public Dictionary<string, Dictionary<string, List<LeaderboardStat>>> TopLeaderboards { get; } = new();
    public HashSet<string> MultiplayerUuiDs { get; } = [];

    public LocalModData(string uuid)
    {
        this.MultiplayerUuiDs.Add(uuid);
    }
}
