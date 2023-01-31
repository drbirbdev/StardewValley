using BirbShared.Asset;
using Microsoft.Xna.Framework.Graphics;

namespace LookToTheSky
{
    [AssetClass]
    class Assets
    {
        [AssetProperty("assets/firework.png")]
        public Texture2D Firework { get; set; }
    }
}
