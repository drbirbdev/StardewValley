using BirbCore.Annotations;
using Microsoft.Xna.Framework.Graphics;

namespace BinningSkill;

[SAsset]
public class Assets
{
    [SAsset.Asset("assets/binningiconA.png")]
    public Texture2D IconA { get; set; }
    [SAsset.Asset("assets/binningiconB.png")]
    public Texture2D IconB { get; set; }


    [SAsset.Asset("assets/environmentalist.png")]
    public Texture2D Environmentalist { get; set; }
    [SAsset.Asset("assets/reclaimer.png")]
    public Texture2D Reclaimer { get; set; }
    [SAsset.Asset("assets/recycler.png")]
    public Texture2D Recycler { get; set; }
    [SAsset.Asset("assets/salvager.png")]
    public Texture2D Salvager { get; set; }
    [SAsset.Asset("assets/sneak.png")]
    public Texture2D Sneak { get; set; }
    [SAsset.Asset("assets/upseller.png")]
    public Texture2D Upseller { get; set; }

    // Prestige Icons
    [SAsset.Asset("assets/environmentalistP.png")]
    public Texture2D EnvironmentalistP { get; set; }
    [SAsset.Asset("assets/reclaimerP.png")]
    public Texture2D ReclaimerP { get; set; }
    [SAsset.Asset("assets/recyclerP.png")]
    public Texture2D RecyclerP { get; set; }
    [SAsset.Asset("assets/salvagerP.png")]
    public Texture2D SalvagerP { get; set; }
    [SAsset.Asset("assets/sneakP.png")]
    public Texture2D SneakP { get; set; }
    [SAsset.Asset("assets/upsellerP.png")]
    public Texture2D UpsellerP { get; set; }
}
