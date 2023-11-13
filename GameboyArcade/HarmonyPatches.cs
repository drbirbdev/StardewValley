using System;
using System.Reflection;
using BirbCore;
using HarmonyLib;
using StardewValley;

namespace GameboyArcade;

class Utilities
{
    public static void ShowArcadeMenu(string minigameId, string arcadeName)
    {
        Response[] arcadeOptions = new Response[2]
        {
            new Response("Play", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Play")),
            new Response("Exit", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Leave")),
        };
        Game1.currentLocation.createQuestionDialogue($"== {arcadeName} ==", arcadeOptions, $"drbirbdev.GameboyArcade {minigameId}");
    }
}

[HarmonyPatch(typeof(GameLocation), nameof(GameLocation.answerDialogueAction))]
class GameLocation_AnswerDialogueAction
{
    internal static void Postfix(string questionAndAnswer, string[] questionParams, ref bool __result)
    {
        try
        {
            if (!__result && questionAndAnswer == "drbirbdev.GameboyArcade_Play")
            {
                if (questionParams is null || questionParams.Length < 2)
                {
                    Log.Error("drbirbdev.ArcadeGame_Play dialogueKey requires minigame id parameter");
                    return;
                }

                Content content = null;

                if (questionParams.Length < 3)
                {
                    content = ModEntry.SearchGames(questionParams[1]);
                }
                else
                {
                    content = ModEntry.GetGame(questionParams[1], questionParams[2]);
                }

                if (content is null)
                {
                    Log.Error($"drbirbdev.ArcadeGame_Play dialogueKey had unknown minigame id parameter {questionParams.Join(delimiter: ",")}");
                    return;
                }

                GameboyMinigame.LoadGame(content);

                __result = true;
            }
        }
        catch (Exception e)
        {
            Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
        }
    }
}
