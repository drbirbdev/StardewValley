using BinningSkill.Core;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;

namespace BinningSkill
{
    internal class KeyedProfession : SpaceCore.Skills.Skill.Profession
    {
        readonly object Tokens;
        public KeyedProfession(Skills.Skill skill, string id) : base(skill, id)
        {
            this.Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>($"Mods/drbirbdev.BinningSkill/{id}");
        }

        public KeyedProfession(SpaceCore.Skills.Skill skill, string id, object tokens) : base(skill, id)
        {
            this.Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>($"Mods/drbirbdev.BinningSkill/{id}");
            this.Tokens = tokens;
        }

        public override string GetDescription()
        {
            return ModEntry.Instance.I18n.Get($"{this.Id}.desc", this.Tokens);
        }

        public override string GetName()
        {
            return ModEntry.Instance.I18n.Get($"{this.Id}.name", this.Tokens);
        }
    }
}
