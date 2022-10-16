using HarmonyLib;
using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Locations;
using Netcode;
using SpaceCore;
using Microsoft.Xna.Framework;
using StardewValley.Tools;
using BirbShared;
using BinningSkill.Core;
using StardewValley.Objects;

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

                if (ModEntry.RSVLoaded)
                {
                    harmony.Patch(
                        original: AccessTools.Method(AccessTools.TypeByName("RidgesideVillage.TrashCans"), "Trigger"),
                        prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(RidgesideVillage_TrashCans_Trigger_Prefix)),
                        postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(RidgesideVillage_TrashCans_Trigger_Postfix)));
                }

                // Reclaimer Profession
                //  - give more profit from reclaiming
                harmony.Patch(
                    original: AccessTools.Method(typeof(Utility), nameof(Utility.getTrashReclamationPrice)),
                    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Utility_GetTrashReclamationPrice_Postfix)) { after = new string[] {"shivaGuy.BetterTrashCan"} });

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
                    original: AccessTools.Constructor(typeof(StardewValley.CraftingRecipe), new Type[] { typeof(string), typeof(bool) }),
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
                        // TODO: skip this stuff by transpiling.  Will fix global chat message being displayed with sneak profession
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

                    DoTrashCanCheck("Town", __state[0].ToString(), __state[1], GetItemPosition(new Vector2(tileLocation.X, tileLocation.Y)));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(Town_CheckAction_Postfix)}\n{ex}");
            }
        }

        public static void RidgesideVillage_TrashCans_Trigger_Prefix(
            ref object[] __state,
            string tileAction,
            Vector2 position,
            HashSet<Vector2> ___TrashCansTriggeredToday)
        {
            try
            {
                if (___TrashCansTriggeredToday.Contains(position))
                {
                    return;
                }

                __state = new object[]
                {
                    tileAction.Split(' ')[1],
                    Game1.currentLocation.debris.Count,
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(RidgesideVillage_TrashCans_Trigger_Prefix)}\n{ex}");
            }
        }

        public static void RidgesideVillage_TrashCans_Trigger_Postfix(
            object[] __state,
            string tileAction,
            Vector2 position)
        {
            try
            {
                if (__state != null && __state.Length > 0)
                {
                    if (!Game1.player.HasCustomProfession(BinningSkill.Sneak))
                    {
                        Character c = Utility.isThereAFarmerOrCharacterWithinDistance(position, 7, Game1.currentLocation);
                        if (c != null && c is NPC && c is not StardewValley.Characters.Horse)
                        {
                            // Sneak Profession
                            // RSV doesn't have people catch you in trashcans, so just do some vanilla behaviour to add that.

                            // TODO: broadcast chat messages
                            // Game1.multiplayer.globalChatInfoMessage("TrashCan", Game1.player.Name, c.name);
                            int friendshipLoss = -ModEntry.Config.FriendshipRecovery;
                            if (c.Name.Equals("Linus"))
                            {
                                c.doEmote(32);
                                (c as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Linus"), add: true, clearOnMovement: true);
                                Game1.player.changeFriendship(5, c as NPC);
                                // Game1.multiplayer.globalChatInfoMessage("LinusTrashCan");
                            }
                            else if ((c as NPC).Age == 2)
                            {
                                c.doEmote(28);
                                (c as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Child"), add: true, clearOnMovement: true);
                                Game1.player.changeFriendship(friendshipLoss, c as NPC);
                            }
                            else if ((c as NPC).Age == 1)
                            {
                                c.doEmote(8);
                                (c as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Teen"), add: true, clearOnMovement: true);
                                Game1.player.changeFriendship(friendshipLoss, c as NPC);
                            }
                            else
                            {
                                c.doEmote(12);
                                (c as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Adult"), add: true, clearOnMovement: true);
                                Game1.player.changeFriendship(friendshipLoss, c as NPC);
                            }
                            Game1.drawDialogue(c as NPC);
                        }
                    }

                    DoTrashCanCheck("RSV", (string)__state[0], (int)__state[1], GetItemPosition(position));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed in {nameof(RidgesideVillage_TrashCans_Trigger_Postfix)}\n{ex}");
            }
        }

        private static void DoTrashCanCheck(string location, string whichCan, int debrisCount, Vector2 position)
        {
            if (Game1.currentLocation.debris is not null && Game1.currentLocation.debris.Count > debrisCount)
            {
                // something was already gotten from the trash, give standard binning experience and return
                Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromTrashSuccess);
                return;

            }

            int rarity = GetRarity(GetRarityLevels());

            if (rarity < 0)
            {
                // Give binning experience for not getting anything
                Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromTrashFail);
                return;
            }

            Item drop = GetRandomDropFromLootTable(location, whichCan, rarity);

            if (rarity == 2)
            {
                Game1.currentLocation.playSound("yoba");
            }
            else if (rarity == 3)
            {
                Game1.currentLocation.playSound("reward");
            }

            Skills.AddExperience(Game1.player, "drbirbdev.Binning", ModEntry.Config.ExperienceFromTrashBonus * (int)(Math.Pow(2, rarity)));

            Game1.createItemDebris(drop, position, 2, Game1.currentLocation, (int)position.Y + 64);
        }

        private static Vector2 GetItemPosition(Vector2 tilePosition)
        {
            return new Vector2(tilePosition.X + 0.5f, tilePosition.Y - 1) * 64f;
        }

        public static int[] GetRarityLevels()
        {
            int binningLevel = Skills.GetSkillLevel(Game1.player, "drbirbdev.Binning");
            return new int[]
            {
                        binningLevel * ModEntry.Config.PerLevelBonusDropChance,
                        binningLevel >= 3 ? ModEntry.Config.RareDropChance : 0,
                        binningLevel >= 6 ? ModEntry.Config.SuperRareDropChance : 0,
                        binningLevel >= 9 ? ModEntry.Config.UltraRareDropChance : 0,
            };
        }

        /// <summary>
        /// Determine the rarity level of a drop.
        /// </summary>
        /// <param name="chances"></param>
        /// <returns></returns>
        public static int GetRarity(int[] chances)
        {
            Random random = new();
            int rarity = -1;
            for (int i = 0; i < chances.Length; i++)
            {
                if (random.Next(100) < chances[i])
                {
                    rarity++;
                }
                else
                {
                    return rarity;
                }
            }
            Log.Debug($"Using rarity {rarity}");
            return rarity;
        }

        public static Item GetRandomDropFromLootTable(string location, string whichCan, int rarity)
        {
            Random random = new();
            if (rarity < 0 || rarity > 3)
            {
                Log.Error($"Invalid rarity level {rarity}");
            }
            string key = $"{location}.{whichCan}.{rarity}";
            string defaultKey = $"{location}.*.{rarity}";
            List<string> possibleIds;
            if (ModEntry.Assets.TrashDrops.ContainsKey(key))
            {
                possibleIds = ModEntry.Assets.TrashDrops[key];
            }
            else if (ModEntry.Assets.TrashDrops.ContainsKey(defaultKey))
            {
                possibleIds = ModEntry.Assets.TrashDrops[defaultKey];
            }
            else
            {
                Log.Error($"Found no specific or default drops for key {key}");
                possibleIds = new List<string>() { "item.168" };
            }
            Log.Info($"Used key {key} to find loot {string.Join(',', possibleIds)}");
            return ParseTrashDropId(possibleIds[random.Next(possibleIds.Count)]);
        }

        public static Item ParseTrashDropId(string id)
        {
            try
            {
                string[] parts = id.Split('.');
                string itemType = parts[0];
                string itemId = parts[1];

                if (!ModEntry.JALoaded && parts[0].StartsWith("ja_"))
                {
                    Log.Error($"Tried to parse JsonAssets trash drop but mod isn't loaded {id}");
                    return new StardewValley.Object(168, 1);
                }
                if (!ModEntry.DGALoaded && parts[0].StartsWith("dga_"))
                {
                    Log.Error($"Tried to parse DynamicGameAssets trash drop but mod isn't loaded {id}");
                    return new StardewValley.Object(168, 1);
                }

                switch (itemType)
                {
                    case "item":
                        return new StardewValley.Object(int.Parse(itemId), 1);
                    case "bigcraftable":
                        return new StardewValley.Object(Vector2.Zero, int.Parse(itemId));
                    case "bedfurniture":
                        return new BedFurniture(int.Parse(itemId), Vector2.Zero);
                    case "boots":
                        return new Boots(int.Parse(itemId));
                    case "clothing":
                        return new Clothing(int.Parse(itemId));
                    case "furniture":
                        return new Furniture(int.Parse(itemId), Vector2.Zero);
                    case "hat":
                        return new Hat(int.Parse(itemId));                        
                    case "ring":
                        return new Ring(int.Parse(itemId));
                    case "storagefurniture":
                        return new StorageFurniture(int.Parse(itemId), Vector2.Zero);
                    case "weapon":
                        return new MeleeWeapon(int.Parse(itemId));
                    case "ja_item":
                        return new StardewValley.Object(ModEntry.JsonAssets.GetObjectId(itemId), 1);
                    case "ja_bigcraftable":
                        return new StardewValley.Object(Vector2.Zero, ModEntry.JsonAssets.GetBigCraftableId(itemId));
                    case "ja_hat":
                        return new Hat(ModEntry.JsonAssets.GetHatId(itemId));
                    case "ja_weapon":
                        return new MeleeWeapon(ModEntry.JsonAssets.GetWeaponId(itemId));
                    case "ja_clothing":
                        return new Clothing(ModEntry.JsonAssets.GetClothingId(itemId));
                    case "dga_item":
                        return (Item)ModEntry.DynamicGameAssets.SpawnDGAItem(itemId);
                    default:
                        Log.Error($"Failed to parse drop type {id}");
                        return new StardewValley.Object(168, 1);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to parse drop id {id}\n{ex}");
                return new StardewValley.Object(168, 1);
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
                    if (__result < 0)
                    {
                        return;
                    }
                    float extraPercentage = (ModEntry.Config.ReclaimerExtraValuePercent / 100.0f);
                    int extraAmount = 0;
                    if (i.canBeTrashed())
                    {
                        if (i is StardewValley.Object && !(i as StardewValley.Object).bigCraftable)
                        {
                            extraAmount = (int)((float)i.Stack * ((float)(i as StardewValley.Object).sellToStorePrice(-1L) * extraPercentage));
                        }
                        if (i is MeleeWeapon || i is Ring || i is Boots)
                        {
                            extraAmount = (int)((float)i.Stack * ((float)(i.salePrice() / 2) * extraPercentage));
                        }
                    }
                    __result += extraAmount;
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
