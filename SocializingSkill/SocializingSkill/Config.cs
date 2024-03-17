using BirbCore.Attributes;

namespace SocializingSkill;

[SConfig]
internal class Config
{
    [SConfig.PageLink("Experience")]
    [SConfig.PageLink("Perks")]
    [SConfig.PageLink("Professions")]

    [SConfig.PageBlock("Experience")]
    [SConfig.SectionTitle("ExperienceModifiers")]
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromTalking { get; set; } = 5;
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromGifts { get; set; } = 5;
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromEvents { get; set; } = 20;
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromQuests { get; set; } = 50;
    [SConfig.Option(1, 5, 0.1f)]
    public float LovedGiftExpMultiplier { get; set; } = 2;
    [SConfig.Option(1, 5, 0.1f)]
    public float BirthdayGiftExpMultiplier { get; set; } = 5;

    [SConfig.PageBlock("Perks")]
    [SConfig.SectionTitle("SkillPerkModifiers")]
    [SConfig.Option(1, 10, 1)]
    public int ChanceNoFriendshipDecayPerLevel { get; set; } = 5;

    [SConfig.PageBlock("Professions")]
    [SConfig.SectionTitle("ProfessionModifiers")]

    [SConfig.SectionTitle("FriendlyProfessionModifiers")]
    [SConfig.Option(0, 100, 1)]
    public int FriendlyExtraFriendship { get; set; } = 10;


    [SConfig.SectionTitle("SmoothTalkerProfessionModifiers")]
    [SConfig.Option(-5.0f, 5.0f, 0.1f)]
    public float SmoothTalkerPositiveMultiplier { get; set; } = 2.0f;
    [SConfig.Option(-5.0f, 5.0f, 0.1f)]
    public float SmoothTalkerNegativeMultiplier { get; set; } = 0.5f;


    [SConfig.SectionTitle("GifterProfessionModifiers")]
    [SConfig.Option(0, 100, 1)]
    public int GifterLovedGiftExtraFriendship { get; set; } = 80;
    [SConfig.Option(0, 100, 1)]
    public int GifterLikedGiftExtraFriendship { get; set; } = 40;
    [SConfig.Option(0, 100, 1)]
    public int GifterNeutralGiftExtraFriendship { get; set; } = 20;

    [SConfig.SectionTitle("HelpfulProfessionModifiers")]
    [SConfig.Option(1.0f, 5.0f, 0.1f)]
    public float HelpfulRewardMultiplier { get; set; } = 2.0f;


    [SConfig.SectionTitle("HagglerProfessionModifiers")]
    [SConfig.Option(1, 10, 1)]
    public int HagglerMinHeartLevel { get; set; } = 6;
    [SConfig.Option(0, 10, 1)]
    public int HagglerDiscountPercentPerHeartLevel { get; set; } = 2;


    [SConfig.SectionTitle("BelovedProfessionModifiers")]
    [SConfig.Option(0, 100, 1)]
    public int BelovedGiftPercentChance { get; set; } = 5;
    [SConfig.Option(0, 100, 1)]
    public int BelovedGiftRarePercentChance { get; set; } = 30;
    [SConfig.Option(0, 100, 1)]
    public int BelovedGiftSuperRarePercentChance { get; set; } = 30;
}
