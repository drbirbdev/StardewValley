using System;
using System.Reflection;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace BirbCore.Annotations;
public class SMod : ClassHandler
{

    public class Api : PropertyHandler
    {
        public string UniqueID;
        public bool IsRequired;

        public Api(string uniqueID, bool isRequired = true)
        {
            this.UniqueID = uniqueID;
            this.IsRequired = isRequired;
        }

        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.GameLaunched += (object sender, GameLaunchedEventArgs e) =>
            {
                object api = mod.Helper.ModRegistry.GetType().GetMethod("GetApi", 1, new Type[] { typeof(string) })
                    .MakeGenericMethod(property.PropertyType)
                    .Invoke(mod.Helper.ModRegistry, new object[] { UniqueID });
                if (api is null && IsRequired)
                {
                    Log.Error($"[{property.Name}] Can't access required API");
                }
                property.SetValue(instance, api);
            };
        }
    }
}
