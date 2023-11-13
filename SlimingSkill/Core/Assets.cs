using BirbCore.Annotations;
using Microsoft.Xna.Framework.Graphics;

namespace SlimingSkill;

[SAsset]
internal class Assets
{

    [SAsset.Asset("assets/slimingiconA.png")]
    public Texture2D IconA { get; set; }
    [SAsset.Asset("assets/slimingiconB.png")]
    public Texture2D IconB { get; set; }

    [SAsset.Asset("assets/rancher.png")]
    public Texture2D Rancher { get; set; }
    [SAsset.Asset("assets/breeder.png")]
    public Texture2D Breeder { get; set; }
    [SAsset.Asset("assets/hatcher.png")]
    public Texture2D Hatcher { get; set; }
    [SAsset.Asset("assets/hunter.png")]
    public Texture2D Hunter { get; set; }
    [SAsset.Asset("assets/poacher.png")]
    public Texture2D Poacher { get; set; }
    [SAsset.Asset("assets/tamer.png")]
    public Texture2D Tamer { get; set; }

    [SAsset.Asset("assets/rancherP.png")]
    public Texture2D RancherP { get; set; }
    [SAsset.Asset("assets/breederP.png")]
    public Texture2D BreederP { get; set; }
    [SAsset.Asset("assets/hatcherP.png")]
    public Texture2D HatcherP { get; set; }
    [SAsset.Asset("assets/hunterP.png")]
    public Texture2D HunterP { get; set; }
    [SAsset.Asset("assets/poacherP.png")]
    public Texture2D PoacherP { get; set; }
    [SAsset.Asset("assets/tamerP.png")]
    public Texture2D TamerP { get; set; }
}
