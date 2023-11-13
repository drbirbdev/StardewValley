using StardewValley;

namespace LeaderboardLibrary;

class MultiplayerLeaderboardAPI : ChainableLeaderboardAPI
{
    private string ModId;
    private ILeaderboardAPI DelegateApi;
    public override ILeaderboardAPI Delegate => DelegateApi;

    public MultiplayerLeaderboardAPI(string modId)
    {
        ModId = modId;
        DelegateApi = new CachedLeaderboardAPI(modId);
        ModEntry.Instance.Helper.Events.Multiplayer.ModMessageReceived += this.Multiplayer_ModMessageReceived;
    }

    private void Multiplayer_ModMessageReceived(object sender, StardewModdingAPI.Events.ModMessageReceivedEventArgs e)
    {
        if (e.FromModID == ModEntry.Instance.ModManifest.UniqueID && e.Type == $"{ModId}:UploadScore" && e.FromPlayerID != Game1.player.UniqueMultiplayerID)
        {
            string name = Game1.getFarmer(e.FromPlayerID)?.Name;
            UploadScoreMessage message = e.ReadAs<UploadScoreMessage>();
            ((CachedLeaderboardAPI)Delegate).UpdateCache(message.Stat, message.Score, message.UserUUID, name);
        }
    }

    public override bool UploadScore(string stat, int score)
    {
        UploadScoreMessage message = new UploadScoreMessage(stat, score, ModEntry.GlobalModData.Value.UserUUID);
        ModEntry.Instance.Helper.Multiplayer.SendMessage<UploadScoreMessage>(message, $"{ModId}:UploadScore", new[] { ModEntry.Instance.ModManifest.UniqueID });
        return Delegate.UploadScore(stat, score);
    }
}

class UploadScoreMessage
{
    public string Stat;
    public int Score;
    public string UserUUID;

    public UploadScoreMessage(string stat, int score, string userUuid)
    {
        Stat = stat;
        Score = score;
        UserUUID = userUuid;
    }
}
