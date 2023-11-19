using System;
using System.Collections.Generic;
using System.Reflection;
using BirbCore.Attributes;

namespace LeaderboardLibrary;

public class LeaderboardAPI : ChainableLeaderboardAPI
{
    private readonly string ModId;

    private readonly ILeaderboardAPI DelegateAPI;
    public override ILeaderboardAPI Delegate => this.DelegateAPI;

    public LeaderboardAPI(string modId)
    {
        this.ModId = modId;
        this.DelegateAPI = new ThrottledLeaderboardAPI(modId);
    }


    public override int GetLocalRank(string stat)
    {
        try
        {
            return this.Delegate.GetLocalRank($"{this.ModId}:{stat}");
        }
        catch (Exception e)
        {
            Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            return -1;
        }
    }

    public override List<Dictionary<string, string>> GetLocalTopN(string stat, int count)
    {
        if (count > 10)
        {
            throw new ArgumentException("Cannot get more than 10 top scores from GetLocalTopN API (Do you really have more than 10 players?)");
        }
        try
        {
            return this.Delegate.GetLocalTopN($"{this.ModId}:{stat}", count);
        }
        catch (Exception e)
        {
            Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            return null;
        }
    }

    public override Dictionary<string, string> GetPersonalBest(string stat)
    {
        try
        {
            return this.Delegate.GetPersonalBest($"{this.ModId}:{stat}");
        }
        catch (Exception e)
        {
            Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            return null;
        }
    }

    public override int GetRank(string stat)
    {
        throw new NotImplementedException("GetRank API is not currently implemented");
    }

    public override List<Dictionary<string, string>> GetTopN(string stat, int count)
    {
        if (count > 10)
        {
            throw new ArgumentException("Cannot get more than 10 top scores from GetTopN API");
        }
        try
        {
            return this.Delegate.GetTopN($"{this.ModId}:{stat}", count);
        }
        catch (Exception e)
        {
            Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            return null;
        }
    }

    public override bool RefreshCache(string stat)
    {
        try
        {
            this.Delegate.RefreshCache($"{this.ModId}:{stat}");
        }
        catch (Exception e)
        {
            Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            return false;
        }
        return true;
    }

    public override bool UploadScore(string stat, int score)
    {
        try
        {
            this.Delegate.UploadScore($"{this.ModId}:{stat}", score);
        }
        catch (Exception e)
        {
            Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            return false;
        }
        return true;
    }
}
