using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using StardewModdingAPI;

namespace BirbCore.Annotations;

public class SContent : ClassHandler
{
    public string FileName = "content.json";

    protected PropertyInfo ModContent;

    public override object Handle(Type type, IMod mod = null)
    {
        Type dictionaryType = Type.GetType("Dictionary").MakeGenericType(typeof(string), type);

        this.ModContent = mod.GetType().GetProperties().Where(p => p.PropertyType == dictionaryType).First();
        if (this.ModContent == null)
        {
            Log.Error("Mod must define a Content dictionary property");
            return null;
        }

        object contentDictionary = AccessTools.CreateInstance(dictionaryType);

        foreach (IContentPack contentPack in mod.Helper.ContentPacks.GetOwned())
        {
            object content = contentPack.GetType().GetMethod("ReadJsonFile")
                .MakeGenericMethod(dictionaryType)
                .Invoke(contentPack, new object[] { FileName });

            string modId = contentPack.Manifest.UniqueID;

            contentDictionary.GetType().GetMethod("Add")
                .MakeGenericMethod(typeof(string), type)
                .Invoke(contentDictionary, new object[] { modId, content });
        }

        ModContent.SetValue(mod, contentDictionary);

        return contentDictionary;
    }
}
