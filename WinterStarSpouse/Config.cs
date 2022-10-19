using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirbShared.Config;

namespace WinterStarSpouse
{
    [ConfigClass]
    internal class Config
    {

        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int SpouseIsRecipientChance { get; set; } = 50;

        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int SpouseIsGiverChance { get; set; } = 50;
    }
}
