using BirbShared.Config;
using StardewModdingAPI;

namespace LookToTheSky
{
    [ConfigClass]
    class Config
    {
        [ConfigOption]
        public SButton Button { get; set; } = SButton.U;

        [ConfigOption(Min = 0, Max = 100)]
        public int SpawnChancePerSecond { get; set; } = 5;

        [ConfigOption]
        public bool DoNotificationNoise { get; set; } = true;
    }
}
