using BinningSkill.Core;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;

namespace BinningSkill
{
    internal class KeyedProfession : SpaceCore.Skills.Skill.Profession
    {
        readonly object Tokens;
        public KeyedProfession(Skills.Skill skill, string id, Texture2D icon) : base(skill, id)
        {
            this.Icon = icon;
        }

        public KeyedProfession(SpaceCore.Skills.Skill skill, string id, Texture2D icon, object tokens) : base(skill, id)
        {
            this.Icon = icon;
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
