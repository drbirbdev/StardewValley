using BirbCore.Attributes;

namespace WinterStarSpouse;

[SConfig]
internal class Config
{

    [SConfig.Option(0, 100, 1)]
    public int SpouseIsRecipientChance { get; set; } = 50;

    [SConfig.Option(0, 100, 1)]
    public int SpouseIsGiverChance { get; set; } = 50;
}
