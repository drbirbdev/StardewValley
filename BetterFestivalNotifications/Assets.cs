using System.Collections.Generic;
using BirbShared.Asset;

namespace BetterFestivalNotifications
{
    [AssetClass]
    internal class Assets
    {
        [AssetProperty("assets/festivalnotifications.json")]
        public Dictionary<string, List<int>> FestivalNotifications { get; set; }
    }
}
