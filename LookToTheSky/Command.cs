using BirbShared.Command;

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
