using BirbShared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RanchingToolUpgrades
{
    [XmlType("Mods_drbirbdev_upgradeablepail")] // SpaceCore serialisation signature
    public class UpgradeablePail : MilkPail
    {
        public const int MaxUpgradeLevel = 4;

        public UpgradeablePail() : base()
        {
            base.UpgradeLevel = 0;
        }

        public UpgradeablePail(int upgradeLevel) : base()
        {
            base.UpgradeLevel = upgradeLevel;
            base.InitialParentTileIndex = -1;
            base.IndexOfMenuItemView = -1;
        }

        public static bool CanBeUpgraded()
        {
            Tool pail = Game1.player.getToolFromName("Pail");
            return pail is not null && pail.UpgradeLevel != MaxUpgradeLevel;
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
            Rectangle source = new(0, 0, 16, 16);
            source.Y += upgradeLevel * source.Height;
            return source;
        }

        public override bool canBeTrashed()
        {
            return false;
        }

        public static void AddToShopStock(Dictionary<ISalable, int[]> itemPriceAndStock, Farmer who)
        {
            if (who == Game1.player && CanBeUpgraded())
            {
                int quantity = 1;
                int upgradeLevel = who.getToolFromName("Pail").UpgradeLevel + 1;
                if (who.getToolFromName("Pail") is not UpgradeablePail)
                {
                    upgradeLevel = 1;
                }
                int upgradePrice = ModEntry.Instance.Helper.Reflection.GetMethod(
                    typeof(Utility), "priceForToolUpgradeLevel")
                    .Invoke<int>(upgradeLevel);
                upgradePrice = (int)(upgradePrice * ModEntry.Config.PailUpgradeCostMultiplier);
                int extraMaterialIndex = ModEntry.Instance.Helper.Reflection.GetMethod(
                    typeof(Utility), "indexOfExtraMaterialForToolUpgrade")
                    .Invoke<int>(upgradeLevel);
                itemPriceAndStock.Add(
                    new UpgradeablePail(upgradeLevel: upgradeLevel),
                    new int[] { upgradePrice, quantity, extraMaterialIndex, ModEntry.Config.PailUpgradeCostBars });
            }
        }

        public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
        {
            FarmAnimal animal = ModEntry.Instance.Helper.Reflection.GetField<FarmAnimal>((MilkPail)this, "animal").GetValue();

            if (animal != null && animal.currentProduce.Value != null && animal.isAdult() && animal.CanGetProduceWithTool(this))
            {
                // do extra friendship effect
                int extraFriendship = ModEntry.Config.ExtraFriendshipBase * this.UpgradeLevel;
                animal.friendshipTowardFarmer.Value = Math.Min(1000, animal.friendshipTowardFarmer + extraFriendship);
                Log.Debug($"Applied extra friendship {extraFriendship}.  Total friendship: {animal.friendshipTowardFarmer.Value}");

                // do quality bump effect
                float higherQualityChance = ModEntry.Config.QualityBumpChanceBase * this.UpgradeLevel;
                if (higherQualityChance > Game1.random.NextDouble())
                {
                    switch (animal.produceQuality)
                    {
                        case 0:
                            animal.produceQuality.Set(1);
                            break;
                        case 1:
                            animal.produceQuality.Set(2);
                            break;
                        case 2:
                            animal.produceQuality.Set(4);
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
