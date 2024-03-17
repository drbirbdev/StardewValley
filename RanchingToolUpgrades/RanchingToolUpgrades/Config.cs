using BirbCore.Attributes;

namespace RanchingToolUpgrades;

[SConfig]
internal class Config
{
    [SConfig.Option(0, 100000, 500)]
    public int PailBuyCost { get; set; } = 1000;

    [SConfig.Option(0, 3, 0.1f)]
    public float PailUpgradeCostMultiplier { get; set; } = 1.0f;

    [SConfig.Option(1, 20, 1)]
    public int PailUpgradeCostBars { get; set; } = 5;

    [SConfig.Option(1, 5, 1)]
    public int PailUpgradeDays { get; set; } = 2;

    [SConfig.Option(0, 100000, 500)]
    public int ShearsBuyCost { get; set; } = 1000;

    [SConfig.Option(0, 3, 0.1f)]
    public float ShearsUpgradeCostMultiplier { get; set; } = 1.0f;

    [SConfig.Option(1, 20, 1)]
    public int ShearsUpgradeCostBars { get; set; } = 5;

    [SConfig.Option(1, 5, 1)]
    public int ShearsUpgradeDays { get; set; } = 2;

    /*
    public bool BuyableAutograbber { get; set; } = true;

    public int AutograbberBuyCost { get; set; } = 25000;

    public float AutograbberUpgradeCostMultiplier { get; set; } = 5.0f;

    public int AutograbberUpgradeCostBars { get; set; } = 10;

    public int AutograbberUpgradeDays { get; set; } = 2;*/

    // N extra friendship per upgrade level.
    [SConfig.Option(0, 10, 1)]
    public int ExtraFriendshipBase { get; set; } = 2;

    // N% chance of higher quality goods.
    [SConfig.Option(0, 1, 0.01f)]
    public float QualityBumpChanceBase { get; set; } = 0.05f;

    // N% chance of double produce.
    [SConfig.Option(0, 1, 0.01f)]
    public float ExtraProduceChance { get; set; } = 0.1f;
}
