using System;
using System.Collections.Generic;
using BirbCore;

namespace LeaderboardLibrary;

class ThrottledLeaderboardAPI : ChainableLeaderboardAPI
{
    public DateTime RefreshNextCall = DateTime.MinValue;
    public DateTime UploadNextCall = DateTime.MinValue;

    private ILeaderboardAPI DelegateApi;
    public override ILeaderboardAPI Delegate => this.DelegateApi;

    public ThrottledLeaderboardAPI(string modId)
    {
        this.DelegateApi = new MultiplayerLeaderboardAPI(modId);
    }

    public override bool RefreshCache(string stat)
    {
        if (DateTime.UtcNow > this.RefreshNextCall)
        {
            this.Delegate.RefreshCache(stat);
            this.RefreshNextCall = DateTime.UtcNow.AddSeconds(5);
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
        Dictionary<string, string> oldRecord = this.Delegate.GetLocalTopN(stat, 10).Find((match) => match["UserUUID"] == ModEntry.GlobalModData.Value.UserUUID);
        if (oldRecord is not null)
        {
            int.TryParse(oldRecord["Score"], out oldScore);
        }

        if (score > oldScore)
        {
            if (DateTime.UtcNow > this.UploadNextCall)
            {
                this.Delegate.UploadScore(stat, score);
                this.UploadNextCall = DateTime.UtcNow.AddSeconds(5);
            }
            else
            {
                Log.Warn($"{stat} was throttled when calling UploadScore");
                return false;
            }
        }
        return true;
    }
}
