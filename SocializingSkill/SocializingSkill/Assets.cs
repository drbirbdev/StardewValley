using System;
using System.Collections.Generic;
using BirbCore.Attributes;
using StardewModdingAPI;
using StardewValley.GameData;

namespace SocializingSkill;

[SAsset]
internal class Assets
{
    [SAsset.Asset("assets/skill_texture.png")]
    public IRawTextureData SkillTexture;

    [SAsset.Asset("assets/beloved_data.json")]
    public Dictionary<string, List<BelovedEntry>> BelovedData;

    public class BelovedEntry : GenericSpawnItemDataWithCondition
    {
        public string Dialogue { get; set; }
    }
}
