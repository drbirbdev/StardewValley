using BirbShared.Config;

namespace BetterFestivalNotifications
{
    [ConfigClass]
    internal class Config
    {
        [ConfigOption]
        public bool PlayStartSound { get; set; } = true;
        [ConfigOption]
        public string StartSound { get; set; } = "crystal";


        [ConfigOption]
        public int WarnHoursAheadOfTime { get; set; } = 2;

        [ConfigOption]
        public bool PlayWarnSound { get; set; } = true;

        [ConfigOption]
        public bool ShowWarnNotification { get; set; } = true;

        [ConfigOption]
        public string WarnSound { get; set; } = "phone";


        [ConfigOption]
        public bool PlayOverSound { get; set; } = false;

        [ConfigOption]
        public bool ShowOverNotification { get; set; } = false;

        [ConfigOption]
        public string OverSound { get; set; } = "ghost";
    }
}
