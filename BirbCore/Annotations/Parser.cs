using System;
using System.Reflection;
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
    public static void ParseAll(IMod mod, Assembly assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        foreach (Type type in assembly.GetTypes())
        {
            foreach (Attribute attribute in type.GetCustomAttributes(true))
            {
                if (attribute is ClassHandler handler)
                {
                    handler.Handle(type, mod);
                }
            }
        }

        foreach (PropertyInfo property in mod.GetType().GetProperties())
        {
            foreach (Attribute attribute in property.GetCustomAttributes())
            {
                if (attribute is PropertyHandler handler)
                {
                    handler.Handle(property, mod, mod);
                }
            }
        }

        foreach (MethodInfo method in mod.GetType().GetMethods())
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
    public virtual object Handle(Type type, IMod mod = null)
    {
        object instance = Activator.CreateInstance(type);

        foreach (PropertyInfo property in type.GetProperties())
        {
            foreach (Attribute attribute in property.GetCustomAttributes())
            {
                if (attribute is PropertyHandler handler)
                {
                    handler.Handle(property, instance, mod);
                }
            }
        }
        foreach (MethodInfo method in type.GetMethods())
        {
            foreach (Attribute attribute in method.GetCustomAttributes())
            {
                if (attribute is MethodHandler handler)
                {
                    handler.Handle(method, instance, mod);
                }
            }
        }

        return instance;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public abstract class MethodHandler : Attribute
{
    public abstract void Handle(MethodInfo method, object instance, IMod mod = null);
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public abstract class PropertyHandler : Attribute
{
    public abstract void Handle(PropertyInfo property, object instance, IMod mod = null);
}
