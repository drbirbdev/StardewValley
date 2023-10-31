using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Tools;
using System;
using System.Xml.Serialization;
using BirbShared;

namespace RanchingToolUpgrades
{
    [XmlType("Mods_drbirbdev_upgradeableshears")]
    public class UpgradeableShears : Shears
    {
        public const int MaxUpgradeLevel = 4;

        public UpgradeableShears() : base()
        {
            base.UpgradeLevel = 0;
            base.InitialParentTileIndex = -1;
            base.IndexOfMenuItemView = -1;
        }

        public UpgradeableShears(int upgradeLevel) : base()
        {
            base.UpgradeLevel = upgradeLevel;
            base.InitialParentTileIndex = -1;
            base.IndexOfMenuItemView = -1;
        }

        public static bool CanBeUpgraded()
        {
            Tool shears = Game1.player.getToolFromName("Shears");
            return shears is not null && shears.UpgradeLevel != MaxUpgradeLevel;
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            spriteBatch.Draw(
                texture: ModEntry.Assets.Sprites,
                position: location + new Vector2(32f, 32f),
                sourceRectangle: IconSourceRectangle(this.UpgradeLevel),
                color: color * transparency,
                rotation: 0f,
                origin: new Vector2(8, 8),
                scale: Game1.pixelZoom * scaleSize,
                effects: SpriteEffects.None,
                layerDepth: layerDepth);
        }

        public static Rectangle IconSourceRectangle(int upgradeLevel)
        {
            Rectangle source = new(16, 0, 16, 16);
            source.Y += upgradeLevel * source.Height;
            return source;
        }

        public override bool canBeTrashed()
        {
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SMAPI.CommonErrors", "AvoidImplicitNetFieldCast:Netcode types shouldn't be implicitly converted", Justification = "<Pending>")]
        public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
        {
            FarmAnimal animal = ModEntry.Instance.Helper.Reflection.GetField<FarmAnimal>((Shears)this, "animal").GetValue();

            if (animal != null && animal.currentProduce.Value != null && animal.isAdult() && animal.CanGetProduceWithTool(this))
            {
                // do extra friendship effect
                int extraFriendship = ModEntry.Config.ExtraFriendshipBase * this.UpgradeLevel;
                animal.friendshipTowardFarmer.Value = Math.Min(1000, animal.friendshipTowardFarmer + extraFriendship);
                Log.Trace($"Applied extra friendship {extraFriendship}.  Total friendship: {animal.friendshipTowardFarmer.Value}");

                // do quality bump effect
                float higherQualityChance = ModEntry.Config.QualityBumpChanceBase * this.UpgradeLevel;
                if (higherQualityChance > Game1.random.NextDouble())
                {
                    switch (animal.produceQuality) {
                        case 0: animal.produceQuality.Set(1);
                            break;
                        case 1: animal.produceQuality.Set(2);
                            break;
                        case 2: animal.produceQuality.Set(4);
                            break;
                        default: break;
                    }
                    Log.Debug($"Quality Bump Chance {higherQualityChance}, succeeded.  New quality {animal.produceQuality.Value}");
                }
                else
                {
                    Log.Debug($"Quality Bump Chance {higherQualityChance} failed.");
                }

                // do extra produce effect
                int extraProduce = 0;
                for (int i = 0; i < this.UpgradeLevel; i++)
                {
                    if (ModEntry.Config.ExtraProduceChance > Game1.random.NextDouble())
                    {
                        extraProduce++;
                    }
                }
                Log.Debug($"Extra Produce Chance {ModEntry.Config.ExtraProduceChance} generated {extraProduce} additional produce from {this.UpgradeLevel} draws.");
                if (extraProduce > 0)
                {
                    who.addItemToInventory(new StardewValley.Object(animal.currentProduce.Value, extraProduce, quality: animal.produceQuality.Value));
                }
            }

            base.DoFunction(location, x, y, power, who);
        }

    }
}
