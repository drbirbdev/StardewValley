using BirbCore.Attributes;

namespace BinningSkill;

[SConfig]
public class Config
{
    [SConfig.PageLink("Experience")]
    [SConfig.PageLink("BonusDrops")]
    [SConfig.PageLink("Professions")]
    [SConfig.PageBlock("Experience")]
    [SConfig.SectionTitle("ExperienceModifiers")]
    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromCheckingTrash { get; set; } = 5;

    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromCheckingRecycling { get; set; } = 2;

    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromComposting { get; set; } = 2;

    [SConfig.Option(0, 100, 1)]
    public int ExperienceFromRecycling { get; set; } = 2;

    // Increase in chance of any drop per level.
    [SConfig.PageBlock("BonusDrops")]
    [SConfig.SectionTitle("BonusDropModifiers")]
    [SConfig.Option(0, 0.1f, 0.001f)]
    public float PerLevelBaseDropChanceBonus { get; set; } = 0.03f;

    // Increase in chance for rare drops (using drbirbdev.BinningSkill_RANDOM condition
    [SConfig.Option(0, 0.1f, 0.001f)]
    public float PerLevelRareDropChanceBonus { get; set; } = 0.001f;

    // What level to Mega drops become available
    [SConfig.Option(0, 10, 1)]
    public int MegaMinLevel { get; set; } = 4;

    // What level to DoubleMega drops become available
    [SConfig.Option(0, 10, 1)]
    public int DoubleMegaMinLevel { get; set; } = 7;

    [SConfig.PageBlock("Professions")]
    [SConfig.SectionTitle("ProfessionModifiers")]

    // Recycler
    // No configs associated
    [SConfig.SectionTitle("RecyclerProfessionModifiers")]
    [SConfig.Paragraph("None")]

    // Environmentalist
    [SConfig.SectionTitle("EnvironmentalistProfessionModifiers")]
    // Gain friendship for every N recyclables
    [SConfig.Option(100, 10000, 100)]
    public int RecyclingCountToGainFriendship { get; set; } = 1000;

    // Amount of friendship to gain each time
    [SConfig.Option(0, 100, 1)]
    public int RecyclingFriendshipGain { get; set; } = 10;

    // Additional friendship if prestiged
    [SConfig.Option(0, 100, 1)]
    public int RecyclingPrestigeFriendshipGain { get; set; } = 10;


    // Salvager
    [SConfig.SectionTitle("SalvagerProfessionModifiers")]
    [SConfig.Paragraph("None")]

    // Sneak
    [SConfig.SectionTitle("SneakProfessionModifiers")]
    // How quiet is sneaking.  Default noise range is 7, so a value of 7 removes all noise.
    [SConfig.Option(0, 25, 1)]
    public int NoiseReduction { get; set; } = 5;

    // How loud when digging through trash gives friendship.
    [SConfig.Option(0, 25, 1)]
    public int PrestigeNoiseIncrease { get; set; } = 0;


    // Upseller
    [SConfig.SectionTitle("UpsellerProfessionModifiers")]
    [SConfig.Paragraph("None")]
    // No configs associated

    // Reclaimer
    [SConfig.SectionTitle("ReclaimerProfessionModifiers")]
    // The amount of extra value that the reclaimer skill provides.
    [SConfig.Option(0, 1, 0.01f)]
    public float ReclaimerExtraValuePercent { get; set; } = 0.2f;

    [SConfig.Option(0, 1, 0.01f)]
    public float ReclaimerPrestigeExtraValuePercent { get; set; } = 0.2f;
}
