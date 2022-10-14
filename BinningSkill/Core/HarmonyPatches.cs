using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Locations;
using Netcode;
using SpaceCore;
using Microsoft.Xna.Framework;
using StardewValley.Tools;
using BirbShared;
using BinningSkill.Core;

namespace BinningSkill
{
    internal interface HarmonyPatches
    {
        public static void Patch(string id)
        {
            Harmony harmony = new(id);
            try
            {

                // Binning Skill
                //  - give binning exp
                //  - give binning skill bonus drops
                // Sneak Profession
                //  - negate negative reactions to being caught
                harmony.Patch(
                    original: AccessTools.Method(typeof(Town), nameof(Town.checkAction)),
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Town_CheckAction_Prefix)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Town_CheckAction_Postfix)));

                // Reclaimer Profession
                //  - give more profit from reclaiming
                harmony.Patch(
                    original: AccessTools.Method(typeof(Utility), nameof(Utility.getTrashReclamationPrice)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Utility_GetTrashReclamationPrice_Postfix)));

                // Upseller Profession
                //  - increase acceptance of hated and disliked gifts by one notch
                harmony.Patch(
                    original: AccessTools.Method(typeof(NPC), nameof(NPC.getGiftTasteForThisItem)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(NPC_GetGiftTasteForThisItem_Postfix)));

                // Binning Skill
                //  - give binning skill exp
                // Salvager Profession
                //  - provide rare results from recycling
                // Environmentalist Profession
                //  - gain friendship from recycling
                harmony.Patch(
                    original: AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.performObjectDropInAction)),
                    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Object_PerformObjectDropInAction_Prefix)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Object_PerformObjectDropInAction_Postfix)));

                // Recycler Profession
                //  - make recycing machines cheaper to craft
                harmony.Patch(
                    original: AccessTools.Constructor(typeof(StardewValley.CraftingRecipe), new Type[] {typeof(string), typeof(bool)}),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(CraftingRecipe_Constructor_Postfix)));
                    

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static void Town_CheckAction_Prefix(
            Town __instance,
            ref int[] __state,
            NetArray<bool, NetBool> ___garbageChecked,
            xTile.Dimensions.Location tileLocation,
            xTile.Dimensions.Rectangle viewport,
            Farmer who)
        {
            try
            {
                if (who.mount == null && __instance.map.GetLayer("Buildings").Tiles[tileLocation] != null &&
                    __instance.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex == 78)
                {
                    string s = __instance.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Action", "Buildings");
                    int whichCan = ((s != null) ? Convert.ToInt32(s.Split(' ')[1]) : (-1));
                    if (whichCan < 0 || whichCan >= ___garbageChecked.Length)
                    {
                        return;
                    }
                    if (!___garbageChecked[whichCan])
                    {
                        // Remember which can was interacted with, since the game code adjusts garbageChecked array
                        __state = new int[]
                        {
                            whichCan,
                            __instance.debris.Count,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Town_CheckAction_Prefix)}\n{ex}");
            }
        }

        public static void Town_CheckAction_Postfix(
            bool __result,
            Town __instance,
            ref int[] __state,
            NetArray<bool, NetBool> ___garbageChecked,
            xTile.Dimensions.Location tileLocation,
            xTile.Dimensions.Rectangle viewport,
            Farmer who)
        {
            try
            {
                if (__state != null && __state.Length > 0)
                {
                    if (who.HasCustomProfession(BinningSkill.Sneak))
                    {
                        Character c = Utility.isThereAFarmerOrCharacterWithinDistance(new Vector2(tileLocation.X, tileLocation.Y), 7, __instance);
                        if (c != null && c is NPC && c is not StardewValley.Characters.Horse)
                        {
                            if (!c.Name.Equals("Linus"))
                            {
                                // Sneak Profession
                                // Undo friendship drop and cancel dialogue/emote
                                c.isEmoting = false;
                                Game1.dialogueUp = false;
                                Game1.activeClickableMenu = null;
                                who.forceCanMove();
                                who.changeFriendship(ModEntry.Config.FriendshipRecovery, c as NPC);
                            }
                        }
                    }
                    if (__instance.debris is null && __instance.debris.Count > __state[1])
                    {
                        // something was already gotten from the trash, give standard binning experience and return
                        SpaceCore.Skills.AddExperience(who, "drbirbdev.Binning", ModEntry.Config.ExperienceFromTrashSuccess);
                        return;
                      
                    }

                    int whichCan = __state[0];
                    int binningLevel = SpaceCore.Skills.GetSkillLevel(who, "drbirbdev.Binning");
                    int rarity = -1;
                    // Determine the rarity level of the drop.
                    Random random = new();
                    if (random.Next(100) < ModEntry.Config.PerLevelBonusDropChance * binningLevel)
                    {
                        rarity = 0;
                        if (binningLevel >= 3 && random.Next(100) < ModEntry.Config.RareDropChance)
                        {
                            rarity = 1;
                            if (binningLevel >= 6 && random.Next(100) < ModEntry.Config.SuperRareDropChance)
                            {
                                rarity = 2;
                                if (binningLevel >= 9 && random.Next(100) < ModEntry.Config.UltraRareDropChance)
                                {
                                    rarity = 3;
                                }
                            }
                        }
                    }
                    if (rarity == -1)
                    {
                        // Give binning experience for not getting anything
                        SpaceCore.Skills.AddExperience(who, "drbirbdev.Binning", ModEntry.Config.ExperienceFromTrashFail);
                        return;
                    }
                    Item drop = new StardewValley.Object(168, 1);
                    if (rarity == 0)
                    {
                        drop = new StardewValley.Object(Utility.getRandomItemFromSeason(Game1.currentSeason, tileLocation.X * tileLocation.Y, false), 1);
                    }
                    else if (rarity == 1)
                    {
                        List<int> possibleItemIds = new();
                        switch (whichCan)
                        {
                            case 0:
                                // Jodi/Kent/Sam/Vincent
                                // Out-of-season crop (*)
                                if (!Game1.currentSeason.Equals("spring"))
                                {
                                    possibleItemIds.AddRange(new int[] { 597, 190, 433, 248, 188, 250, 24, 192, 252, 400, 591 });
                                }
                                if (!Game1.currentSeason.Equals("summer"))
                                {
                                    possibleItemIds.AddRange(new int[] { 258, 270, 304, 260, 254, 376, 264, 266, 268, 593, 421, 256 });
                                }
                                if (!Game1.currentSeason.Equals("fall"))
                                {
                                    possibleItemIds.AddRange(new int[] { 300, 274, 284, 278, 282, 272, 595, 398, 276, 280 });
                                }
                                break;
                            case 1:
                                // Haley/Emily
                                // Any gemstone
                                possibleItemIds.AddRange(new int[] { 60, 62, 64, 66, 68, 70, 72, 80, 82});
                                break;
                            case 2:
                                // Lewis
                                // Out-of-season forage
                                if (!Game1.currentSeason.Equals("spring"))
                                {
                                    possibleItemIds.AddRange(new int[] { 16, 18, 20, 22, 399, 296 });
                                }
                                if (!Game1.currentSeason.Equals("summer"))
                                {
                                    possibleItemIds.AddRange(new int[] { 398, 396, 402, 259 });
                                }
                                if (!Game1.currentSeason.Equals("fall"))
                                {
                                    possibleItemIds.AddRange(new int[] { 406, 408, 410, 281 });
                                }
                                if (!Game1.currentSeason.Equals("winter"))
                                {
                                    possibleItemIds.AddRange(new int[] { 412, 414, 416, 418, 283 });
                                }
                                break;
                            case 3:
                                // Museum (Gunther)
                                // Rare Geodes
                                possibleItemIds.AddRange(new int[] { 749, 275 });
                                break;
                            case 4:
                                // Blacksmith (Clint)
                                // Bars
                                possibleItemIds.AddRange(new int[] { 334, 335, 336, 337 });
                                break;
                            case 5:
                                // Saloon (Gus)
                                // Dish of the day
                                possibleItemIds.Add((Game1.dishOfTheDay.ParentSheetIndex != 217) ? (Game1.dishOfTheDay.ParentSheetIndex) : 216);
                                break;
                            case 6:
                                // George/Evelyn/Alex
                                // Fish
                                possibleItemIds.AddRange(new int[] { 128, 129, 130, 131, 132, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 154, 155 });
                                break;
                            case 7:
                                // Jojamart/Movie Theater
                                // Ticket or poster
                                if (Utility.doesMasterPlayerHaveMailReceivedButNotMailForTomorrow("ccMovieTheater"))
                                {
                                    possibleItemIds.AddRange(new int[] { 809, 1952, 1953, 1954, 1955, 1956, 1957, 1958, 1959 });
                                } else
                                {
                                    possibleItemIds.Add(167);
                                }
                                break;
                        }
                        int dropId = possibleItemIds.ElementAt(random.Next(possibleItemIds.Count));
                        if (dropId < 1000)
                        {
                            drop = new StardewValley.Object(dropId, 1);
                        }
                        else
                        {
                            drop = new StardewValley.Objects.Furniture(dropId, Vector2.Zero);
                        }
                    } 
                    else if (rarity == 2)
                    {
                        __instance.playSound("yoba");
                        List<int> possibleItemIds = new();
                        switch (whichCan)
                        {
                            case 0:
                                // Jodi/Kent/Sam/Vincent
                                // Enricher
                                // Pressure Nozzle
                                possibleItemIds.AddRange(new int[] { 913, 915 });
                                break;
                            case 1:
                                // Haley/Emily
                                // Prismatic Shard
                                possibleItemIds.Add(74);
                                break;
                            case 2:
                                // Lewis
                                // Mermaid Pendant
                                // Wilted Bouquet
                                possibleItemIds.AddRange(new int[] { 460, 277 });
                                break;
                            case 3:
                                // Museum (Gunther)
                                // Deconstructor
                                // Hopper
                                possibleItemIds.AddRange(new int[] { 265, 275 });
                                break;
                            case 4:
                                // Blacksmith (Clint)
                                // Radioactive Bar
                                possibleItemIds.Add(910);
                                break;
                            case 5:
                                // Saloon (Gus)
                                // Qi Seasoning
                                // Mushroom Tree Seed
                                possibleItemIds.AddRange(new int[] { 917, 891 });
                                break;
                            case 6:
                                // George/Evelyn/Alex
                                // Magic Bait
                                // Fairy Dust
                                possibleItemIds.AddRange(new int[] { 908, 872 });
                                break;
                            case 7:
                                // Jojamart/Movie Theater
                                // Golden Pumpkin
                                // Pearl
                                // Treasure Chest
                                possibleItemIds.AddRange(new int[] { 373, 797, 166 });
                                break;
                        }
                        int dropId = possibleItemIds.ElementAt(random.Next(possibleItemIds.Count));
                        if (whichCan == 3)
                        {
                            drop = new StardewValley.Object(Vector2.Zero, dropId);
                        }
                        else
                        {
                            drop = new StardewValley.Object(dropId, 1);
                        }
                    }
                    else if (rarity == 3)
                    {
                        __instance.playSound("reward");
                        List<Item> possibleDrops = new();
                        switch (whichCan)
                        {
                            case 0:
                                // Jodi/Kent/Sam/Vincent
                                // Sam's Old Guitar
                                // Seb's Lost Mace
                                // Abby's Planchette
                                // Sam's Boombox
                                possibleDrops.AddRange(new Item[]
                                {
                                    new MeleeWeapon(30),
                                    new MeleeWeapon(41),
                                    new MeleeWeapon(40),
                                    new StardewValley.Objects.Furniture(1309, Vector2.Zero),
                                });
                                break;
                            case 1:
                                // Haley/Emily
                                // Haley's Iron
                                // Sewing Machine
                                // Night Sky Decal #1
                                // Night Sky Decal #2
                                // Night Sky Decal #3
                                possibleDrops.AddRange(new Item[]
                                {
                                    new MeleeWeapon(42),
                                    new StardewValley.Object(Vector2.Zero, 247),
                                    new StardewValley.Objects.Furniture(1914, Vector2.Zero),
                                    new StardewValley.Objects.Furniture(1915, Vector2.Zero),
                                    new StardewValley.Objects.Furniture(1916, Vector2.Zero),
                                });
                                break;
                            case 2:
                                // Lewis
                                // Mini-Shipping Bin
                                // Solid Gold Lewis
                                // Stardew Hero Trophy
                                // Statue of Perfection
                                // Statue of True Perfection
                                possibleDrops.AddRange(new Item[]
                                {
                                    new StardewValley.Object(Vector2.Zero, 248),
                                    new StardewValley.Object(Vector2.Zero, 164),
                                    new StardewValley.Object(Vector2.Zero, 116),
                                    new StardewValley.Object(Vector2.Zero, 160),
                                    new StardewValley.Object(Vector2.Zero, 280),
                                });
                                break;
                            case 3:
                                // Museum (Gunther)
                                // Elliott's Pencil
                                // Penny's Fryer
                                // Pirate Flag
                                // Pirate Rug
                                possibleDrops.AddRange(new Item[]
                                {
                                    new MeleeWeapon(35),
                                    new MeleeWeapon(38),
                                    new StardewValley.Objects.Furniture(1900, Vector2.Zero),
                                    new StardewValley.Objects.Furniture(1902, Vector2.Zero),
                                });
                                break;
                            case 4:
                                // Blacksmith (Clint)
                                // Harvey's Mallet
                                // Maru's Wrench
                                // ??HMTGF??
                                // ??Foroguemon??
                                // ??Pinky Lemon??
                                possibleDrops.AddRange(new Item[]
                                {
                                    new MeleeWeapon(37),
                                    new MeleeWeapon(36),
                                    new StardewValley.Object(Vector2.Zero, 155),
                                    new StardewValley.Object(Vector2.Zero, 161),
                                    new StardewValley.Object(Vector2.Zero, 162),
                                });
                                break;
                            case 5:
                                // Saloon (Gus)
                                // Leah's Whittler
                                // Leah's Sculpture
                                // My First Painting
                                // Prairie King Arcade System
                                // Junimo Kart Arcade System
                                possibleDrops.AddRange(new Item[]
                                {
                                    new MeleeWeapon(39),
                                    new StardewValley.Objects.Furniture(1306, Vector2.Zero),
                                    new StardewValley.Objects.Furniture(1802, Vector2.Zero),
                                    new StardewValley.Object(Vector2.Zero, 141),
                                    new StardewValley.Object(Vector2.Zero, 159),
                                });
                                break;
                            case 6:
                                // George/Evelyn/Alex
                                // Alex's Bat
                                // Coffee Maker
                                // Strawberry Decal
                                // Fruit Salad Rug
                                possibleDrops.AddRange(new Item[]
                                {
                                    new MeleeWeapon(25),
                                    new StardewValley.Object(Vector2.Zero, 246),
                                    new StardewValley.Objects.Furniture(1907, Vector2.Zero),
                                    new StardewValley.Objects.Furniture(1909, Vector2.Zero),
                                });
                                break;
                            case 7:
                                // Jojamart/Movie Theater
                                // Auto-Petter
                                // Soda Machine
                                // Junimo Plush
                                // Stone Junimo
                                possibleDrops.AddRange(new Item[]
                                {
                                    new StardewValley.Object(Vector2.Zero, 272),
                                    new StardewValley.Object(Vector2.Zero, 117),
                                    new StardewValley.Objects.Furniture(1733, Vector2.Zero),
                                    new StardewValley.Object(Vector2.Zero, 55),
                                });
                                break;
                        }
                        drop = possibleDrops.ElementAt(random.Next(possibleDrops.Count));
                    }

                    SpaceCore.Skills.AddExperience(who, "drbirbdev.Binning", ModEntry.Config.ExperienceFromTrashBonus * (int)(Math.Pow(2, rarity)));

                    Vector2 origin = new Vector2((float)tileLocation.X + 0.5f, tileLocation.Y - 1) * 64f;
                    Game1.createItemDebris(drop, origin, 2, __instance, (int)origin.Y + 64);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Town_CheckAction_Postfix)}\n{ex}");
            }
        }

        public static void Utility_GetTrashReclamationPrice_Postfix(
            Item i,
            Farmer f,
            ref int __result)
        {
            try
            {
                if (f.HasCustomProfession(BinningSkill.Reclaimer))
                {
                    float sellPercentage = 0.15f * f.trashCanLevel + (ModEntry.Config.ReclaimerExtraValuePercent / 100.0f);
                    if (i.canBeTrashed())
                    {
                        if (i is StardewValley.Object && !(i as StardewValley.Object).bigCraftable)
                        {
                            __result = (int)((float)i.Stack * ((float)(i as StardewValley.Object).sellToStorePrice(-1L) * sellPercentage));
                        }
                        if (i is MeleeWeapon || i is StardewValley.Objects.Ring || i is StardewValley.Objects.Boots)
                        {
                            __result = (int)((float)i.Stack * ((float)(i.salePrice() / 2) * sellPercentage));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Utility_GetTrashReclamationPrice_Postfix)}\n{ex}");
            }

        }

        public static void NPC_GetGiftTasteForThisItem_Postfix(
            Item item,
            ref int __result)
        {
            try
            {
                if (Game1.player.HasCustomProfession(BinningSkill.Upseller))
                {
                    if (__result == 6)
                    {
                        __result = 4;
                    }
                    else if (__result == 4)
                    {
                        __result = 8;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(NPC_GetGiftTasteForThisItem_Postfix)}\n{ex}");
            }
        }

        public static void Object_PerformObjectDropInAction_Prefix(
            StardewValley.Object __instance,
            ref StardewValley.Object __state,
            Item dropInItem,
            bool probe,
            Farmer who)
        {
            try
            {
                // Check heldObject in the prefix.  Need to see if this was null to know if trash has been recycled.
                __state = __instance.heldObject.Value;
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Object_PerformObjectDropInAction_Prefix)}\n{ex}");
            }
        }

        public static void Object_PerformObjectDropInAction_Postfix(
            StardewValley.Object __instance,
            StardewValley.Object __state,
            Item dropInItem,
            bool probe,
            Farmer who)
        {
            try
            {
                if (probe)
                {
                    return;
                }
                if (!__instance.Name.Equals("Recycling Machine"))
                {
                    return;
                }
                if (__instance.isTemporarilyInvisible)
                {
                    return;
                }
                if (dropInItem is not StardewValley.Object)
                {
                    return;
                }
                if (dropInItem is StardewValley.Objects.Wallpaper)
                {
                    return;
                }
                StardewValley.Object dropIn = dropInItem as StardewValley.Object;
                if (dropIn.ParentSheetIndex == 872)
                {
                    return;
                }

                if (dropIn.ParentSheetIndex >= 168 && dropIn.ParentSheetIndex <= 172 && __state == null)
                {
                    if (who.HasCustomProfession(BinningSkill.Salvager))
                    {
                        Random random = new();
                        if (random.Next(100) < ModEntry.Config.SalvagerRareDropChance)
                        {
                            bool superRare = false;
                            if (random.Next(100) < ModEntry.Config.SalvagerSuperRareDropChance)
                            {
                                superRare = true;
                            }
                            switch (dropIn.ParentSheetIndex)
                            {
                                case 168:
                                    __instance.heldObject.Value = new StardewValley.Object(superRare ? 787 : 330, superRare ? 1 : random.Next(1, 4));
                                    break;
                                case 169:
                                    __instance.heldObject.Value = new StardewValley.Object(superRare ? 709 : 92, superRare ? random.Next(1, 4) : random.Next(5, 11));
                                    break;
                                case 170:
                                    __instance.heldObject.Value = new StardewValley.Object(superRare ? 848 : 386, superRare ? 1 : random.Next(1, 3));
                                    break;
                                case 171:
                                    __instance.heldObject.Value = new StardewValley.Object(superRare ? 909 : 384, superRare ? 1 : random.Next(1, 4));
                                    break;
                                case 172:
                                    __instance.heldObject.Value = new StardewValley.Object(superRare ? 814 : 771, superRare ? 1 : random.Next(5, 11));
                                    break;
                            }
                        }
                    }

                    if (who.HasCustomProfession(BinningSkill.Environmentalist))
                    {
                        if (who.stats.PiecesOfTrashRecycled % ModEntry.Config.RecyclingCountToGainFriendship == 0)
                        {
                            Utility.improveFriendshipWithEveryoneInRegion(who, ModEntry.Config.RecyclingFriendshipGain, 2);
                        }
                    }

                    SpaceCore.Skills.AddExperience(who, "drbirbdev.Binning", ModEntry.Config.ExperienceFromRecycling);
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Object_PerformObjectDropInAction_Postfix)}\n{ex}");
            }
        }
    
        public static void CraftingRecipe_Constructor_Postfix(
            CraftingRecipe __instance,
            string name,
            bool isCookingRecipe)
        {
            try
            {
                if (name.Equals("Recycling Machine") && Game1.player.HasCustomProfession(BinningSkill.Recycler))
                {
                    __instance.recipeList = new()
                    {
                        { 388, 15 },
                        { 390, 15 },
                        { 334, 1 }
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(CraftingRecipe_Constructor_Postfix)}\n{ex}");
            }
        }
    }
}
