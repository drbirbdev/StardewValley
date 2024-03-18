using System.Collections.Generic;
using BirbCore.Attributes;

namespace RealtimeFramework;

[SAsset]
class Assets
{
    [SAsset.Asset("assets/holidays.json")]
    public Dictionary<string, HolidayModel> Holidays;
}

class HolidayModel
{
    public int ComingDays = 7;
    public int PassingDays = 1;

    public int StartDelayHours = 0;

    public int EndDelayHours = 0;

    public int[] Date = [1, 1];
    public Dictionary<string, int[]> VaryingDates = null;
}
