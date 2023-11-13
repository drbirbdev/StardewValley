using System;
using System.Reflection;
using BirbCore.Extensions;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace BirbCore.Annotations;

/// <summary>
/// A collection of assets that go through the content pipeline.
/// </summary>
public class SAsset : ClassHandler
{
    protected MemberInfo ModAssets;

    public override object Handle(Type type, IMod mod = null)
    {
        this.ModAssets = mod.GetType().GetMemberOfType(type);
        if (this.ModAssets == null)
        {
            Log.Error("Mod must define an asset property");
            return null;
        }

        object instance = base.Handle(type, mod);
        this.ModAssets.GetSetter()(mod, instance);
        return instance;
    }

    /// <summary>
    /// A single asset. This property is synced with what is in the content pipeline, and can be used directly.
    /// This asset can be overriden by other mods, and those changes will be reflected in this property.
    /// The path of the asset will be "Mods/<ModUniqueID>/<Property>", for instance the following property
    /// <code>
    ///    [Asset(Path="assets/my_texture.png")]
    ///    public static Texture2D MyTexture;
    ///    public static string MyTextureAssetName;
    /// </code>
    /// could be located at "Mods/drbirbdev.BirbCore/MyTexture" in the content pipeline. Other mods could then
    /// load this texture to use it, and this mod can just use the MyTexture property directly.
    /// An optional string property sharing the same name, but ending with "AssetName" can also be included.
    /// This property will be set to the "Mods/<ModUniqueID>/<Property>" value, which is required for some methods.
    /// </summary>
    public class Asset : FieldHandler
    {
        public string Path;
        public AssetLoadPriority Priority;

        public Asset(string path, AssetLoadPriority priority = AssetLoadPriority.Medium)
        {
            this.Path = path;
            this.Priority = priority;
        }

        public override void Handle(string name, Type fieldType, Func<object?, object?> getter, Action<object?, object?> setter, object instance, IMod mod, object[] args = null)
        {
            string assetId = PathUtilities.NormalizeAssetName($"Mods/{mod.ModManifest.UniqueID}/{name}");


            Action<object, object> assetNameSetter = instance.GetType().GetMemberOfName(name + "AssetName")?.GetSetter();
            if (assetNameSetter is not null)
            {
                assetNameSetter(instance, assetId);
            }

            mod.Helper.Events.Content.AssetRequested += (object sender, AssetRequestedEventArgs e) =>
            {
                if (!e.Name.IsEquivalentTo(assetId))
                {
                    return;
                }

                object value = e.GetType().GetMethod("LoadFromModFile")
                    .MakeGenericMethod(fieldType)
                    .Invoke(e, new object[] { PathUtilities.NormalizePath(this.Path), this.Priority });
                setter(instance, value);
            };

            mod.Helper.Events.Content.AssetReady += (object sender, AssetReadyEventArgs e) =>
            {
                if (!e.Name.IsEquivalentTo(assetId))
                {
                    return;
                }

                setter(instance, LoadValue(fieldType, assetId));
            };

            mod.Helper.Events.Content.AssetsInvalidated += (object sender, AssetsInvalidatedEventArgs e) =>
            {
                foreach (IAssetName asset in e.Names)
                {
                    if (asset.IsEquivalentTo(assetId))
                    {
                        setter(instance, LoadValue(fieldType, assetId));
                    }
                }
            };

            mod.Helper.Events.GameLoop.GameLaunched += (object sender, GameLaunchedEventArgs e) =>
            {
                setter(instance, LoadValue(fieldType, assetId));
            };
        }

        private static object LoadValue(Type fieldType, string modId)
        {
            return Game1.content.GetType().GetMethod("Load", new[] { typeof(string) })
                .MakeGenericMethod(fieldType)
                .Invoke(Game1.content, new string[] { modId });
        }

    }
}
