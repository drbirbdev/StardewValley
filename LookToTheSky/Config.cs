using BirbCore.Attributes;
using StardewModdingAPI;

namespace LookToTheSky;

[SConfig]
class Config
{
    [SConfig.Option]
    public SButton Button { get; set; } = SButton.U;

    [SConfig.Option(Min = 0, Max = 100)]
    public int SpawnChancePerSecond { get; set; } = 5;

    [SConfig.Option]
    public bool DoNotificationNoise { get; set; } = true;
}
