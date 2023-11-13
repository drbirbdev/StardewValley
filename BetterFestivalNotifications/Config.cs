using BirbCore.Annotations;

namespace BetterFestivalNotifications;

[SConfig]
internal class Config
{
    [SConfig.Option]
    public bool PlayStartSound = true;
    [SConfig.Option]
    public string StartSound = "crystal";


    [SConfig.Option]
    public int WarnHoursAheadOfTime = 2;

    [SConfig.Option]
    public bool PlayWarnSound = true;

    [SConfig.Option]
    public bool ShowWarnNotification = true;

    [SConfig.Option]
    public string WarnSound = "phone";


    [SConfig.Option]
    public bool PlayOverSound = false;

    [SConfig.Option]
    public bool ShowOverNotification = false;

    [SConfig.Option]
    public string OverSound = "ghost";
}
