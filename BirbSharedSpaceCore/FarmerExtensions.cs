using SpaceCore;
using StardewValley;

namespace BirbShared
{
    static class FarmerExtensions
    {
        public static bool HasCustomPrestigeProfession(this Farmer player, Skills.Skill.Profession profession)
        {
            return player.professions.Contains(profession.GetVanillaId() + 100);
        }
    }
}
