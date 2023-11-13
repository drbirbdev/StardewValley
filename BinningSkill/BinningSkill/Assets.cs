using BirbCore.Annotations;
using Microsoft.Xna.Framework.Graphics;

namespace BinningSkill;

[SAsset]
public class Assets
{
    [SAsset.Asset("assets/binningiconA.png")]
    public Texture2D IconA;
    [SAsset.Asset("assets/binningiconB.png")]
    public Texture2D IconB;

    [SAsset.Asset("assets/environmentalist.png")]
    public Texture2D Environmentalist;
    [SAsset.Asset("assets/reclaimer.png")]
    public Texture2D Reclaimer;
    [SAsset.Asset("assets/recycler.png")]
    public Texture2D Recycler;
    [SAsset.Asset("assets/salvager.png")]
    public Texture2D Salvager;
    [SAsset.Asset("assets/sneak.png")]
    public Texture2D Sneak;
    [SAsset.Asset("assets/upseller.png")]
    public Texture2D Upseller;

    // Prestige Icons
    [SAsset.Asset("assets/environmentalistP.png")]
    public Texture2D EnvironmentalistP;
    [SAsset.Asset("assets/reclaimerP.png")]
    public Texture2D ReclaimerP;
    [SAsset.Asset("assets/recyclerP.png")]
    public Texture2D RecyclerP;
    [SAsset.Asset("assets/salvagerP.png")]
    public Texture2D SalvagerP;
    [SAsset.Asset("assets/sneakP.png")]
    public Texture2D SneakP;
    [SAsset.Asset("assets/upsellerP.png")]
    public Texture2D UpsellerP;
}
