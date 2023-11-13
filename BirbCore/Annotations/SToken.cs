#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using BirbCore.APIs;
using StardewModdingAPI;

namespace BirbCore.Annotations;

/// <summary>
/// Specifies a method or class as a content patcher simple or advanced token.
/// </summary>
public class SToken : ClassHandler
{
    private static IContentPatcherApi? Api;
    public override void Handle(Type type, object? instance, IMod mod, object[]? args = null)
    {
        mod.Helper.Events.GameLoop.GameLaunched += (sender, e) =>
        {
            Api = mod.Helper.ModRegistry.GetApi<IContentPatcherApi>("Pathoschild.ContentPatcher");
            if (Api == null)
            {
                Log.Error("Content Patcher is not enabled, so will skip parsing");
                return;
            }
            base.Handle(type, null, mod);
        };

        return;
    }

    public class Token : MethodHandler
    {
        public override void Handle(MethodInfo method, object? instance, IMod mod, object[]? args = null)
        {
            if (Api == null)
            {
                Log.Error("Content Patcher is not enabled, so will skip parsing");
                return;
            }
            Api.RegisterToken(mod.ModManifest, method.Name, method.CreateDelegate<Func<IEnumerable<string>>>(instance));
        }
    }

    public class AdvancedToken : ClassHandler
    {
        public override void Handle(Type type, object? instance, IMod mod, object[]? args = null)
        {
            instance = Activator.CreateInstance(type);
            if (instance is null)
            {
                Log.Error("Content Patcher advanced api requires an instance of token class. Provided token class may be static?");
                return;
            }
            base.Handle(type, instance, mod);
            if (Api == null)
            {
                Log.Error("Content Patcher is not enabled, so will skip parsing");
                return;
            }
            Api.RegisterToken(mod.ModManifest, type.Name, instance);

            return;
        }
    }
}
