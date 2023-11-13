using System.Collections.Generic;
using BirbShared;

namespace SlimingSkill
{
    internal class SlimingSkill : SpaceCore.Skills.Skill
    {
        public const string ID = "drbirbdev.Sliming";

        public static KeyedProfession Rancher;
        public static KeyedProfession Breeder;
        public static KeyedProfession Hatcher;
        public static KeyedProfession Hunter;
        public static KeyedProfession Poacher;
        public static KeyedProfession Tamer;

        public SlimingSkill() : base(ID)
        {
            this.Icon = ModEntry.Assets.IconA;
            this.SkillsPageIcon = ModEntry.Assets.IconB;
            this.ExperienceBarColor = new Microsoft.Xna.Framework.Color(120, 255, 31);

            if (ModEntry.MargoLoaded)
            {
                this.ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4000, 6900, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 55000, 60000, 70000 };

                this.AddProfessions(
                    Rancher = new KeyedProfession(this, "Rancher", ModEntry.Assets.Rancher, ModEntry.Instance.Helper, null, ModEntry.Assets.RancherP),
                    Hunter = new KeyedProfession(this, "Hunter", ModEntry.Assets.Hunter, ModEntry.Instance.Helper, null, ModEntry.Assets.HunterP),
                    Breeder = new KeyedProfession(this, "Breeder", ModEntry.Assets.Breeder, ModEntry.Instance.Helper, null, ModEntry.Assets.BreederP),
                    Hatcher = new KeyedProfession(this, "Hatcher", ModEntry.Assets.Hatcher, ModEntry.Instance.Helper, null, ModEntry.Assets.HatcherP),
                    Poacher = new KeyedProfession(this, "Poacher", ModEntry.Assets.Poacher, ModEntry.Instance.Helper, null, ModEntry.Assets.PoacherP),
                    Tamer = new KeyedProfession(this, "Tamer", ModEntry.Assets.Tamer, ModEntry.Instance.Helper, null, ModEntry.Assets.TamerP)
                );
            }
            else
            {
                this.ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4000, 6900, 10000, 15000 };

                this.AddProfessions(
                    Rancher = new KeyedProfession(this, "Rancher", ModEntry.Assets.Rancher, ModEntry.Instance.Helper),
                    Hunter = new KeyedProfession(this, "Hunter", ModEntry.Assets.Hunter, ModEntry.Instance.Helper),
                    Breeder = new KeyedProfession(this, "Breeder", ModEntry.Assets.Breeder, ModEntry.Instance.Helper),
                    Hatcher = new KeyedProfession(this, "Hatcher", ModEntry.Assets.Hatcher, ModEntry.Instance.Helper),
                    Poacher = new KeyedProfession(this, "Poacher", ModEntry.Assets.Poacher, ModEntry.Instance.Helper),
                    Tamer = new KeyedProfession(this, "Tamer", ModEntry.Assets.Tamer, ModEntry.Instance.Helper)
                );
            }
        }

        private void AddProfessions(KeyedProfession lvl5A, KeyedProfession lvl5B, KeyedProfession lvl10A1, KeyedProfession lvl10A2, KeyedProfession lvl10B1, KeyedProfession lvl10B2)
        {
            this.Professions.Add(lvl5A);
            this.Professions.Add(lvl5B);
            this.ProfessionsForLevels.Add(new ProfessionPair(5, lvl5A, lvl5B));

            this.Professions.Add(lvl10A1);
            this.Professions.Add(lvl10A2);
            this.ProfessionsForLevels.Add(new ProfessionPair(10, lvl10A1, lvl10A2, lvl5A));

            this.Professions.Add(lvl10B1);
            this.Professions.Add(lvl10B2);
            this.ProfessionsForLevels.Add(new ProfessionPair(10, lvl10B1, lvl10B2, lvl5B));
        }

        public override string GetName()
        {
            return ModEntry.Instance.I18n.Get("skill.name");
        }

        public override List<string> GetExtraLevelUpInfo(int level)
        {
            List<string> result = new()
            {
                ModEntry.Instance.I18n.Get("skill.perk", new { bonus = 0 })
            };

            return result;
        }

        public override string GetSkillPageHoverText(int level)
        {
            return ModEntry.Instance.I18n.Get("skill.perk", new { bonus = level * 0 });
        }
    }
}
