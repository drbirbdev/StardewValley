using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirbShared.Config;
using StardewModdingAPI;

namespace LookToTheSky
{
    [ConfigClass]
    class Config
    {
        [ConfigOption]
        public SButton Button { get; set; } = SButton.U;

        [ConfigOption]
        public int SpawnChancePerSecond { get; set; } = 5;
    }
}
