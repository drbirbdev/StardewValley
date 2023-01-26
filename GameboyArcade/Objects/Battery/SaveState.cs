using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyArcade
{
    public class SaveState
    {
        public int[] RAM { get; set; }
        public long[] ClockData { get; set; }
    }
}
