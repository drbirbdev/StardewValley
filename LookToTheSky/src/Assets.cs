using BirbCore.Attributes;
using Microsoft.Xna.Framework.Graphics;

namespace LookToTheSky;

[SAsset]
class Assets
{
    [SAsset.Asset("assets/firework.png")]
    public Texture2D Firework { get; set; }
}
