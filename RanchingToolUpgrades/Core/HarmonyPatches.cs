using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using System.Collections.Generic;
using System.Reflection.Emit;
using StardewValley.Tools;

namespace RanchingToolUpgrades
{
    // 3rd party
    // Allow sending tools to upgrade in the mail with Mail Services
    [HarmonyPatch("MailServicesMod.ToolUpgradeOverrides", "mailbox")]
    class MailServicesMod_ToolUpgradeOverrides_Mailbox
    {
        public static bool Prepare()
        {
            return ModEntry.Instance.Helper.ModRegistry.IsLoaded("Digus.MailServicesMod");
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = new List<CodeInstruction>(instructions);

            for (int i = 0; i < code.Count; i++)
            {
                if (code[i].Is(OpCodes.Isinst, typeof(Axe)))
                {
                    yield return new CodeInstruction(OpCodes.Isinst, typeof(UpgradeablePail));
                    yield return code[i + 1];
                    yield return code[i + 2];
                    yield return code[i + 3];
                    yield return new CodeInstruction(OpCodes.Isinst, typeof(UpgradeableShears));
                    yield return code[i + 1];
                    yield return code[i + 2];
                    yield return code[i + 3];
                    yield return code[i];
                }
                else
                {
                    yield return code[i];
                }
            }
        }
    }
}
