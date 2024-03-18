using BirbCore.Attributes;

namespace WinterStarSpouse;

[SConfig]
internal class Config
{
    [SConfig.Option(0, 100)]
    public int SpouseIsRecipientChance = 50;

    [SConfig.Option(0, 100)]
    public int SpouseIsGiverChance = 50;
}
