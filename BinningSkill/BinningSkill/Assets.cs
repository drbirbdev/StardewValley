using BirbCore.Attributes;
using StardewModdingAPI;

namespace BinningSkill;

[SAsset(Priority = 0)]
public class Assets
{
    [SAsset.Asset("assets/skill_texture.png")]
    public IRawTextureData SkillTexture;
}
