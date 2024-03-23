using BirbCore.Attributes;
using StardewValley;
using StardewValley.Delegates;

namespace SocializingSkill;

[SDelegate]
public class Delegates
{
    [SDelegate.GameStateQuery]
    public static bool HeartLevel(string[] query, GameStateQueryContext context)
    {
        string npcName = ArgUtility.Get(query, 2, "Target", false);
        if (npcName == "Target")
        {
            if (context.CustomFields is null ||
                !context.CustomFields.TryGetValue("NPC", out object value) ||
                value is not NPC npc)
            {
                Log.Warn("FRIENDSHIP GSQ expects NPC Custom Field");
                return false;
            }

            npcName = npc.Name;
        }

        if (!ArgUtility.TryGetInt(query, 1, out int level, out string error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return context.Player.getFriendshipHeartLevelForNPC(npcName) >= level;
    }
}
