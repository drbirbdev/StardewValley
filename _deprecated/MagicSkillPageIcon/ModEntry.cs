using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BirbCore.Attributes;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
using StardewModdingAPI;
#pragma warning disable CS0169 // Field is never used

namespace MagicSkillPageIcon;

[SMod]
class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);
    }
}

[HarmonyPatch("Magic.Framework.Skills.Skill", "GetName")]
class Skill_Constructor
{
    public static void Postfix(Skills.Skill __instance)
    {
        try
        {
            __instance.SkillsPageIcon ??= Assets.SkillPageIcon;
        }
        catch (Exception e)
        {
            Log.Error($"Failed in {MethodBase.GetCurrentMethod()?.DeclaringType}\n{e}");
        }
    }
}

[SAsset]
public class Assets
{
    [SAsset.Asset("assets/magic_skill_page_icon.png")]
    [SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible")]
    public static Texture2D SkillPageIcon;
}
