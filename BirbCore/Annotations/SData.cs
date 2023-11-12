using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using StardewModdingAPI;

namespace BirbCore.Annotations;

public class SData : ClassHandler
{
    protected PropertyInfo ModData;

    public override object Handle(Type type, IMod mod = null)
    {
        this.ModData = mod.GetType().GetProperties().Where(p => p.PropertyType == type).First();
        if (this.ModData == null)
        {
            Log.Error("Mod must define a data property");
            return null;
        }

        object instance = base.Handle(type, mod);
        this.ModData.SetValue(mod, instance);
        return instance;
    }

    public class SaveData : PropertyHandler
    {
        public string Key;

        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.SaveLoaded += (object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e) =>
            {
                object saveData = mod.Helper.Data.GetType().GetMethod("ReadSaveData")
                    .MakeGenericMethod(property.PropertyType)
                    .Invoke(mod.Helper.Data, new object[] { Key });

                property.SetValue(instance, saveData);
            };

            mod.Helper.Events.GameLoop.SaveCreated += (object sender, StardewModdingAPI.Events.SaveCreatedEventArgs e) =>
            {
                object saveData = AccessTools.CreateInstance(property.PropertyType);

                property.SetValue(instance, saveData);
            };

            mod.Helper.Events.GameLoop.DayEnding += (object sender, StardewModdingAPI.Events.DayEndingEventArgs e) =>
            {
                object saveData = property.GetValue(instance);

                mod.Helper.Data.WriteSaveData(Key, saveData);
            };
        }
    }

    public class LocalData : PropertyHandler
    {
        public string JsonFile;

        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.GameLaunched += (object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e) =>
            {
                object localData = mod.Helper.Data.GetType().GetMethod("ReadJsonFile")
                    .MakeGenericMethod(property.PropertyType)
                    .Invoke(mod.Helper.Data, new object[] { JsonFile });

                if (localData is null)
                {
                    localData = AccessTools.CreateInstance(property.PropertyType);
                }

                property.SetValue(instance, localData);
            };

            mod.Helper.Events.GameLoop.DayEnding += (object sender, StardewModdingAPI.Events.DayEndingEventArgs e) =>
            {
                object localData = property.GetValue(instance);

                mod.Helper.Data.WriteJsonFile(JsonFile, localData);
            };
        }
    }

    public class GlobalData : PropertyHandler
    {
        public string Key;

        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            mod.Helper.Events.GameLoop.GameLaunched += (object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e) =>
            {
                object globalData = mod.Helper.Data.GetType().GetMethod("ReadGlobalData")
                    .MakeGenericMethod(property.PropertyType)
                    .Invoke(mod.Helper.Data, new object[] { Key });

                if (globalData is null)
                {
                    globalData = AccessTools.CreateInstance(property.PropertyType);
                }

                property.SetValue(instance, globalData);
            };

            mod.Helper.Events.GameLoop.DayEnding += (object sender, StardewModdingAPI.Events.DayEndingEventArgs e) =>
            {
                object globalData = property.GetValue(instance);

                mod.Helper.Data.WriteGlobalData(Key, globalData);
            };
        }
    }

}
