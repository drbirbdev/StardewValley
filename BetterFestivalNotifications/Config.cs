using BirbCore.Annotations;

namespace BetterFestivalNotifications
{
    [SConfig]
    internal class Config
    {
        [SConfig.Option]
        public bool PlayStartSound { get; set; } = true;
        [SConfig.Option]
        public string StartSound { get; set; } = "crystal";


        [SConfig.Option]
        public int WarnHoursAheadOfTime { get; set; } = 2;

        [SConfig.Option]
        public bool PlayWarnSound { get; set; } = true;

        [SConfig.Option]
        public bool ShowWarnNotification { get; set; } = true;

        [SConfig.Option]
        public string WarnSound { get; set; } = "phone";


        [SConfig.Option]
        public bool PlayOverSound { get; set; } = false;

        [SConfig.Option]
        public bool ShowOverNotification { get; set; } = false;

        [SConfig.Option]
        public string OverSound { get; set; } = "ghost";
    }
}
