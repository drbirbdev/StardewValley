using System;
using System.Collections.Generic;
using BirbCore;
using BirbShared.APIs;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;

namespace BirbShared
{
    internal class Utilities
    {
        public static int GetIntData(Item item, string key, int defaultValue = 0)
        {
            if (!item.modData.TryGetValue(key, out string value) || !int.TryParse(value, out int intValue))
            {
                intValue = defaultValue;
            }
            return intValue;
        }

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
            Log.Trace($"Using rarity {rarity}");
            return rarity;
        }

        public static string GetRandomDropStringFromLootTable(Dictionary<string, List<string>> table, string primary, string secondary, string tertiary)
        {
            Random random = new();

            List<string> possibleIds = null;
            foreach (string key in GetPossibleKeys(primary, secondary, tertiary))
            {
                if (table.ContainsKey(key))
                {
                    possibleIds = table[key];
                    Log.Trace($"Using key {key} to find possible loot");
                    break;
                }
            }
            if (possibleIds == null)
            {
                possibleIds = new List<string>() { "item.168" };
                Log.Error($"Found no specific or default drops for key {primary}.{secondary}.{tertiary}");

            }

            return possibleIds[random.Next(possibleIds.Count)];
        }

        internal static IEnumerable<string> GetPossibleKeys(string primary, string secondary, string tertiary)
        {
            yield return $"{primary}.{secondary}.{tertiary}";
            yield return $"{primary}.{secondary}.*";
            yield return $"{primary}.*.{tertiary}";
            yield return $"{primary}.*.*";
            yield return $"*.{secondary}.{tertiary}";
            yield return $"*.{secondary}.*";
            yield return $"*.*.{tertiary}";
            yield return $"*.*.*";
        }

        public static Item ParseDropString(string id)
        {
            try
            {
                string[] parts = id.Split('.');
                string itemId = parts[0];
                int itemCount = 1;
                if (parts.Length > 1)
                {
                    itemCount = int.Parse(parts[1]);
                }

                return new StardewValley.Object(itemId, itemCount);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to parse drop id {id}\n{ex}");
                return new StardewValley.Object("(O)168", 1);
            }
        }
    }
}
