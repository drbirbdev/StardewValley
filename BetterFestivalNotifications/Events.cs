using System.Collections.Generic;
using System;
using BirbCore.Annotations;
using BirbCore;
using StardewValley.GameData;
using StardewValley;

namespace BetterFestivalNotifications;

[SEvent]
internal class Events
{

    private string FestivalName;
    private int StartTime;
    private int EndTime;

    [SEvent.DayEnding]
    private void GameLoop_DayEnding(object sender, StardewModdingAPI.Events.DayEndingEventArgs e)
    {
        ModEntry.Instance.Helper.Events.GameLoop.TimeChanged -= this.GameLoop_TimeChanged;
    }

    [SEvent.DayStarted]
    private void GameLoop_DayStarted(object sender, StardewModdingAPI.Events.DayStartedEventArgs e)
    {
        if (!Utility.isFestivalDay() && !Utility.IsPassiveFestivalDay())
        {
            return;
        }

        if (Utility.isFestivalDay())
        {
            Dictionary<string, string> festivalData = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + Game1.dayOfMonth);

            this.FestivalName = festivalData["name"];
            string startAndEnd = festivalData["conditions"].Split('/')[1];
            this.StartTime = Convert.ToInt32(ArgUtility.SplitBySpaceAndGet(startAndEnd, 0, "-1"));
            EndTime = Convert.ToInt32(ArgUtility.SplitBySpaceAndGet(startAndEnd, 1, "-1"));

            if (StartTime < 600 || StartTime >= 2600 || EndTime < 600 || EndTime > 2600)
            {
                Log.Warn("Festival start or end time is invalid");
                return;
            }
        }
        else
        {
            if (!Utility.TryGetPassiveFestivalDataForDay(Game1.dayOfMonth, Game1.season, out string id, out PassiveFestivalData data))
            {
                Log.Warn("Could not load passive festival name, start, and end time");
                return;

            }

            FestivalName = data.DisplayName;
            StartTime = data.StartTime;
            EndTime = 2600;
        }

        ModEntry.Instance.Helper.Events.GameLoop.TimeChanged += GameLoop_TimeChanged;
    }

    private void GameLoop_TimeChanged(object sender, StardewModdingAPI.Events.TimeChangedEventArgs e)
    {
        if (e.NewTime == StartTime)
        {
            if (ModEntry.Config.PlayStartSound)
            {
                Game1.playSound(ModEntry.Config.StartSound);
            }
        }
        else if (e.NewTime == this.EndTime - (100 * ModEntry.Config.WarnHoursAheadOfTime))
        {
            if (ModEntry.Config.PlayWarnSound)
            {
                Game1.playSound(ModEntry.Config.WarnSound);
            }
            if (ModEntry.Config.ShowWarnNotification)
            {
                Game1.showGlobalMessage(ModEntry.Instance.I18n.Get("festivalWarn", new { festival = this.FestivalName }));
            }
        }
        else if (e.NewTime == this.EndTime)
        {
            if (ModEntry.Config.PlayOverSound)
            {
                Game1.playSound(ModEntry.Config.OverSound);
            }
            if (ModEntry.Config.ShowOverNotification)
            {
                Game1.showGlobalMessage(ModEntry.Instance.I18n.Get("festivalOver", new { festival = this.FestivalName }));
            }
        }
    }
}
