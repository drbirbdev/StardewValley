#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using BirbCore.Extensions;
using HarmonyLib;
using StardewModdingAPI;

namespace BirbCore.Annotations;

public class SContent : ClassHandler
{
    public string FileName;
    public bool IsList;
    public bool IsDictionary;

    public SContent(string fileName = "content.json", bool isList = false, bool isDictionary = false)
    {
        this.FileName = fileName;
        this.IsList = isList;
        this.IsDictionary = isDictionary;
    }

    public override void Handle(Type type, object? instance, IMod mod)
    {
        Type innerType = type;
        if (this.IsList)
        {
            type = typeof(List<>).MakeGenericType(type);
        }
        else if (this.IsDictionary)
        {
            type = typeof(Dictionary<,>).MakeGenericType(typeof(string), type);
        }

        Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(string), type);

        MemberInfo modContent = mod.GetType().GetMemberOfType(dictionaryType);
        if (modContent is null)
        {
            Log.Error("Mod must define a Content dictionary property");
            return;
        }

        object contentDictionary = AccessTools.CreateInstance(dictionaryType);
        if (contentDictionary is null)
        {
            Log.Error("contentDictionary was null.  Underlying type might be static? Cannot initialize.");
            return;
        }

        foreach (IContentPack contentPack in mod.Helper.ContentPacks.GetOwned())
        {
            object? content = contentPack.GetType().GetMethod("ReadJsonFile")
                ?.MakeGenericMethod(type)
                .Invoke(contentPack, new object[] { this.FileName });

            if (content is null)
            {
                Log.Error($"{this.FileName} in content pack {contentPack.Manifest.UniqueID} was null");
                continue;
            }

            Dictionary<string, object> modContents = new();
            if (this.IsList)
            {
                int i = 0;
                foreach (object c in (List<object>)content)
                {
                    modContents.Add(i + "", c);
                    i++;
                }
            }
            else if (this.IsDictionary)
            {
                modContents = (Dictionary<string, object>)content;
            }
            else
            {
                modContents.Add("", content);
            }
            foreach (string contentId in modContents.Keys)
            {
                object contentValue = modContents[contentId];
                foreach (PropertyInfo propertyInfo in contentValue.GetType().GetProperties(ReflectionExtensions.AllDeclared))
                {
                    foreach (Attribute attribute in propertyInfo.GetCustomAttributes())
                    {
                        if (attribute is FieldHandler handler)
                        {
                            handler.Handle(propertyInfo, content, mod, new object[] { contentPack, contentId });
                        }
                    }
                }
                foreach (FieldInfo fieldInfo in contentValue.GetType().GetFields(ReflectionExtensions.AllDeclared))
                {
                    foreach (Attribute attribute in fieldInfo.GetCustomAttributes())
                    {
                        if (attribute is FieldHandler handler)
                        {
                            handler.Handle(fieldInfo, content, mod, new object[] { contentPack, contentId });
                        }
                    }
                }
            }


            string modId = contentPack.Manifest.UniqueID;

            contentDictionary.GetType().GetMethod("Add")
                ?.MakeGenericMethod(typeof(string), type)
                .Invoke(contentDictionary, new object[] { modId, content });
        }

        modContent.GetSetter()(mod, contentDictionary);

        return;
    }

    public class ModId : FieldHandler
    {
        public override void Handle(string name, Type fieldType, Func<object, object?> getter, Action<object, object> setter, object? instance, IMod mod, object[]? args = null)
        {
            if (instance is null)
            {
                Log.Error("Content instance might be static? Failing to add all content packs");
                return;
            }
            if (args?[0] == null)
            {
                Log.Error("Something went wrong in BirbCore Content Pack parsing");
                return;
            }
            setter(instance, ((IContentPack)args[0]).Manifest.UniqueID);
        }
    }

    public class UniqueId : FieldHandler
    {
        public override void Handle(string name, Type fieldType, Func<object, object?> getter, Action<object, object> setter, object? instance, IMod mod, object[]? args = null)
        {
            if (instance is null)
            {
                Log.Error("Content instance might be static? Failing to add all content packs");
                return;
            }
            if (args?[0] == null || args?[1] == null)
            {
                Log.Error("Something went wrong in BirbCore Content Pack parsing");
                return;
            }
            setter(instance, ((IContentPack)args[0]).Manifest.UniqueID + args[1]);
        }
    }

    public class ContentId : FieldHandler
    {
        public override void Handle(string name, Type fieldType, Func<object, object?> getter, Action<object, object> setter, object? instance, IMod mod, object[]? args = null)
        {
            if (instance is null)
            {
                Log.Error("Content instance might be static? Failing to add all content packs");
                return;
            }
            if (args?[1] == null)
            {
                Log.Error("Something went wrong in BirbCore Content Pack parsing");
                return;
            }
            setter(instance, args[1]);
        }
    }

    public class ContentPack : FieldHandler
    {
        public override void Handle(string name, Type fieldType, Func<object, object?> getter, Action<object, object> setter, object? instance, IMod mod, object[]? args = null)
        {
            if (instance is null)
            {
                Log.Error("Content instance might be static? Failing to add all content packs");
                return;
            }
            if (args?[0] == null)
            {
                Log.Error("Something went wrong in BirbCore Content Pack parsing");
                return;
            }
            if (fieldType != typeof(IContentPack))
            {
                Log.Error("ContentPack attribute can only set value to field or property of type IContentPack");
                return;
            }
            setter(instance, args[0]);
        }
    }
}
