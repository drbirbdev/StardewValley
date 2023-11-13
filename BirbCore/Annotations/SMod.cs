using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace BirbCore.Annotations;
public class SMod : ClassHandler
{

    public class Api : FieldHandler
    {
        public string UniqueID;
        public bool IsRequired;

        public Api(string uniqueID, bool isRequired = true)
        {
            this.UniqueID = uniqueID;
            this.IsRequired = isRequired;
        }

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod = null, object[] args = null)
        {
            mod.Helper.Events.GameLoop.GameLaunched += (object sender, GameLaunchedEventArgs e) =>
            {
                object api = mod.Helper.ModRegistry.GetType().GetMethod("GetApi", 1, new Type[] { typeof(string) })
                    .MakeGenericMethod(fieldType)
                    .Invoke(mod.Helper.ModRegistry, new object[] { UniqueID });
                if (api is null && IsRequired)
                {
                    Log.Error($"[{name}] Can't access required API");
                }
                setter(instance, api);
            };
        }
    }

    public class Instance : FieldHandler
    {
        public override void Handle(string name, Type fieldType, Func<object, object> getter, Action<object, object> setter, object instance, IMod mod = null, object[] args = null)
        {
            setter(instance, mod);
        }
    }
}
