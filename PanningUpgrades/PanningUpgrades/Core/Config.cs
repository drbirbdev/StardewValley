using BirbCore.Attributes;

namespace PanningUpgrades;

[SConfig]
internal class Config
{

    [SConfig.Option]
    public bool BuyablePan { get; set; } = false;

    [SConfig.Option(0, 100000, 500)]
    public int BuyCost { get; set; } = 1000;

    [SConfig.Option(0, 3, 0.1f)]
    public float UpgradeCostMultiplier { get; set; } = 1.0f;

    [SConfig.Option(1, 20, 1)]
    public int UpgradeCostBars { get; set; } = 5;

    [SConfig.Option(1, 5, 1)]
    public int UpgradeDays { get; set; } = 2;

    [SConfig.Option(0, 1, 0.01f)]
    public float ExtraDrawBaseChance { get; set; } = 0.7f;

    [SConfig.Option(0, 1, 0.01f)]
    public float DailyLuckMultiplier { get; set; } = 1.0f;

    [SConfig.Option(0, 1, 0.01f)]
    public float LuckLevelMultiplier { get; set; } = 0.1f;

    [SConfig.SectionTitle("AnimationSection")]

    [SConfig.Paragraph("AnimationSectionText")]

    [SConfig.Option]
    public int AnimationFrameDuration { get; set; } = 140;

    [SConfig.Option]
    public int AnimationYOffset { get; set; } = -8;

}
