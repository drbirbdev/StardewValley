using BirbCore.Attributes;
using Microsoft.Xna.Framework.Graphics;

namespace MagicSkillPageIcon;

[SAsset]
public class Assets
{
    [SAsset.Asset("assets/magicskillpageicon.png")]
    public Texture2D SkillPageIcon { get; set; }
}
