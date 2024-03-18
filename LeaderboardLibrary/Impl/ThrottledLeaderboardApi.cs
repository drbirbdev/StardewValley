using System;
using System.Collections.Generic;
using BirbCore.Attributes;

namespace LeaderboardLibrary;

class ThrottledLeaderboardApi(string modId) : ChainableLeaderboardApi
{
    private DateTime _refreshNextCall = DateTime.MinValue;
    private DateTime _uploadNextCall = DateTime.MinValue;

    protected override ILeaderboardApi Delegate { get; } = new MultiplayerLeaderboardApi(modId);

    public override bool RefreshCache(string stat)
    {
        if (DateTime.UtcNow > this._refreshNextCall)
        {
            this.Delegate.RefreshCache(stat);
            this._refreshNextCall = DateTime.UtcNow.AddSeconds(5);
        }
        else
        {
            Log.Warn($"{stat} was throttled when calling RefreshCache");
            return false;
        }
        return true;
    }

    public override bool UploadScore(string stat, int score)
    {
        int oldScore = 0;
        Dictionary<string, string> oldRecord = this.Delegate.GetLocalTopN(stat, 10).Find(match => match["UserUUID"] == ModEntry.GLOBAL_MOD_DATA.Value.UserUuid);
        if (oldRecord is not null)
        {
            _ = int.TryParse(oldRecord["Score"], out oldScore);
        }

        if (score <= oldScore)
        {
            return true;
        }

        if (DateTime.UtcNow > this._uploadNextCall)
        {
            this.Delegate.UploadScore(stat, score);
            this._uploadNextCall = DateTime.UtcNow.AddSeconds(5);
        }
        else
        {
            Log.Warn($"{stat} was throttled when calling UploadScore");
            return false;
        }
        return true;
    }
}
