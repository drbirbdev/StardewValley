using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using BirbShared;
using HarmonyLib;
using StardewValley;
using StardewValley.Minigames;

namespace JunimoKartGlobalRankings
{
    [HarmonyPatch(typeof(MineCart), nameof(MineCart.Die))]
    class MineCart_Die
    {
        internal static void Postfix(int ___score)
        {
            try
            {
                Task response = ModEntry.DdbContext.SaveAsync(new JunimoKartDDB()
                {
                    User = ModEntry.Credentials.GetIdentityId(),
                    Score = ___score,
                    Game = "JunimoKart",
                    Name = Game1.player.Name,
                    Farm = Game1.player.farmName,
                    Timestamp = DateTime.UtcNow.ToString("o")
                });
                response.ContinueWith((task) =>
                {
                    if (task.IsFaulted)
                    {
                        Log.Error(task.Exception.Message);
                        return;
                    }
                    if (task.IsCanceled)
                    {
                        Log.Error("JunimoKart Save Score was canceled");
                        return;
                    }
                    if (!task.IsCompleted)
                    {
                        Log.Error("JunimoKart Save Score failed to complete");
                        return;
                    }
                    Log.Info("JunimoKart saved score to global rankings");
                });
            }
            catch (Exception e)
            {
                Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            }
        }

    }

    [HarmonyPatch(typeof(NetLeaderboards), nameof(NetLeaderboards.LoadScores))]
    class NetLeaderboards_LoadScores
    {
        internal static bool Prefix(
            List<KeyValuePair<string, int>> scores,
            NetLeaderboards __instance)
        {
            try
            {
                __instance.entries.Clear();
                QueryRequest queryRequest = new QueryRequest()
                {
                    TableName = JunimoKartDDB.TABLE_NAME,
                    IndexName = JunimoKartDDB.HIGH_SCORE_INDEX_NAME,
                    KeyConditionExpression = "Game = :game",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":game", new AttributeValue {S = "JunimoKart"}}
                    },
                    ScanIndexForward = false,
                    Limit = 10,
                };

                Task<QueryResponse> response = ModEntry.DdbClient.QueryAsync(queryRequest);

                response.ContinueWith((task) =>
                {
                    if (task.IsFaulted)
                    {
                        Log.Error(task.Exception.Message);
                        return;
                    }
                    if (task.IsCanceled)
                    {
                        Log.Error("JunimoKart HighScore Query was canceled");
                        return;
                    }
                    if (!task.IsCompleted)
                    {
                        Log.Error("JunimoKart HighScore Query failed to complete");
                        return;
                    }

                    foreach (Dictionary<string, AttributeValue> item in task.Result.Items)
                    {
                        if (!item.TryGetValue("Name", out AttributeValue name) && name.S != "")
                        {
                            Log.Error("Invalid HighScore entry missing name: " + item);
                            return;
                        }
                        if (!item.TryGetValue("Score", out AttributeValue scoreString) || !int.TryParse(scoreString.N, out int score))
                        {
                            Log.Error("Invalid HighScore entry missing score: " + item);
                            return;
                        }

                        __instance.AddScore(name.S, score);
                    }
                    Log.Info("Loaded JunimoKart Global High Scores");
                });
                return false;
            }
            catch (Exception e)
            {
                Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            }
            return true;
        }
    }
}
