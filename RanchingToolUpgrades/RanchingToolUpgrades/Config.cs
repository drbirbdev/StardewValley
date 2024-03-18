using BirbCore.Attributes;
#pragma warning disable CS0414 // Field is assigned but its value is never used

namespace RanchingToolUpgrades;

[SConfig]
internal class Config
{
    [SConfig.Option(0, 100000, 500)]
    public int PailBuyCost = 1000;

    [SConfig.Option(0, 100000, 500)]
    public int ShearsBuyCost = 1000;

    /*
    public bool BuyableAutograbber { get; set; } = true;

    public int AutograbberBuyCost { get; set; } = 25000;

    public float AutograbberUpgradeCostMultiplier { get; set; } = 5.0f;

    public int AutograbberUpgradeCostBars { get; set; } = 10;

    public int AutograbberUpgradeDays { get; set; } = 2;*/

    // N extra friendship per upgrade level.
    [SConfig.Option(0, 10)]
    public int ExtraFriendshipBase = 2;

    // N% chance of higher quality goods.
    [SConfig.Option(0, 1, 0.01f)]
    public float QualityBumpChanceBase = 0.05f;

    // N% chance of double produce.
    [SConfig.Option(0, 1, 0.01f)]
    public float ExtraProduceChance = 0.1f;
}
