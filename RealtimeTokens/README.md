# Realtime Framework

An API + Content Patcher tokens for handling Real-time events.

Other games get special holiday items around Halloween and Christmas, why not Stardew Valley?

## Available to Modders

### Patchable Content

`assets/holidays.json`

Contains data about holidays.  Add or remove holidays as you see fit.

### API

`bool IsHoliday(string holiday);`

Does the given holiday exist?

`bool IsComingHoliday(string holiday);`

Is the given holiday upcoming?

`bool IsCurrentHoliday(string holiday);`

Is the given holiday happening right now?

`bool IsPassingHoliday(string holiday);`

Has the given holiday just passed?

`IEnumberable<string> GetAllHolidays();`

Return all holidays.

`IEnumberable<string> GetComingHolidays();`

Return all upcoming holidays.

`IEnumerable<string> GetCurrentHolidays();`

Return all holidays happening right now.

`IEnumerable<string> GetPassingHolidays();`

Return all holidays which just passed.

`string GetLocalName(string holiday);`

Gets the localized name of a holiday.

### Content Patcher Tokens

`drbirbdev.RealtimeFramework_`

Suffix for all Tokens

```
Hour
DayOfMonth
DayOfWeek
DayOfYear
Month
Year
```

General purpose time related tokens.

```
WeekdayLocal
MonthLocal
```

Time related, localized strings.

```
AllHolidays
ComingHolidays
CurrentHolidays
PassingHolidays
```

Holiday related tokens, similar to the API.

```
AllHolidaysLocal
ComingHolidaysLocal
CurrentHolidaysLocal
PassingHolidaysLocal
```

Localized versions of holiday related tokens.

### Game State Queries

TODO: Add some GSQs which act similarly to the APIs.

### Event Preconditions

TODO: Add some Event Preconditions which act similarly to the APIs.

### TokenParser

TODO: Add some token parsers which handle localization


