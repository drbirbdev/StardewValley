using HarmonyLib;
using System;
using System.Collections.Generic;
using StardewValley;
using SpaceCore;
using Microsoft.Xna.Framework;
using BirbShared;
using System.Reflection;
using System.Reflection.Emit;

namespace BinningSkill
{
    /// <summary>
    /// Reclaimer Profession
    /// </summary>
    [HarmonyPatch(typeof(Utility), nameof(Utility.getTrashReclamationPrice))]
    class Utility_GetTrashReclmantionPrice
    {
        public static void Postfix(
            Item i,
            Farmer f,
            ref int __result)
        {
            try
            {
                if (__result < 0)
                {
                    return;
                }

                if (!f.HasCustomProfession(BinningSkill.Reclaimer))
                {
                    return;
                }

                float extraPercentage = ModEntry.Config.ReclaimerExtraValuePercent / 100.0f;
                if (f.HasCustomPrestigeProfession(BinningSkill.Reclaimer))
                {
                    extraPercentage *= 1.5f;
                }

                __result = (int)(__result * extraPercentage);
            }
            catch (Exception e)
            {
                Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            }
        }
    }

    /// <summary>
    /// Upseller Profession
    /// </summary>
    [HarmonyPatch(typeof(NPC), nameof(NPC.getGiftTasteForThisItem))]
    class NPC_GetGiftTasteForThisItem
    {
        public static void Postfix(
            ref int __result)
        {
            try
            {
                if (!Game1.player.HasCustomProfession(BinningSkill.Upseller))
                {
                    return;
                }

                if (__result == 4)
                {
                    __result = 8;
                    return;
                }

                if (__result == 6)
                {
                    if (Game1.player.HasCustomPrestigeProfession(BinningSkill.Upseller))
                    {
                        __result = 8;
                    } else
                    {
                        __result = 4;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
            }
        }
    }

    /// <summary>
    /// Sneak Profession
    /// </summary>
    [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.CheckGarbage))]
    class GameLocation_CheckGarbage
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instr in instructions)
            {
                if (instr.Is(OpCodes.Call, AccessTools.Method(typeof(Utility), nameof(Utility.isThereAFarmerOrCharacterWithinDistance))))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GameLocation_CheckGarbage), nameof(CheckVicinity)));
                }
                else
                {
                    yield return instr;
                }
            }
        }

        public static Character CheckVicinity(Vector2 tileLocation, int tilesAway, GameLocation location)
        {
            Character c = Utility.isThereAFarmerOrCharacterWithinDistance(tileLocation, tilesAway, location);
            if (c != null && c is NPC && c is not StardewValley.Characters.Horse && c.Name.Equals("Linus"))
            {
                return c;
            }
            if (!Game1.player.HasCustomProfession(BinningSkill.Sneak))
            {
                return c;
            }
            if (Game1.player.HasCustomPrestigeProfession(BinningSkill.Sneak))
            {
                if (c != null && c is NPC npc && c is not StardewValley.Characters.Horse)
                {
                    c.doEmote(32);
                    Game1.player.changeFriendship(50, npc);
                    switch (npc.Age)
                    {
                        case 1:
                            npc.setNewDialogue(ModEntry.Instance.I18n.Get("sneak.prestige.dialogue.teen"), true, true);
                            break;
                        case 2:
                            npc.setNewDialogue(ModEntry.Instance.I18n.Get("sneak.prestige.dialogue.child"), true, true);
                            break;
                        default:
                            npc.setNewDialogue(ModEntry.Instance.I18n.Get("sneak.prestige.dialogue.adult"), true, true);
                            break;
                    }
                    Game1.drawDialogue(npc);
                }
            }
            return null;
        }
    }
}
