using System;
using System.Collections.Generic;
using BirbCore;
using BirbCore.Annotations;
using BirbCore.APIs;
using StardewModdingAPI;

namespace RealtimeFramework;

public class ModEntry : Mod
{
    internal static ModEntry Instance;
    internal static Assets Assets;
    [SMod.Api("Pathoschild.ContentPatcher", false)]
    internal static IContentPatcherApi ContentPatcher;

    internal ITranslationHelper I18n => this.Helper.Translation;

    internal static IRealtimeAPI API = new RealtimeAPI();

    public override void Entry(IModHelper helper)
    {
        Parser.ParseAll(this);

        helper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;
    }

    public override object GetApi()
    {
        return API;
    }

    private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
    {
        if (ContentPatcher == null)
        {
            Log.Info("Content Patcher is not installed, will skip adding tokens");
            return;
        }
        ContentPatcher.RegisterToken(Instance.ModManifest, "Hour", this.RegisterHourToken);
        ContentPatcher.RegisterToken(Instance.ModManifest, "DayOfMonth", this.RegisterDayOfMonth);
        ContentPatcher.RegisterToken(Instance.ModManifest, "DayOfWeek", this.RegisterDayOfWeek);
        ContentPatcher.RegisterToken(Instance.ModManifest, "DayOfYear", this.RegisterDayOfYear);
        ContentPatcher.RegisterToken(Instance.ModManifest, "Month", this.RegisterMonth);
        ContentPatcher.RegisterToken(Instance.ModManifest, "Year", this.RegisterYear);
        ContentPatcher.RegisterToken(Instance.ModManifest, "WeekdayLocal", this.RegisterWeekdayLocal);
        ContentPatcher.RegisterToken(Instance.ModManifest, "MonthLocal", this.RegisterMonthLocal);

        ContentPatcher.RegisterToken(Instance.ModManifest, "AllHolidays", this.RegisterAllHolidays);
        ContentPatcher.RegisterToken(Instance.ModManifest, "ComingHolidays", this.RegisterComingHolidays);
        ContentPatcher.RegisterToken(Instance.ModManifest, "CurrentHolidays", this.RegisterCurrentHolidays);
        ContentPatcher.RegisterToken(Instance.ModManifest, "PassingHolidays", this.RegisterPassingHolidays);
        ContentPatcher.RegisterToken(Instance.ModManifest, "AllHolidaysLocal", this.RegisterAllHolidaysLocal);
        ContentPatcher.RegisterToken(Instance.ModManifest, "ComingHolidaysLocal", this.RegisterComingHolidaysLocal);
        ContentPatcher.RegisterToken(Instance.ModManifest, "CurrentHolidaysLocal", this.RegisterCurrentHolidaysLocal);
        ContentPatcher.RegisterToken(Instance.ModManifest, "PassingHolidaysLocal", this.RegisterPassingHolidaysLocal);
    }

    private IEnumerable<string> RegisterHourToken()
    {
        yield return "" + DateTime.Now.Hour;
    }

    private IEnumerable<string> RegisterDayOfMonth()
    {
        yield return "" + DateTime.Today.Day;
    }

    private IEnumerable<string> RegisterDayOfWeek()
    {
        yield return "" + ((int)DateTime.Today.DayOfWeek + 1);
    }

    private IEnumerable<string> RegisterDayOfYear()
    {
        yield return "" + DateTime.Today.DayOfYear;
    }

    private IEnumerable<string> RegisterMonth()
    {
        yield return "" + DateTime.Today.Month;
    }

    private IEnumerable<string> RegisterYear()
    {
        yield return "" + DateTime.Today.Year;
    }

    private IEnumerable<string> RegisterWeekdayLocal()
    {
        yield return Instance.I18n.Get("time.weekday." + DateTime.Today.DayOfWeek);
    }

    private IEnumerable<string> RegisterMonthLocal()
    {
        yield return Instance.I18n.Get("time.month." + DateTime.Today.Month);
    }

    // Because Content Patcher treats null and empty arrays as an unready token, we need to return
    // a single empty string iff we would otherwise return an empty set of values.
    private IEnumerable<string> RegisterAllHolidays()
    {
        bool empty = true;
        foreach (string holiday in API.GetAllHolidays())
        {
            empty = false;
            yield return holiday;
        }
        if (empty)
        {
            yield return "";
        }
    }

    private IEnumerable<string> RegisterComingHolidays()
    {
        bool empty = true;
        foreach (string holiday in API.GetComingHolidays())
        {
            empty = false;
            yield return holiday;
        }
        if (empty)
        {
            yield return "";
        }
    }

    private IEnumerable<string> RegisterCurrentHolidays()
    {
        bool empty = true;
        foreach (string holiday in API.GetCurrentHolidays())
        {
            empty = false;
            yield return holiday;
        }
        if (empty)
        {
            yield return "";
        }
    }

    private IEnumerable<string> RegisterPassingHolidays()
    {
        bool empty = true;
        foreach (string holiday in API.GetPassingHolidays())
        {
            empty = false;
            yield return holiday;
        }
        if (empty)
        {
            yield return "";
        }
    }

    private IEnumerable<string> RegisterAllHolidaysLocal()
    {
        bool empty = true;
        foreach (string holiday in API.GetAllHolidays())
        {
            empty = false;
            yield return API.GetLocalName(holiday);
        }
        if (empty)
        {
            yield return "";
        }
    }


    private IEnumerable<string> RegisterComingHolidaysLocal()
    {
        bool empty = true;
        foreach (string holiday in API.GetComingHolidays())
        {
            empty = false;
            yield return API.GetLocalName(holiday);
        }
        if (empty)
        {
            yield return "";
        }
    }

    private IEnumerable<string> RegisterCurrentHolidaysLocal()
    {
        bool empty = true;
        foreach (string holiday in API.GetCurrentHolidays())
        {
            empty = false;
            yield return API.GetLocalName(holiday);
        }
        if (empty)
        {
            yield return "";
        }
    }

    private IEnumerable<string> RegisterPassingHolidaysLocal()
    {
        bool empty = true;
        foreach (string holiday in API.GetPassingHolidays())
        {
            empty = false;
            yield return API.GetLocalName(holiday);
        }
        if (empty)
        {
            yield return "";
        }
    }
}
