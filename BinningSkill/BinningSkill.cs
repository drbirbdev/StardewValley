using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinningSkill.Core;
using Microsoft.Xna.Framework.Graphics;

namespace BinningSkill
{
    internal class BinningSkill : SpaceCore.Skills.Skill
    {
        public static KeyedProfession Recycler;
        public static KeyedProfession Sneak;
        public static KeyedProfession Environmentalist;
        public static KeyedProfession Salvager;
        public static KeyedProfession Upseller;
        public static KeyedProfession Reclaimer;

        public BinningSkill() : base("drbirbdev.Binning")
        {
            this.Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/iconA.png");
            this.SkillsPageIcon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/iconB.png");


            this.ExperienceCurve = new[] { 100, 300, 770, 1300, 2150, 3300, 4000, 6900, 10000, 15000 };
            this.ExperienceBarColor = new Microsoft.Xna.Framework.Color(99, 107, 107);

            this.AddProfessions(
                    Recycler = new KeyedProfession(this, "Recycler", ModEntry.Assets.Recycler),
                    Sneak = new KeyedProfession(this, "Sneak", ModEntry.Assets.Sneak),
                    Environmentalist = new KeyedProfession(this, "Environmentalist", ModEntry.Assets.Environmentalist),
                    Salvager = new KeyedProfession(this, "Salvager", ModEntry.Assets.Salvager),
                    Upseller = new KeyedProfession(this, "Upseller", ModEntry.Assets.Upseller),
                    Reclaimer = new KeyedProfession(this, "Reclaimer", ModEntry.Assets.Reclaimer, ModEntry.Config)
                );
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
                ModEntry.Instance.I18n.Get("skill.perk", new { bonus = 1 })
            };
            if (level == 3 || level == 6 || level == 9)
            {
                string rarity = ModEntry.Instance.I18n.Get($"skill.perk_rarity_{level}");
                result.Add(ModEntry.Instance.I18n.Get("skill.perk_bonus", new { rarity }));
            }
            
            return result;
        }

        public override string GetSkillPageHoverText(int level)
        {
            return ModEntry.Instance.I18n.Get("skill.perk", new { bonus = level });
        }
    }
}
