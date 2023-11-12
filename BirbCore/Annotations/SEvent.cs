using System;
using System.Reflection;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace BirbCore.Annotations;

/// <summary>
/// Specifies a method as a SMAPI event.  
/// </summary>
public class SEvent : ClassHandler
{

    public class GameLaunched : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.GameLaunched += (EventHandler<GameLaunchedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<GameLaunchedEventArgs>), method);
        }
    }

    public class UpdateTicking : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.UpdateTicking += (EventHandler<UpdateTickingEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<UpdateTickingEventArgs>), method);
        }
    }

    public class UpdateTicked : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.UpdateTicked += (EventHandler<UpdateTickedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<UpdateTickedEventArgs>), method);
        }
    }

    public class OneSecondUpdateTicking : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.OneSecondUpdateTicking += (EventHandler<OneSecondUpdateTickingEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<OneSecondUpdateTickingEventArgs>), method);
        }
    }

    public class OneSecondUpdateTicked : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.OneSecondUpdateTicked += (EventHandler<OneSecondUpdateTickedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<OneSecondUpdateTickedEventArgs>), method);
        }
    }

    public class SaveCreating : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.SaveCreating += (EventHandler<SaveCreatingEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<SaveCreatingEventArgs>), method);
        }
    }

    public class SaveCreated : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.SaveCreated += (EventHandler<SaveCreatedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<SaveCreatedEventArgs>), method);
        }
    }

    public class Saving : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.Saving += (EventHandler<SavingEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<SavingEventArgs>), method);
        }
    }

    public class Saved : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.Saved += (EventHandler<SavedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<SavedEventArgs>), method);
        }
    }

    public class SaveLoaded : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.SaveLoaded += (EventHandler<SaveLoadedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<SaveLoadedEventArgs>), method);
        }
    }

    public class DayStarted : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.DayStarted += (EventHandler<DayStartedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<DayStartedEventArgs>), method);
        }
    }

    public class DayEnding : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.DayEnding += (EventHandler<DayEndingEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<DayEndingEventArgs>), method);
        }
    }

    public class TimeChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.TimeChanged += (EventHandler<TimeChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<TimeChangedEventArgs>), method);
        }
    }

    public class ReturnedToTitle : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.ReturnedToTitle += (EventHandler<ReturnedToTitleEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<ReturnedToTitleEventArgs>), method);
        }
    }

    public class ButtonsChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Input.ButtonsChanged += (EventHandler<ButtonsChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<ButtonsChangedEventArgs>), method);
        }
    }

    public class ButtonPressed : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Input.ButtonPressed += (EventHandler<ButtonPressedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<ButtonPressedEventArgs>), method);
        }
    }

    public class ButtonReleased : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Input.ButtonReleased += (EventHandler<ButtonReleasedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<ButtonReleasedEventArgs>), method);
        }
    }

    public class CursorMoved : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Input.CursorMoved += (EventHandler<CursorMovedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<CursorMovedEventArgs>), method);
        }
    }

    public class MouseWheelScrolled : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Input.MouseWheelScrolled += (EventHandler<MouseWheelScrolledEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<MouseWheelScrolledEventArgs>), method);
        }
    }

    public class PeerContextReceived : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Multiplayer.PeerContextReceived += (EventHandler<PeerContextReceivedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<PeerContextReceivedEventArgs>), method);
        }
    }

    public class PeerConnected : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Multiplayer.PeerConnected += (EventHandler<PeerConnectedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<PeerConnectedEventArgs>), method);
        }
    }

    public class ModMessageReceived : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Multiplayer.ModMessageReceived += (EventHandler<ModMessageReceivedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<ModMessageReceivedEventArgs>), method);
        }
    }

    public class PeerDisconnected : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Multiplayer.PeerDisconnected += (EventHandler<PeerDisconnectedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<PeerDisconnectedEventArgs>), method);
        }
    }

    public class InventoryChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Player.InventoryChanged += (EventHandler<InventoryChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<InventoryChangedEventArgs>), method);
        }
    }

    public class LevelChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Player.LevelChanged += (EventHandler<LevelChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<LevelChangedEventArgs>), method);
        }
    }

    public class Warped : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Player.Warped += (EventHandler<WarpedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<WarpedEventArgs>), method);
        }
    }

    public class LocationListChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.World.LocationListChanged += (EventHandler<LocationListChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<LocationListChangedEventArgs>), method);
        }
    }

    public class BuildingListChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.World.BuildingListChanged += (EventHandler<BuildingListChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<BuildingListChangedEventArgs>), method);
        }
    }

    public class ChestInventoryChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.World.ChestInventoryChanged += (EventHandler<ChestInventoryChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<ChestInventoryChangedEventArgs>), method);
        }
    }

    public class DebrisListChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.World.DebrisListChanged += (EventHandler<DebrisListChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<DebrisListChangedEventArgs>), method);
        }
    }

    public class FurnitureListChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.World.FurnitureListChanged += (EventHandler<FurnitureListChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<FurnitureListChangedEventArgs>), method);
        }
    }

    public class LargeTerrainFeatureListChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.World.LargeTerrainFeatureListChanged += (EventHandler<LargeTerrainFeatureListChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<LargeTerrainFeatureListChangedEventArgs>), method);
        }
    }

    public class NpcListChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.World.NpcListChanged += (EventHandler<NpcListChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<NpcListChangedEventArgs>), method);
        }
    }

    public class ObjectListChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.World.ObjectListChanged += (EventHandler<ObjectListChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<ObjectListChangedEventArgs>), method);
        }
    }

    public class TerrainFeatureListChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.World.TerrainFeatureListChanged += (EventHandler<TerrainFeatureListChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<TerrainFeatureListChangedEventArgs>), method);
        }
    }

    public class MenuChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.MenuChanged += (EventHandler<MenuChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<MenuChangedEventArgs>), method);
        }
    }

    public class Rendering : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.Rendering += (EventHandler<RenderingEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<RenderingEventArgs>), method);
        }
    }

    public class Rendered : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.Rendered += (EventHandler<RenderedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<RenderedEventArgs>), method);
        }
    }

    public class RenderingWorld : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.RenderingWorld += (EventHandler<RenderingWorldEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<RenderingWorldEventArgs>), method);
        }
    }

    public class RenderedWorld : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.RenderedWorld += (EventHandler<RenderedWorldEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<RenderedWorldEventArgs>), method);
        }
    }

    public class RenderingActiveMenu : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.RenderingActiveMenu += (EventHandler<RenderingActiveMenuEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<RenderingActiveMenuEventArgs>), method);
        }
    }

    public class RenderedActiveMenu : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.RenderedActiveMenu += (EventHandler<RenderedActiveMenuEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<RenderedActiveMenuEventArgs>), method);
        }
    }

    public class RenderingHud : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.RenderingHud += (EventHandler<RenderingHudEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<RenderingHudEventArgs>), method);
        }
    }

    public class RenderedHud : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.RenderedHud += (EventHandler<RenderedHudEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<RenderedHudEventArgs>), method);
        }
    }

    public class WindowResized : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Display.WindowResized += (EventHandler<WindowResizedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<WindowResizedEventArgs>), method);
        }
    }

    public class AssetRequested : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Content.AssetRequested += (EventHandler<AssetRequestedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<AssetRequestedEventArgs>), method);
        }
    }

    public class AssetsInvalidated : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Content.AssetsInvalidated += (EventHandler<AssetsInvalidatedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<AssetsInvalidatedEventArgs>), method);
        }
    }

    public class AssetReady : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Content.AssetReady += (EventHandler<AssetReadyEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<AssetReadyEventArgs>), method);
        }
    }

    public class LocaleChanged : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            mod.Helper.Events.Content.LocaleChanged += (EventHandler<LocaleChangedEventArgs>)Delegate.CreateDelegate(typeof(EventHandler<LocaleChangedEventArgs>), method);
        }
    }

    public class ApisLoaded : MethodHandler
    {
        private MethodInfo Method;
        private IMod Mod;
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            this.Method = method;
            this.Mod = mod;

            mod.Helper.Events.GameLoop.GameLaunched += (object sender, GameLaunchedEventArgs e) =>
            {
                mod.Helper.Events.GameLoop.OneSecondUpdateTicked += OneSecondUpdateTicked;
            };
        }

        private void OneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            this.Mod.Helper.Events.GameLoop.OneSecondUpdateTicked -= OneSecondUpdateTicked;
            this.Method.Invoke(this.Mod, new object[] { e });
        }
    }

}
