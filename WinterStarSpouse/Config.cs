using BirbCore.Annotations;

namespace WinterStarSpouse;

[SConfig]
internal class Config
{

    [SConfig.Option(Min = 0, Max = 100, Interval = 1)]
    public int SpouseIsRecipientChance { get; set; } = 50;

    [SConfig.Option(Min = 0, Max = 100, Interval = 1)]
    public int SpouseIsGiverChance { get; set; } = 50;
}
