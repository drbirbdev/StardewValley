using System.Collections.Generic;
using BirbCore.Annotations;
using Microsoft.Xna.Framework.Graphics;

namespace SocializingSkill;

[SAsset]
internal class Assets
{
    [SAsset.Asset("assets/socializingiconA.png")]
    public Texture2D IconA { get; set; }
    [SAsset.Asset("assets/socializingiconB.png")]
    public Texture2D IconB { get; set; }


    [SAsset.Asset("assets/friendly.png")]
    public Texture2D Friendly { get; set; }
    [SAsset.Asset("assets/smoothtalker.png")]
    public Texture2D SmoothTalker { get; set; }
    [SAsset.Asset("assets/gifter.png")]
    public Texture2D Gifter { get; set; }
    [SAsset.Asset("assets/helpful.png")]
    public Texture2D Helpful { get; set; }
    [SAsset.Asset("assets/haggler.png")]
    public Texture2D Haggler { get; set; }
    [SAsset.Asset("assets/beloved.png")]
    public Texture2D Beloved { get; set; }

    [SAsset.Asset("assets/friendlyP.png")]
    public Texture2D FriendlyP { get; set; }
    [SAsset.Asset("assets/smoothtalkerP.png")]
    public Texture2D SmoothTalkerP { get; set; }
    [SAsset.Asset("assets/gifterP.png")]
    public Texture2D GifterP { get; set; }
    [SAsset.Asset("assets/helpfulP.png")]
    public Texture2D HelpfulP { get; set; }
    [SAsset.Asset("assets/hagglerP.png")]
    public Texture2D HagglerP { get; set; }
    [SAsset.Asset("assets/belovedP.png")]
    public Texture2D BelovedP { get; set; }


    [SAsset.Asset("assets/belovedtable.json")]
    public Dictionary<string, List<string>> BelovedTable { get; set; }
}
