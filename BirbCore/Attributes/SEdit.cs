#nullable enable
using System;
using System.Collections;
using System.Reflection;
using BirbCore.Extensions;
using Microsoft.Xna.Framework;
using Sickhead.Engine.Util;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace BirbCore.Attributes;

/// <summary>
/// A collection of Edits made to the content pipeline.  Similar functionality to Content Patcher, but in code, and with far fewer features.
/// </summary>
internal class SEdit : ClassHandler
{

    public enum Update
    {
        Never,
        OnDayStart,
        OnLocationChange,
        OnTimeChange,
        OnTick,
    }

    public SEdit() : base(0)
    {

    }

    public override void Handle(Type type, object? instance, IMod mod, object[]? args = null)
    {
        instance = Activator.CreateInstance(type);
        base.Handle(type, instance, mod, args);
        return;
    }

    internal abstract class BaseEdit : FieldHandler
    {
        public string Target;
        public string? Condition;
        public Update Frequency;
        public AssetEditPriority Priority;
        protected IMod? Mod;
        private bool IsApplied = false;

        protected BaseEdit(string target, string? condition = null, Update update = Update.Never, AssetEditPriority priority = AssetEditPriority.Default)
        {
            this.Target = target;
            this.Condition = condition;
            this.Frequency = update;
            this.Priority = priority;
        }

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object? instance, IMod mod, object[]? args = null)
        {
            if (GameStateQuery.IsImmutablyFalse(this.Condition))
            {
                Log.Error($"Condition {this.Condition} will never be true, so edit {name} will never be applied.");
                return;
            }

            this.Mod = mod;

            switch (this.Frequency)
            {
                case Update.OnDayStart: this.Mod.Helper.Events.GameLoop.DayStarted += this.InvalidateIfNeeded; break;
                case Update.OnLocationChange: this.Mod.Helper.Events.Player.Warped += this.InvalidateIfNeeded; break;
                case Update.OnTimeChange: this.Mod.Helper.Events.GameLoop.TimeChanged += this.InvalidateIfNeeded; break;
                case Update.OnTick: this.Mod.Helper.Events.GameLoop.UpdateTicked += this.InvalidateIfNeeded; break;
                default: break;
            }

            this.Mod.Helper.Events.Content.AssetRequested += (object? sender, AssetRequestedEventArgs e) =>
            {
                if (!e.Name.IsEquivalentTo(this.Target))
                {
                    return;
                }
                if (!GameStateQuery.CheckConditions(this.Condition))
                {
                    return;
                }

                e.Edit(asset =>
                {
                    this.DoEdit(asset, getter(instance), name, fieldType, instance);
                }, this.Priority, this.Mod.ModManifest.UniqueID);
            };
        }

        public abstract void DoEdit(IAssetData asset, object? edit, string name, Type fieldType, object? instance);

        public void InvalidateIfNeeded(object? sender, object e)
        {
            if (this.Mod is not null && this.IsApplied != GameStateQuery.CheckConditions(this.Condition))
            {
                this.IsApplied = !this.IsApplied;
                this.Mod.Helper.GameContent.InvalidateCache(this.Target);
            }
        }
    }

    /// <summary>
    /// Change some data content.
    /// Target - the asset name to edit.
    /// Field - path to a field within the asset, similar to TargetField in Content Patcher.  Optional, default empty.
    /// Condition - a Game State Query for when to apply this change.  Optional, default always.
    /// UpdateFrequency - the frequency to recheck the condition to see if this asset should be invalidated.
    /// Priority - the priority with which to apply this change.  Optional, default 0 (normal priority).
    /// </summary>
    public class Data : BaseEdit
    {
        public string[]? Field;

        public Data(string target, string[]? field = null, string? condition = null, Update update = Update.Never, AssetEditPriority priority = AssetEditPriority.Default) : base(target, condition, update, priority)
        {
            this.Field = field;
        }

        public override void DoEdit(IAssetData asset, object? edit, string name, Type fieldType, object? instance)
        {
            if (this.Mod is null)
            {
                return;
            }
            object? toEdit = asset;
            MemberInfo? toEditMemberInfo = asset.GetType().GetProperty("Data");

            if (this.Field != null)
            {
                foreach (string field in this.Field)
                {
                    if (toEditMemberInfo is not null)
                    {
                        toEdit = toEditMemberInfo.GetValue(toEdit);
                        toEditMemberInfo = null;
                    }

                    if (toEdit is IList toEditList)
                    {
                        if (!int.TryParse(field, out int index) || toEditList.Count >= index)
                        {
                            Log.Error($"SEdit could not parse field path [{string.Join(",", this.Field)}] at entry {field}. Expected index in list.");
                            return;
                        }
                        toEdit = toEditList[index];
                    }
                    else if (toEdit is IDictionary toEditDict)
                    {
                        if (!toEditDict.Contains(field))
                        {
                            Log.Error($"SEdit could not parse field path [{string.Join(",", this.Field)}] at entry {field}. Expected key for dictionary.");
                            return;
                        }
                        toEdit = toEditDict[field];
                    }
                    else
                    {
                        toEditMemberInfo = toEdit?.GetType().GetField(field);
                        toEditMemberInfo ??= toEdit?.GetType().GetProperty(field);

                        if (toEditMemberInfo is null)
                        {
                            Log.Error($"SEdit could not parse field path [{string.Join(",", this.Field)}] at entry {field}. Expected field or property name.");
                            return;
                        }
                    }
                }
            }

            if (toEdit is IList editedList)
            {
                if (edit is null)
                {
                    editedList.Clear();
                }
                else if (edit is IList toAddList)
                {
                    foreach (object item in toAddList)
                    {
                        editedList.Add(item);
                    }
                }
                else
                {
                    editedList.Add(edit);
                }
            }
            else if (toEdit is IDictionary editedDictionary)
            {
                if (edit is null)
                {
                    editedDictionary.Clear();
                }
                else if (edit is IDictionary toAddDictionary)
                {
                    foreach (DictionaryEntry entry in toAddDictionary)
                    {
                        editedDictionary.Add(entry.Key, entry.Value);
                    }
                }
                else
                {
                    editedDictionary.Add(this.Mod.ModManifest.UniqueID + "_" + name, edit);
                }
            }
            else
            {
                toEditMemberInfo.SetValue(toEdit, edit);
            }
        }
    }

    /// <summary>
    /// Expects a relative path to a source image file as the string field value.
    /// </summary>
    public class Image : BaseEdit
    {
        public PatchMode PatchMode;

        public Image(string target, PatchMode patchMode, string? condition = null, Update update = Update.Never, AssetEditPriority priority = AssetEditPriority.Default) : base(target, condition, update, priority)
        {
            this.PatchMode = patchMode;
        }

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object? instance, IMod mod, object[]? args = null)
        {
            if (fieldType != typeof(string))
            {
                Log.Error($"SEdit.Image only works with string fields or properties, but was {fieldType}");
                return;
            }

            base.Handle(name, fieldType, getter, setter, instance, mod, args);
        }

        public override void DoEdit(IAssetData asset, object? edit, string name, Type fieldType, object? instance)
        {
            if (edit is null || this.Mod is null)
            {
                return;
            }
            string filePath = (string)edit;
            IAssetDataForImage image = asset.AsImage();

            IRawTextureData source = this.Mod.Helper.ModContent.Load<IRawTextureData>(filePath);
            if (source is null)
            {
                return;
            }
            Rectangle? sourceRect = null;
            Rectangle? targetRect = null;

            Func<object?, object?>? rectGetter = fieldType.GetMemberOfName(name + "SourceArea")?.GetGetter();
            if (rectGetter is not null)
            {
                sourceRect = (Rectangle?)rectGetter(instance);
            }
            rectGetter = instance?.GetType().GetMemberOfName(name + "TargetArea")?.GetGetter();
            if (rectGetter is not null)
            {
                targetRect = (Rectangle?)rectGetter(instance);
            }

            image.PatchImage(source, sourceRect, targetRect, this.PatchMode);
        }
    }

    public class Map : BaseEdit
    {
        public PatchMapMode PatchMode;

        public Map(string target, PatchMapMode patchMode = PatchMapMode.Overlay, string? condition = null, Update update = Update.Never, AssetEditPriority priority = AssetEditPriority.Default) : base(target, condition, update, priority)
        {
            this.PatchMode = patchMode;
        }

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object? instance, IMod mod, object[]? args = null)
        {
            if (fieldType != typeof(string))
            {
                Log.Error($"SEdit.Map only works with string fields or properties, but was {fieldType}");
                return;
            }

            base.Handle(name, fieldType, getter, setter, instance, mod, args);
        }

        public override void DoEdit(IAssetData asset, object? edit, string name, Type fieldType, object? instance)
        {
            if (edit is null || this.Mod is null)
            {
                return;
            }
            string filePath = (string)edit;
            IAssetDataForMap map = asset.AsMap();

            xTile.Map source = this.Mod.Helper.ModContent.Load<xTile.Map>(filePath);

            Rectangle? sourceRect = null;
            Rectangle? targetRect = null;
            Func<object?, object?>? rectGetter = fieldType.GetMemberOfName(name + "SourceArea")?.GetGetter();
            if (rectGetter is not null)
            {
                sourceRect = (Rectangle?)rectGetter(instance);
            }
            rectGetter = instance?.GetType().GetMemberOfName(name + "TargetArea")?.GetGetter();
            if (rectGetter is not null)
            {
                targetRect = (Rectangle?)rectGetter(instance);
            }

            map.PatchMap(source, sourceRect, targetRect, this.PatchMode);
        }
    }
}
