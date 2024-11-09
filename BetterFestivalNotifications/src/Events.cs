using System;
using System.Collections.Generic;
using BirbCore.Attributes;
using StardewValley;
using StardewValley.GameData;
using StardewValley.TokenizableStrings;

namespace BetterFestivalNotifications;

[SEvent]
internal class Events
{
    private string _warnMessage;
    private string _endMessage;
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
            string festivalName = festivalData["name"];
            if (festivalName == "")
            {
                festivalName = ModEntry.Instance.I18N.Get("defaultFestivalName").ToString();
            }

            this._warnMessage = ModEntry.Instance.I18N.Get("festivalWarn", new { festival = festivalName }).ToString();
            this._endMessage = ModEntry.Instance.I18N.Get("festivalOver", new { festival = festivalName }).ToString();
            this._startTime = startTime;
            this._endTime = endTime;

            ModEntry.Instance.Helper.Events.GameLoop.TimeChanged += this.GameLoop_TimeChanged;
        }
        else if (Utility.TryGetPassiveFestivalDataForDay(
                     Game1.dayOfMonth,
                     Game1.season,
                     null,
                     out string _,
                     out PassiveFestivalData passiveFestivalData))
        {
            string festivalName = TokenParser.ParseText(passiveFestivalData.DisplayName);
            if (festivalName == "")
            {
                if (Game1.IsSummer && (Game1.dayOfMonth == 20 || Game1.dayOfMonth == 21))
                {
                    festivalName = Game1.content.LoadString("Strings\\1_6_Strings:TroutDerby");
                }
                else if (Game1.IsWinter && (Game1.dayOfMonth == 12 || Game1.dayOfMonth == 21))
                {
                    festivalName = Game1.content.LoadString("Strings\\1_6_Strings:SquidFest");
                }
                else
                {
                    festivalName = ModEntry.Instance.I18N.Get("defaultFestivalName").ToString();
                }
            }

            this._warnMessage = ModEntry.Instance.I18N.Get("festivalWarn", new { festival = festivalName }).ToString();
            this._endMessage = ModEntry.Instance.I18N.Get("festivalOver", new { festival = festivalName }).ToString();
            this._startTime = passiveFestivalData.StartTime;
            this._endTime = 2600;

            ModEntry.Instance.Helper.Events.GameLoop.TimeChanged += this.GameLoop_TimeChanged;
        }
        else if (Utility.getDaysOfBooksellerThisSeason().Contains(Game1.dayOfMonth))
        {
            string festivalName = Game1.content.LoadString("Strings\\1_6_Strings:Bookseller");

            this._warnMessage = ModEntry.Instance.I18N.Get("visitorWarn", new { visitor = festivalName }).ToString();
            this._endMessage = ModEntry.Instance.I18N.Get("visitorOver", new { visitor = festivalName }).ToString();
            this._startTime = 600;
            this._endTime = 2600;

            ModEntry.Instance.Helper.Events.GameLoop.TimeChanged += this.GameLoop_TimeChanged;
        }
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
                Game1.showGlobalMessage(this._warnMessage);
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
                Game1.showGlobalMessage(this._endMessage);
            }
        }
    }
}
