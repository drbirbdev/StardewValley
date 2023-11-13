using System;
using System.Reflection;
using BirbCore.APIs;
using BirbCore.Extensions;
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

    public override object Handle(Type type, IMod mod = null)
    {
        mod.Helper.Events.GameLoop.GameLaunched += (sender, e) =>
        {
            Api = mod.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (Api == null)
            {
                Log.Error("Generic Mod Config Menu is not enabled, so will skip parsing");
                return;
            }

            MemberInfo configField = mod.GetType().GetMemberOfType(type);
            var getter = configField.GetGetter();
            var setter = configField.GetSetter();

            if (configField == null)
            {
                Log.Error("Mod must define a Config property");
                return;
            }

            Api.Register(
                mod: mod.ModManifest,
                reset: () => setter(mod, Activator.CreateInstance(type)),
                save: () => mod.Helper.WriteConfig(getter(mod)),
                titleScreenOnly: this.TitleScreenOnly
            );

            object instance = base.Handle(type, mod);
            setter(mod, instance);
        };

        return null;
    }


    /// <summary>
    /// Specifies a property as a config.
    /// </summary>
    public class Option : FieldHandler
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



        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod = null, object[] args = null)
        {
            if (fieldType == typeof(bool))
            {
                SConfig.Api.AddBoolOption(
                    mod: mod.ModManifest,
                    getValue: () => (bool)getter(instance),
                    setValue: value => setter(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{name}") ?? name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{name}.tooltip"),
                    fieldId: this.FieldId
                );
            }
            else if (fieldType == typeof(int))
            {
                SConfig.Api.AddNumberOption(
                    mod: mod.ModManifest,
                    getValue: () => (int)getter(instance),
                    setValue: value => setter(instance, (int)value),
                    name: () => mod.Helper.Translation.Get($"config.{name}") ?? name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{name}.tooltip"),
                    fieldId: this.FieldId,
                    min: this.Min == float.MaxValue ? null : this.Min,
                    max: this.Max == float.MinValue ? null : this.Max,
                    interval: this.Interval == float.MinValue ? null : this.Interval,
                    formatValue: null
                );
            }
            else if (fieldType == typeof(float))
            {
                SConfig.Api.AddNumberOption(
                    mod: mod.ModManifest,
                    getValue: () => (float)getter(instance),
                    setValue: value => setter(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{name}") ?? name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{name}.tooltip"),
                    fieldId: FieldId,
                    min: this.Min == float.MaxValue ? null : this.Min,
                    max: this.Max == float.MinValue ? null : Max,
                    interval: this.Interval == float.MinValue ? null : this.Interval,
                    formatValue: null
                );
            }
            else if (fieldType == typeof(string))
            {
                SConfig.Api.AddTextOption(
                    mod: mod.ModManifest,
                    getValue: () => (string)getter(instance),
                    setValue: value => setter(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{name}") ?? name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{name}.tooltip"),
                    fieldId: FieldId,
                    allowedValues: AllowedValues,
                    formatAllowedValue: null
                );
            }
            else if (fieldType == typeof(SButton))
            {
                SConfig.Api.AddKeybind(
                    mod: mod.ModManifest,
                    getValue: () => (SButton)getter(instance),
                    setValue: value => setter(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{name}") ?? name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{name}.tooltip"),
                    fieldId: FieldId
                );
            }
            else if (fieldType == typeof(KeybindList))
            {
                SConfig.Api.AddKeybindList(
                    mod: mod.ModManifest,
                    getValue: () => (KeybindList)getter(instance),
                    setValue: value => setter(instance, value),
                    name: () => mod.Helper.Translation.Get($"config.{name}") ?? name,
                    tooltip: () => mod.Helper.Translation.Get($"config.{name}.tooltip"),
                    fieldId: FieldId
                );
            }
            else
            {
                throw new Exception($"Config had invalid property type {name}");
            }
        }
    }

    /// <summary>
    /// Adds a section title to the config menu.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SectionTitle : FieldHandler
    {
        public string Key;

        public SectionTitle(string key)
        {
            this.Key = key;
        }

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod = null, object[] args = null)
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
    public class Paragraph : FieldHandler
    {
        public string Key;

        public Paragraph(string key)
        {
            this.Key = key;
        }

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod = null, object[] args = null)
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
    public class PageBlock : FieldHandler
    {
        public string PageId;

        public PageBlock(string pageId)
        {
            this.PageId = pageId;
        }

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod = null, object[] args = null)
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
    public class PageLink : FieldHandler
    {
        public string PageId;

        public PageLink(string pageId)
        {
            this.PageId = pageId;
        }

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod = null, object[] args = null)
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
    public class Image : FieldHandler
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

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod = null, object[] args = null)
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
    public class StartTitleOnlyBlock : FieldHandler
    {
        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod = null, object[] args = null)
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
    public class EndTitleOnlyBlock : FieldHandler
    {
        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod = null, object[] args = null)
        {
            SConfig.Api.SetTitleScreenOnlyForNextOptions(
                mod: mod.ModManifest,
                titleScreenOnly: false
            );
        }
    }

}
