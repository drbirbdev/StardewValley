using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirbShared.Command;
using StardewValley;

namespace LookToTheSky
{
    [CommandClass]
    class Command
    {

        [CommandMethod("Add a bird to the sky")]
        public static void AddBird(int yPos = 100, bool moveRight = true)
        {
            ModEntry.Instance.SkyObjects.Add(new Bird(yPos, moveRight));
        }
    }
}
