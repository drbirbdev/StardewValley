using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using BirbShared;

namespace PanningUpgrades
{
    internal interface HarmonyPatches
    {
        public static void Patch(string id)
        {
            Harmony harmony = new(id);
            try
            {
                // Patch relevent shop inventories, and upgrade actions
                harmony.Patch(
                    original: AccessTools.Method(typeof(Utility), nameof(Utility.getBlacksmithUpgradeStock)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Utility_GetBlacksmithUpgradeStock_Postfix)));
                harmony.Patch(
                    original: AccessTools.Method(typeof(Farmer), nameof(Farmer.showHoldingItem)),
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Farmer_ShowHoldingItem_Prefix)));
                harmony.Patch(
                    original: AccessTools.Method(typeof(Utility), nameof(Utility.getFishShopStock)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Utility_GetFishShopStock_Postfix)));

                // Patch logic around wearing a pan as a hat.
                harmony.Patch(
                    original: AccessTools.Method(typeof(Utility), nameof(Utility.PerformSpecialItemPlaceReplacement)),
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Utility_PerformSpecialItemPlaceReplacement_Prefix)));
                harmony.Patch(
                    original: AccessTools.Method(typeof(Utility), nameof(Utility.PerformSpecialItemGrabReplacement)),
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Utility_PerformSpecialItemGrabReplacement_Prefix)));
                harmony.Patch(
                    original: AccessTools.Method(typeof(InventoryPage), nameof(InventoryPage.receiveLeftClick)),
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(InventoryPage_ReceiveLeftClick_Prefix)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(InventoryPage_ReceiveLeftClick_Postfix)));

                // Patch animation when using pan.
                harmony.Patch(
                    original: AccessTools.Method(typeof(FarmerSprite), nameof(FarmerSprite.getAnimationFromIndex)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(FarmerSprite_GetAnimationFromIndex_Postfix)));

                // Patch event logic when receiving a pan.
                harmony.Patch(
                    original: AccessTools.Method(typeof(Event), nameof(Event.command_awardFestivalPrize)),
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Event_Command_AwardFestivalPrize_Prefix)));
                harmony.Patch(
                    original: AccessTools.Method(typeof(Event), nameof(Event.command_itemAboveHead)),
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Event_Command_ItemAboveHead_Prefix)));
                harmony.Patch(
                    original: AccessTools.Method(typeof(Event), nameof(Event.skipEvent)),
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Event_SkipEvent_Prefix)));

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Tries to add cooking tool to Blacksmith shop stock.
        /// </summary>
        public static void Utility_GetBlacksmithUpgradeStock_Postfix(
            Dictionary<ISalable, int[]> __result,
            Farmer who)
        {
            try
            {
                UpgradeablePan.AddToShopStock(itemPriceAndStock: __result, who: who);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Utility_GetBlacksmithUpgradeStock_Postfix)}\n{ex}");
            }
        }

        /// <summary>
        /// Draws the correct tool sprite when receiving an upgrade.
        /// </summary>
        public static bool Farmer_ShowHoldingItem_Prefix(
            Farmer who)
        {
            try
            {
                if (who.mostRecentlyGrabbedItem is UpgradeablePan)
                {
                    Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(
                        textureName: ModEntry.Assets.SpritesPath,
                        sourceRect: UpgradeablePan.IconSourceRectangle((who.mostRecentlyGrabbedItem as Tool).UpgradeLevel),
                        animationInterval: 2500f,
                        animationLength: 1,
                        numberOfLoops: 0,
                        position: who.Position + new Vector2(0f, -124f),
                        flicker: false,
                        flipped: false,
                        layerDepth: 1f,
                        alphaFade: 0f,
                        color: Color.White,
                        scale: 4f,
                        scaleChange: 0f,
                        rotation: 0f,
                        rotationChange: 0f)
                    {
                        motion = new Vector2(0f, -0.1f)
                    });
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Farmer_ShowHoldingItem_Prefix)}\n{ex}");
            }
            return true;
        }

        /// <summary>
        /// Removes the old Copper Pan tool from the fishing shop.
        /// </summary>
        public static void Utility_GetFishShopStock_Postfix(
            Dictionary<ISalable, int[]> __result,
            Farmer who)
        {
            try
            {
                // Keying off of `new Pan()` doesn't work.
                // Iterate over items for sale, and remove any by the name "Copper Pan".
                foreach (ISalable key in __result.Keys)
                {
                    if (key.Name.Equals("Copper Pan"))
                    {
                        __result.Remove(key);
                    }
                }
                if (ModEntry.Config.BuyablePan)
                {
                    __result.Add(new UpgradeablePan(0), new int[2] { ModEntry.Config.BuyCost, 2147483647 });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Utility_GetFishShopStock_Postfix)}\n{ex}");
            }
        }

        /// <summary>
        /// Handles using pan as a hat in certain menus.
        /// </summary>
        public static bool Utility_PerformSpecialItemPlaceReplacement_Prefix(
            ref Item __result,
            Item placedItem)
        {
            try
            {
                if (placedItem != null && placedItem is UpgradeablePan upgradeablePan)
                {
                    __result = PanToHat(upgradeablePan);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Utility_PerformSpecialItemPlaceReplacement_Prefix)}\n{ex}");
            }
            return true;
        }

        public static Hat PanToHat(UpgradeablePan pan)
        {
            return pan.UpgradeLevel switch
            {
                0 => new Hat(ModEntry.JsonAssets.GetHatId("Pan")),
                1 => new Hat(71),
                2 => new Hat(ModEntry.JsonAssets.GetHatId("Steel Pan")),
                3 => new Hat(ModEntry.JsonAssets.GetHatId("Gold Pan")),
                4 => new Hat(ModEntry.JsonAssets.GetHatId("Iridium Pan")),
                _ => new Hat(ModEntry.JsonAssets.GetHatId("Pan")),
            };
        }

        /// <summary>
        /// Handles using pan as a hat in certain menus.
        /// </summary>
        public static bool Utility_PerformSpecialItemGrabReplacement_Prefix(
            ref Item __result,
            Item heldItem)
        {
            try
            {
                if (heldItem != null && heldItem is Hat)
                {
                    int hatId = (int)(heldItem as Hat).which;
                    if (hatId == ModEntry.JsonAssets.GetHatId("Pan"))
                    {
                        __result = new UpgradeablePan(0);
                    }
                    else if (hatId == 71) // Using original copper pan hat.
                    {
                        __result = new UpgradeablePan(1);
                    }
                    else if (hatId == ModEntry.JsonAssets.GetHatId("Steel Pan"))
                    {
                        __result = new UpgradeablePan(2);
                    }
                    else if (hatId == ModEntry.JsonAssets.GetHatId("Gold Pan"))
                    {
                        __result = new UpgradeablePan(3);
                    }
                    else if (hatId == ModEntry.JsonAssets.GetHatId("Iridium Pan"))
                    {
                        __result = new UpgradeablePan(4);
                    }
                    else
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Utility_PerformSpecialItemGrabReplacement_Prefix)}\n{ex}");
            }
            return true;
        }

        /// <summary>
        /// Handles pan to hat conversion in Inventory page.  Since there's no good entry point for patching,
        /// detects changes to player.hat.Value and player.CursorSlotItem using __state.
        /// </summary>
        public static void InventoryPage_ReceiveLeftClick_Prefix(
            ref Item[] __state,
            int x,
            int y,
            bool playSound)
        {
            try
            {
                if (Game1.player.CursorSlotItem is UpgradeablePan)
                {
                    __state = new Item[] {
                        Game1.player.CursorSlotItem,
                        Game1.player.hat.Value,
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(InventoryPage_ReceiveLeftClick_Prefix)}\n{ex}");
            }
        }

        /// <summary>
        /// Handles pan to hat conversion in Inventory page.  Since there's no good entry point for patching,
        /// detects changes to player.hat.Value and player.CursorSlotItem using __state.
        /// </summary>
        public static void InventoryPage_ReceiveLeftClick_Postfix(
            Item[] __state,
            int x,
            int y,
            bool playSound)
        {
            try
            {
                if (__state is not null && __state[0] is UpgradeablePan upgradeablePan && __state[1] != Game1.player.hat.Value)
                {
                    Game1.player.hat.Value = PanToHat(upgradeablePan);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(InventoryPage_ReceiveLeftClick_Postfix)}\n{ex}");
            }
        }

        /// <summary>
        /// Use a TemporaryAnimatedSprite to make the panning animation reflect upgrade level.
        /// </summary>
        public static void FarmerSprite_GetAnimationFromIndex_Postfix(
            int index,
            FarmerSprite requester,
            int interval,
            int numberOfFrames,
            bool flip,
            bool secondaryArm)
        {
            try
            {
                if (index == 303)
                {
                    int upgradeLevel = Game1.player.CurrentTool.UpgradeLevel;
                    int genderOffset = Game1.player.IsMale ? -1 : 0;

                    Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(
                        textureName: ModEntry.Assets.SpritesPath,
                        sourceRect: UpgradeablePan.AnimationSourceRectangle(upgradeLevel),
                        animationInterval: ModEntry.Config.AnimationFrameDuration,
                        animationLength: 4,
                        numberOfLoops: 3,
                        position: Game1.player.Position + new Vector2(0f, (ModEntry.Config.AnimationYOffset + genderOffset) * 4),
                        flicker: false,
                        flipped: false,
                        layerDepth: 1f,
                        alphaFade: 0f,
                        color: Color.White,
                        scale: 4f,
                        scaleChange: 0f,
                        rotation: 0f,
                        rotationChange: 0f)
                    {
                        endFunction = extraInfo =>
                        {
                            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(
                                textureName: ModEntry.Assets.SpritesPath,
                                sourceRect: UpgradeablePan.AnimationSourceRectangle(upgradeLevel),
                                animationInterval: ModEntry.Config.AnimationFrameDuration,
                                animationLength: 3,
                                numberOfLoops: 0,
                                position: Game1.player.position + new Vector2(0f, (ModEntry.Config.AnimationYOffset + genderOffset) * 4),
                                flicker: false,
                                flipped: false,
                                layerDepth: 1f,
                                alphaFade: 0f,
                                color: Color.White,
                                scale: 4f,
                                scaleChange: 0f,
                                rotation: 0f,
                                rotationChange: 0f)
                            {
                                endFunction = extraInfo =>
                                {
                                    Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(
                                        textureName: ModEntry.Assets.SpritesPath,
                                        sourceRect: UpgradeablePan.AnimationSourceRectangle(upgradeLevel),
                                        animationInterval: ModEntry.Config.AnimationFrameDuration * 2.5f,
                                        animationLength: 1,
                                        numberOfLoops: 0,
                                        position: Game1.player.position + new Vector2(0f, (ModEntry.Config.AnimationYOffset + genderOffset) * 4),
                                        flicker: false,
                                        flipped: false,
                                        layerDepth: 1f,
                                        alphaFade: 0f,
                                        color: Color.White,
                                        scale: 4f,
                                        scaleChange: 0f,
                                        rotation: 0f,
                                        rotationChange: 0f));
                                }
                            });
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(FarmerSprite_GetAnimationFromIndex_Postfix)}\n{ex}");
            }
        }


        /// <summary>
        /// Changes which pan tool is rewarded during events.
        /// </summary>
        public static bool Event_Command_AwardFestivalPrize_Prefix(
            Event __instance,
            GameLocation location,
            GameTime time,
            string[] split)
        {
            try
            {
                if (split.Length > 1 && split[1].ToLower() == "pan")
                {
                    Game1.player.addItemByMenuIfNecessary(new UpgradeablePan());
                    if (Game1.activeClickableMenu == null)
                    {
                        __instance.CurrentCommand++;
                    }
                    __instance.CurrentCommand++;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Event_Command_AwardFestivalPrize_Prefix)}\n{ex}");
            }
            return true;
        }

        /// <summary>
        /// Changes which pan tool is shown being held during events.
        /// </summary>
        public static bool Event_Command_ItemAboveHead_Prefix(
            Event __instance,
            GameLocation location,
            GameTime time,
            string[] split)
        {
            try
            {
                if (split.Length > 1 && split[1].Equals("pan"))
                {
                    __instance.farmer.holdUpItemThenMessage(new UpgradeablePan());
                    __instance.CurrentCommand++;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Event_Command_ItemAboveHead_Prefix)}\n{ex}");
            }
            return true;
        }

        /// <summary>
        /// Rewards modded pan tool if event is skipped.
        /// </summary>
        public static bool Event_SkipEvent_Prefix(
            Event __instance,
            Dictionary<string, Vector3> ___actorPositionsAfterMove)
        {
            try
            {
                if (__instance.id == 404798)
                {
                    // Generic skip logic copied from skipEvent.
                    // If other mods patch skipEvent to change this logic, things might break.
                    if (__instance.playerControlSequence)
                    {
                        __instance.EndPlayerControlSequence();
                    }
                    Game1.playSound("drumkit6");
                    ___actorPositionsAfterMove.Clear();
                    foreach (NPC i in __instance.actors)
                    {
                        bool ignore_stop_animation = i.Sprite.ignoreStopAnimation;
                        i.Sprite.ignoreStopAnimation = true;
                        i.Halt();
                        i.Sprite.ignoreStopAnimation = ignore_stop_animation;
                        __instance.resetDialogueIfNecessary(i);
                    }
                    __instance.farmer.Halt();
                    __instance.farmer.ignoreCollisions = false;
                    Game1.exitActiveMenu();
                    Game1.dialogueUp = false;
                    Game1.dialogueTyping = false;
                    Game1.pauseTime = 0f;

                    // Event specific skip logic.
                    if (Game1.player.getToolFromName("Pan") is null)
                    {
                        Game1.player.addItemByMenuIfNecessary(new UpgradeablePan());
                    }
                    __instance.endBehaviors(new string[1] { "end" }, Game1.currentLocation);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Event_SkipEvent_Prefix)}\n{ex}");
            }
            return true;
        }
    }
}
