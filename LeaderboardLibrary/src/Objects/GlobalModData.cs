using System;

namespace LeaderboardLibrary;


public sealed class GlobalModData
{
    public string UserUuid { get; init; } = Guid.NewGuid().ToString();
    public string Secret { get; init; } = Guid.NewGuid().ToString();
}
