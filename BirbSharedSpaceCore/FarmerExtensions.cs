using SpaceCore;
using StardewValley;
using static SpaceCore.Skills.Skill;

namespace BirbShared
{
    static class FarmerExtensions
    {
        public static bool HasProfession(this Farmer player, string profession, bool checkPrestiged = false)
        {
            Profession p = BirbSkill.KeyedProfessions?[profession];
            if (p is null)
            {
                return false;
            }
            return player.professions.Contains(p.GetVanillaId() + (checkPrestiged ? 100 : 0));
        }
    }
}
