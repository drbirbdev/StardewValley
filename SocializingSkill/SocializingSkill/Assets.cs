using System.Collections.Generic;
using BirbCore.Attributes;
using StardewModdingAPI;

namespace SocializingSkill;

[SAsset]
internal class Assets
{
    [SAsset.Asset("assets/skill_texture.png")]
    public IRawTextureData SkillTexture { get; set; }


    [SAsset.Asset("assets/belovedtable.json")]
    public Dictionary<string, List<string>> BelovedTable { get; set; }
}
