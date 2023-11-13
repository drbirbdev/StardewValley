#nullable enable
using System;
using System.Reflection;
using BirbCore.Extensions;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Internal;

namespace BirbCore.Annotations;

public class SDelegate : ClassHandler
{

    public class EventCommand : MethodHandler
    {
        public override void Handle(MethodInfo method, object? instance, IMod mod, object[]? args = null)
        {
            Event.RegisterCustomCommand($"{mod.ModManifest.UniqueID}_{method.Name}", method.InitDelegate<EventCommandDelegate>(instance));
        }
    }

    public class EventPrecondition : MethodHandler
    {
        public override void Handle(MethodInfo method, object? instance, IMod mod, object[]? args = null)
        {
            Event.RegisterCustomPrecondition($"{mod.ModManifest.UniqueID}_{method.Name}", method.InitDelegate<EventPreconditionDelegate>(instance));
        }
    }

    public class GameStateQuery : MethodHandler
    {
        public override void Handle(MethodInfo method, object? instance, IMod mod, object[]? args = null)
        {
            StardewValley.GameStateQuery.Register($"{mod.ModManifest.UniqueID}_{method.Name}", method.InitDelegate<GameStateQueryDelegate>(instance));
        }
    }

    public class ResolveItemQuery : MethodHandler
    {
        public override void Handle(MethodInfo method, object? instance, IMod mod, object[]? args = null)
        {
            ItemQueryResolver.ItemResolvers.Add($"{mod.ModManifest.UniqueID}_{method.Name}", method.InitDelegate<ResolveItemQueryDelegate>(instance));
        }
    }

    public class TokenParser : MethodHandler
    {
        public override void Handle(MethodInfo method, object? instance, IMod mod, object[]? args = null)
        {
            StardewValley.TokenParser.RegisterParser($"{mod.ModManifest.UniqueID}_{method.Name}", method.InitDelegate<TokenParserDelegate>(instance));
        }
    }

    public class TouchAction : MethodHandler
    {
        public override void Handle(MethodInfo method, object? instance, IMod mod, object[]? args = null)
        {
            GameLocation.RegisterTouchAction($"{mod.ModManifest.UniqueID}_{method.Name}", method.InitDelegate<Action<GameLocation, string[], Farmer, Vector2>>(instance));
        }
    }

    public class TileAction : MethodHandler
    {
        public override void Handle(MethodInfo method, object? instance, IMod mod, object[]? args = null)
        {
            GameLocation.RegisterTileAction($"{mod.ModManifest.UniqueID}_{method.Name}", method.InitDelegate<Func<GameLocation, string[], Farmer, Point, bool>>(instance));
        }
    }


}
