using System;
using System.Collections.Generic;
using BirbShared;
using BirbShared.Mod;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData;

namespace BetterFestivalNotifications
{
    public class ModEntry : Mod
    {
        [SmapiInstance]
        internal static ModEntry Instance;
        [SmapiConfig]
        internal static Config Config;

        internal ITranslationHelper I18n => this.Helper.Translation;

        private string FestivalName;
        private int StartTime;
        private int EndTime;

        public override void Entry(IModHelper helper)
        {
            ModClass mod = new ModClass();
            mod.Parse(this);
            this.Helper.Events.GameLoop.DayStarted += this.GameLoop_DayStarted;
            this.Helper.Events.GameLoop.DayEnding += this.GameLoop_DayEnding;
        }

        private void GameLoop_DayEnding(object sender, StardewModdingAPI.Events.DayEndingEventArgs e)
        {
            this.Helper.Events.GameLoop.TimeChanged -= GameLoop_TimeChanged;
        }

        private void GameLoop_DayStarted(object sender, StardewModdingAPI.Events.DayStartedEventArgs e)
        {
            if (!Utility.isFestivalDay() && !Utility.IsPassiveFestivalDay())
            {
                return;
            }

            if (Utility.isFestivalDay())
            {
                Dictionary<string, string> festivalData = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + Game1.dayOfMonth);

                FestivalName = festivalData["name"];
                string startAndEnd = festivalData["conditions"].Split('/')[1];
                StartTime = Convert.ToInt32(ArgUtility.SplitBySpaceAndGet(startAndEnd, 0, "-1"));
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
          
            this.Helper.Events.GameLoop.TimeChanged += GameLoop_TimeChanged;
        }

        private void GameLoop_TimeChanged(object sender, StardewModdingAPI.Events.TimeChangedEventArgs e)
        {
            if (e.NewTime == StartTime)
            {
                if (Config.PlayStartSound)
                {
                    Game1.playSound(Config.StartSound);
                }
            }
            else if (e.NewTime == EndTime - (100 * Config.WarnHoursAheadOfTime))
            {
                if (Config.PlayWarnSound)
                {
                    Game1.playSound(Config.WarnSound);
                }
                if (Config.ShowWarnNotification)
                {
                    Game1.showGlobalMessage(I18n.Get("festivalWarn", new { festival = FestivalName }));
                }
            }
            else if (e.NewTime == EndTime)
            {
                if (Config.PlayOverSound)
                {
                    Game1.playSound(Config.OverSound);
                }
                if (Config.ShowOverNotification)
                {
                    Game1.showGlobalMessage(I18n.Get("festivalOver", new { festival = FestivalName }));
                }
            }
        }
    }
}
