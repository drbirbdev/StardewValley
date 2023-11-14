using System;
using System.Reflection;
using BirbCore;
using BirbCore.Annotations;
using HarmonyLib;
using SpaceCore;
using StardewModdingAPI;

namespace MagicSkillPageIcon;

[SMod]
class ModEntry : Mod
{
    [SMod.Instance]
    internal static ModEntry Instance;
    internal static Assets Assets;

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}

[HarmonyPatch("Magic.Framework.Skills.Skill", "GetName")]
class Skill_Constructor
{
    public static bool Prepare()
    {
        return ModEntry.Instance.Helper.ModRegistry.IsLoaded("spacechase0.Magic");
    }

    public static void Postfix(Skills.Skill __instance)
    {
        try
        {
            __instance.SkillsPageIcon ??= ModEntry.Assets.SkillPageIcon;
        }
        catch (Exception e)
        {
            Log.Error($"Failed in {MethodBase.GetCurrentMethod().DeclaringType}\n{e}");
        }
    }
}
