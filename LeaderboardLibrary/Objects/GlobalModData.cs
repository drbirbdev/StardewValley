using System;

namespace LeaderboardLibrary;


public sealed class GlobalModData
{
    public string UserUUID { get; set; } = Guid.NewGuid().ToString();
    public string Secret { get; set; } = Guid.NewGuid().ToString();
}
