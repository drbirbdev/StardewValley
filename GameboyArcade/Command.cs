using BirbCore;
using BirbCore.Annotations;

namespace GameboyArcade;

[SCommand("gameboy")]
class Command
{
    [SCommand.Command("Play a loaded rom")]
    public static void Play(string game)
    {
        Content gameToPlay = ModEntry.SearchGames(game);

        if (gameToPlay is null)
        {
            Log.Error($"Could not find game {game}");
            return;
        }
        GameboyMinigame.LoadGame(gameToPlay);
    }

    [SCommand.Command("List all loaded roms")]
    public static void List()
    {
        foreach (Content content in ModEntry.AllGames())
        {
            Log.Info(content.Name);
        }
    }
}
