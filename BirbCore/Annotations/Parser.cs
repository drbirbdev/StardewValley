#nullable enable
using System;
using System.Reflection;
using BirbCore.Extensions;
using HarmonyLib;
using StardewModdingAPI;

namespace BirbCore.Annotations;
public class Parser
{
    /// <summary>
    /// Parse a SMAPI mod for BirbCore attributes. Does a variety of setup for SMAPI and Stardew Valley related objects,
    /// including setting up Assets with the content pipeline, creating command line commands, initializing config files,
    /// loading and saving data to properties, creating events and delegates, initializing APIs, and more.
    /// </summary>
    /// <param name="mod">The mod being parsed.</param>
    /// <param name="assembly">The assembly to scan for attributes. Defaults to the assembly calling ParseAll.</param>
    public static void ParseAll(IMod mod, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        Log.Init(mod.Monitor);

        foreach (Type type in assembly.GetTypes())
        {
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                if (attribute is ClassHandler handler)
                {
                    handler.Handle(type, null, mod);
                }
            }
        }

        foreach (FieldInfo field in mod.GetType().GetFields(ReflectionExtensions.AllDeclared))
        {
            foreach (Attribute attribute in field.GetCustomAttributes())
            {
                if (attribute is FieldHandler handler)
                {
                    handler.Handle(field, mod, mod);
                }
            }
        }

        foreach (PropertyInfo property in mod.GetType().GetProperties(ReflectionExtensions.AllDeclared))
        {
            foreach (Attribute attribute in property.GetCustomAttributes())
            {
                if (attribute is FieldHandler handler)
                {
                    handler.Handle(property, mod, mod);
                }
            }
        }

        foreach (MethodInfo method in mod.GetType().GetMethods(ReflectionExtensions.AllDeclared))
        {
            foreach (Attribute attribute in method.GetCustomAttributes())
            {
                if (attribute is MethodHandler handler)
                {
                    handler.Handle(method, mod, mod);
                }
            }
        }

        new Harmony(mod.ModManifest.UniqueID).PatchAll(assembly);
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public abstract class ClassHandler : Attribute
{
    public virtual void Handle(Type type, object? instance, IMod mod)
    {
        foreach (FieldInfo fieldInfo in type.GetFields(ReflectionExtensions.AllDeclared))
        {
            foreach (Attribute attribute in fieldInfo.GetCustomAttributes())
            {
                if (attribute is FieldHandler handler)
                {
                    handler.Handle(fieldInfo, instance, mod);
                }
            }
        }
        foreach (PropertyInfo propertyInfo in type.GetProperties(ReflectionExtensions.AllDeclared))
        {
            foreach (Attribute attribute in propertyInfo.GetCustomAttributes())
            {
                if (attribute is FieldHandler handler)
                {
                    handler.Handle(propertyInfo, instance, mod);
                }
            }
        }
        foreach (MethodInfo method in type.GetMethods(ReflectionExtensions.AllDeclared))
        {
            foreach (Attribute attribute in method.GetCustomAttributes())
            {
                if (attribute is MethodHandler handler)
                {
                    handler.Handle(method, instance, mod);
                }
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public abstract class MethodHandler : Attribute
{
    public abstract void Handle(MethodInfo method, object? instance, IMod mod);
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public abstract class FieldHandler : Attribute
{
    public void Handle(FieldInfo fieldInfo, object? instance, IMod mod, object[]? args = null)
    {
        this.Handle(fieldInfo.Name, fieldInfo.FieldType, fieldInfo.GetValue, fieldInfo.SetValue, instance, mod, args);
    }

    public void Handle(PropertyInfo propertyInfo, object? instance, IMod mod, object[]? args = null)
    {
        this.Handle(propertyInfo.Name, propertyInfo.PropertyType, propertyInfo.GetValue, propertyInfo.SetValue, instance, mod, args);
    }

    public abstract void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object? instance, IMod mod, object[]? args = null);
}
