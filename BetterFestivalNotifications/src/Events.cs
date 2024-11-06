using System;
using System.Collections.Generic;
using BirbCore.Attributes;
using StardewValley;
using StardewValley.GameData;

namespace BetterFestivalNotifications;

[SEvent]
internal class Events
{

    private string _festivalName;
    private int _startTime;
    private int _endTime;

    [SEvent.DayEnding]
    private void GameLoop_DayEnding(object sender, StardewModdingAPI.Events.DayEndingEventArgs e)
    {
        ModEntry.Instance.Helper.Events.GameLoop.TimeChanged -= this.GameLoop_TimeChanged;
    }

    [SEvent.DayStarted]
    private void GameLoop_DayStarted(object sender, StardewModdingAPI.Events.DayStartedEventArgs e)
    {
        string key = Game1.currentSeason + Game1.dayOfMonth;

        if (Event.tryToLoadFestivalData(key,
                out string _,
                out Dictionary<string, string> festivalData,
                out string _,
                out int startTime,
                out int endTime))
        {
            this._festivalName = festivalData["name"];
            this._startTime = startTime;
            this._endTime = endTime;

            ModEntry.Instance.Helper.Events.GameLoop.TimeChanged += this.GameLoop_TimeChanged;
            return;
        }

        if (!Utility.TryGetPassiveFestivalDataForDay(
                Game1.dayOfMonth,
                Game1.season,
                null,
                out string _,
                out PassiveFestivalData passiveFestivalData))
        {
            return;
        }

        this._festivalName = passiveFestivalData.DisplayName;
        this._startTime = passiveFestivalData.StartTime;
        this._endTime = 2600;

        ModEntry.Instance.Helper.Events.GameLoop.TimeChanged += this.GameLoop_TimeChanged;
    }

    private void GameLoop_TimeChanged(object sender, StardewModdingAPI.Events.TimeChangedEventArgs e)
    {
        if (e.NewTime == this._startTime)
        {
            if (ModEntry.Config.PlayStartSound)
            {
                Game1.playSound(ModEntry.Config.StartSound);
            }
        }
        else if (e.NewTime == this._endTime - (100 * ModEntry.Config.WarnHoursAheadOfTime))
        {
            if (ModEntry.Config.PlayWarnSound)
            {
                Game1.playSound(ModEntry.Config.WarnSound);
            }
            if (ModEntry.Config.ShowWarnNotification)
            {
                Game1.showGlobalMessage(ModEntry.Instance.I18N.Get("festivalWarn", new { festival = this._festivalName }));
            }
        }
        else if (e.NewTime == this._endTime)
        {
            if (ModEntry.Config.PlayOverSound)
            {
                Game1.playSound(ModEntry.Config.OverSound);
            }
            if (ModEntry.Config.ShowOverNotification)
            {
                Game1.showGlobalMessage(ModEntry.Instance.I18N.Get("festivalOver", new { festival = this._festivalName }));
            }
        }
    }
}
