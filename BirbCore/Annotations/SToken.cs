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
    protected static IContentPatcherApi Api;
    public override object Handle(Type type, IMod mod = null)
    {
        mod.Helper.Events.GameLoop.GameLaunched += (sender, e) =>
        {
            Api = mod.Helper.ModRegistry.GetApi<IContentPatcherApi>("Pathoschild.ContentPatcher");
            if (Api == null)
            {
                Log.Error("Content Patcher is not enabled, so will skip parsing");
                return;
            }
            base.Handle(type, mod);
        };

        return null;
    }

    public class Token : MethodHandler
    {
        public override void Handle(MethodInfo method, object instance, IMod mod = null)
        {
            Api.RegisterToken(mod.ModManifest, method.Name, method.CreateDelegate<Func<IEnumerable<string>>>(instance));
        }
    }

    public class AdvancedToken : ClassHandler
    {
        public override object Handle(Type type, IMod mod = null)
        {
            object instance = base.Handle(type, mod);

            Api.RegisterToken(mod.ModManifest, type.Name, instance);

            return instance;
        }
    }
}
