using BirbCore.Annotations;
using StardewModdingAPI;

namespace SlimingSkill;

[SAsset]
internal class Assets
{

    [SAsset.Asset("assets/skill_texture.png")]
    public IRawTextureData SkillTexture { get; set; }
}
