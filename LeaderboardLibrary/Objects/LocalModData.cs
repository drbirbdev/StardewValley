using System.Collections.Generic;

namespace LeaderboardLibrary;

public sealed class LocalModData
{
    public Dictionary<string, Dictionary<string, List<LeaderboardStat>>> LocalLeaderboards { get; set; } = new Dictionary<string, Dictionary<string, List<LeaderboardStat>>>();
    public Dictionary<string, Dictionary<string, List<LeaderboardStat>>> TopLeaderboards { get; set; } = new Dictionary<string, Dictionary<string, List<LeaderboardStat>>>();
    public HashSet<string> MultiplayerUUIDs { get; set; } = new HashSet<string>();

    public LocalModData(string uuid)
    {
        this.MultiplayerUUIDs.Add(uuid);
    }
}
