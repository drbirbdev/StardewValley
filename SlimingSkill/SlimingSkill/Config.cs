using BirbCore.Attributes;

namespace SlimingSkill;

[SConfig]
internal class Config
{
    [SConfig.PageLink("Experience")]
    [SConfig.PageLink("Perk")]
    [SConfig.PageLink("Professions")]

    [SConfig.PageBlock("Experience")]
    [SConfig.SectionTitle("ExperienceModifiers")]
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromSlimeKill { get; set; } = 5;
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromToughSlimeKill { get; set; } = 20;
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromRareSlimeKill { get; set; } = 50;
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromSlimeEgg { get; set; } = 30;
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromSlimeEggPress { get; set; } = 15;
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromSlimeBall { get; set; } = 50;

    [SConfig.PageBlock("Perks")]
    [SConfig.SectionTitle("SkillPerkModifiers")]
    [SConfig.Option(0, 10, 1)]
    public int ExtraSlimeValuePerLevel { get; set; } = 1;

    [SConfig.PageBlock("Professions")]
    [SConfig.SectionTitle("ProfessionModifiers")]

    [SConfig.SectionTitle("RancherProfessionModifiers")]


    [SConfig.SectionTitle("BreederProfessionModifiers")]


    [SConfig.SectionTitle("HatcherProfessionModifiers")]


    [SConfig.SectionTitle("HunterProfessionModifiers")]


    [SConfig.SectionTitle("PoacherProfessionModifiers")]


    [SConfig.SectionTitle("TamerProfessionModifier")]
    public int Dummy { get; set; }
}
