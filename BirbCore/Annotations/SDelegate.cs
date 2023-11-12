using System;
using System.Reflection;
using StardewValley;
using StardewModdingAPI;
using StardewValley.Internal;
using StardewValley.Delegates;
using Microsoft.Xna.Framework;

namespace BirbCore.Annotations;

public class SDelegate : ClassHandler
{

    public class EventCommand : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            Event.RegisterCustomCommand($"{mod.ModManifest.UniqueID}_{method.Name}", (EventCommandDelegate)Delegate.CreateDelegate(typeof(EventCommandDelegate), method));
        }
    }

    public class EventPrecondition : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            Event.RegisterCustomPrecondition($"{mod.ModManifest.UniqueID}_{method.Name}", (EventPreconditionDelegate)Delegate.CreateDelegate(typeof(EventPreconditionDelegate), method));
        }
    }

    public class GameStateQuery : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            StardewValley.GameStateQuery.Register($"{mod.ModManifest.UniqueID}_{method.Name}", (GameStateQueryDelegate)Delegate.CreateDelegate(typeof(GameStateQueryDelegate), method));
        }
    }

    public class ResolveItemQuery : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            ItemQueryResolver.ItemResolvers.Add($"{mod.ModManifest.UniqueID}_{method.Name}", (ResolveItemQueryDelegate)Delegate.CreateDelegate(typeof(ResolveItemQueryDelegate), method));
        }
    }

    public class TokenParser : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            StardewValley.TokenParser.RegisterParser($"{mod.ModManifest.UniqueID}_{method.Name}", (TokenParserDelegate)Delegate.CreateDelegate(typeof(TokenParserDelegate), method));
        }
    }

    public class TouchAction : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            GameLocation.RegisterTouchAction($"{mod.ModManifest.UniqueID}_{method.Name}", (Action<GameLocation, string[], Farmer, Vector2>)Delegate.CreateDelegate(typeof(Action<GameLocation, string[], Farmer, Vector2>), method));
        }
    }

    public class TileAction : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            GameLocation.RegisterTileAction($"{mod.ModManifest.UniqueID}_{method.Name}", (Func<GameLocation, string[], Farmer, Point, bool>)Delegate.CreateDelegate(typeof(Func<GameLocation, string[], Farmer, Point, bool>), method));
        }
    }
}
