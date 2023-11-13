using BirbCore;
using BirbCore.Annotations;
using Microsoft.Xna.Framework;
using StardewValley;

namespace GameboyArcade;

[SDelegate]
internal class Delegates
{

    [SDelegate.TileAction]
    public bool Play(GameLocation gameLocation, string[] args, Farmer farmer, Point point)
    {
        Content content = ModEntry.GetGame(args?[0], args?[1]);
        Utilities.ShowArcadeMenu(content.UniqueID, content.Name);
        return true;
    }

    [SDelegate.EventCommand]
    public void StartGame(Event @event, string[] args, EventContext context)
    {
        if (Game1.currentMinigame != null)
        {
            return;
        }

        Content content = ModEntry.GetGame(args?[0], args?[1]);
        if (content is null)
        {
            Log.Error($"Minigame [{args?[0]} {args?[1]}] does not exist.");
            return;
        }

        if (!content.EnableEvents)
        {
            Log.Error($"Event is attempting to use minigame {content.UniqueID} in a cutscene, but that content pack has disallowed this.");
            return;
        }

        GameboyMinigame.LoadGame(content, true);
        @event.CurrentCommand++;

    }

}
