using System;
using System.Linq;
using System.Reflection;
using BirbCore.APIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace BirbCore.Annotations;


/// <summary>
/// Specifies a class as a config class.
/// </summary>
public class SConfig : ClassHandler
{
    public bool TitleScreenOnly = false;

    protected static IGenericModConfigMenuApi Api;
    protected PropertyInfo ModConfig;

    public override object Handle(Type type, IMod mod = null)
    {
        Api = mod.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (Api == null)
        {
            Log.Error("Generic Mod Config Menu is not enabled, so will skip parsing");
            return null;
        }

        this.ModConfig = mod.GetType().GetProperties().Where(p => p.PropertyType == type).First();
        if (this.ModConfig == null)
        {
            Log.Error("Mod must define a Config property");
            return null;
        }

        Api.Register(
            mod: mod.ModManifest,
            reset: () => ModConfig.SetValue(mod, Activator.CreateInstance(type)),
            save: () => mod.Helper.WriteConfig(ModConfig.GetValue(mod)),
            titleScreenOnly: this.TitleScreenOnly
        );

        object instance = base.Handle(type, mod);
        this.ModConfig.SetValue(mod, instance);
        return instance;
    }


    /// <summary>
    /// Specifies a property as a config.
    /// </summary>
    public class Option : PropertyHandler
    {
        public string FieldId;
        public float Min = float.MaxValue;
        public float Max = float.MinValue;
        public float Interval = float.MinValue;
        public string[] AllowedValues;

        public Option(string fieldId = null)
        {
            this.FieldId = fieldId;
        }

        public Option(int min, int max, int interval = 1, string fieldId = null)
        {
            this.FieldId = fieldId;
            this.Min = min;
            this.Max = max;
            this.Interval = interval;
        }

        public Option(float min, float max, float interval = 1.0f, string fieldId = null)
        {
            this.FieldId = fieldId;
            this.Min = min;
            this.Max = max;
            this.Interval = interval;
        }

        public Option(string[] allowedValues, string fieldId = null)
        {
            this.FieldId= fieldId;
            this.AllowedValues = allowedValues;
        }



        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            if (property.PropertyType == typeof(bool))
            {
                SConfig.Api.AddBoolOption(
                    mod: mod.ModManifest,
                    getValue: () => (bool)property.GetValue(instance),
                    setValue: value => property.SetValue(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{property.Name}") ?? property.Name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{property.Name}.tooltip"),
                    fieldId: FieldId
                );
            }
            else if (property.PropertyType == typeof(int))
            {
                SConfig.Api.AddNumberOption(
                    mod: mod.ModManifest,
                    getValue: () => (int)property.GetValue(instance),
                    setValue: value => property.SetValue(instance, (int)value),
                    name: () => mod.Helper.Translation.Get($"config.{property.Name}") ?? property.Name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{property.Name}.tooltip"),
                    fieldId: FieldId,
                    min: Min == float.MaxValue ? null : Min,
                    max: Max == float.MinValue ? null : Max,
                    interval: Interval == float.MinValue ? null : Interval,
                    formatValue: null
                );
            }
            else if (property.PropertyType == typeof(float))
            {
                SConfig.Api.AddNumberOption(
                    mod: mod.ModManifest,
                    getValue: () => (float)property.GetValue(instance),
                    setValue: value => property.SetValue(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{property.Name}") ?? property.Name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{property.Name}.tooltip"),
                    fieldId: FieldId,
                    min: Min == float.MaxValue ? null : Min,
                    max: Max == float.MinValue ? null : Max,
                    interval: Interval == float.MinValue ? null : Interval,
                    formatValue: null
                );
            }
            else if (property.PropertyType == typeof(string))
            {
                SConfig.Api.AddTextOption(
                    mod: mod.ModManifest,
                    getValue: () => (string)property.GetValue(instance),
                    setValue: value => property.SetValue(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{property.Name}") ?? property.Name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{property.Name}.tooltip"),
                    fieldId: FieldId,
                    allowedValues: AllowedValues,
                    formatAllowedValue: null
                );
            }
            else if (property.PropertyType == typeof(SButton))
            {
                SConfig.Api.AddKeybind(
                    mod: mod.ModManifest,
                    getValue: () => (SButton)property.GetValue(instance),
                    setValue: value => property.SetValue(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{property.Name}") ?? property.Name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{property.Name}.tooltip"),
                    fieldId: FieldId
                );
            }
            else if (property.PropertyType == typeof(KeybindList))
            {
                SConfig.Api.AddKeybindList(
                    mod: mod.ModManifest,
                    getValue: () => (KeybindList)property.GetValue(instance),
                    setValue: value => property.SetValue(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{property.Name}") ?? property.Name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{property.Name}.tooltip"),
                    fieldId: FieldId
                );
            }
            else
            {
                throw new Exception($"Config had invalid property type {property.Name}");
            }
        }
    }

    /// <summary>
    /// Adds a section title to the config menu.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SectionTitle : PropertyHandler
    {
        public string Key;

        public SectionTitle(string key)
        {
            this.Key = key;
        }

        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            SConfig.Api.AddSectionTitle(
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get($"config.{Key}") ?? Key,
                tooltip: () => mod.Helper.Translation.Get($"config.{Key}.tooltip")
            );
        }
    }

    /// <summary>
    /// Adds a paragraph to the config menu.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class Paragraph : PropertyHandler
    {
        public string Key;

        public Paragraph(string key)
        {
            this.Key = key;
        }

        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            SConfig.Api.AddParagraph(
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get($"config.{Key}") ?? Key
            );
        }
    }

    /// <summary>
    /// Starts a page block.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PageBlock : PropertyHandler
    {
        public string PageId;

        public PageBlock(string pageId)
        {
            this.PageId = pageId;
        }

        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            SConfig.Api.AddPage(
                mod: mod.ModManifest,
                pageId: PageId,
                pageTitle: () => mod.Helper.Translation.Get($"config.{PageId}")
            );
        }
    }

    /// <summary>
    /// Adds a link to a config page to the config menu.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PageLink : PropertyHandler
    {
        public string PageId;

        public PageLink(string pageId)
        {
            this.PageId = pageId;
        }

        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            SConfig.Api.AddPageLink(
                mod: mod.ModManifest,
                pageId: PageId,
                text: () => mod.Helper.Translation.Get($"config.{PageId}"),
                tooltip: () => mod.Helper.Translation.Get($"config.{PageId}.tooltip")
            );
        }
    }

    /// <summary>
    /// Adds an image to the config menu.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class Image : PropertyHandler
    {
        public string Texture;
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Image(string texture)
        {
            this.Texture = texture;
        }

        public Image(string texture, int x, int y, int width, int height)
        {
            this.Texture = texture;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            SConfig.Api.AddImage(
                mod: mod.ModManifest,
                texture: () => mod.Helper.GameContent.Load<Texture2D>(Texture),
                texturePixelArea: Width != 0 ? new Rectangle(X, Y, Width, Height) : null
            );
        }
    }

    /// <summary>
    /// Starts or ends a block of title-screen exclusive configs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class StartTitleOnlyBlock : PropertyHandler
    {
        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            SConfig.Api.SetTitleScreenOnlyForNextOptions(
                mod: mod.ModManifest,
                titleScreenOnly: true
            );
        }
    }

    /// <summary>
    /// Starts or ends a block of title-screen exclusive configs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class EndTitleOnlyBlock : PropertyHandler
    {
        public override void Handle(PropertyInfo property, object instance, IMod mod = null)
        {
            SConfig.Api.SetTitleScreenOnlyForNextOptions(
                mod: mod.ModManifest,
                titleScreenOnly: false
            );
        }
    }

}
