using BirbShared.Config;

namespace BinningSkill
{
    [ConfigClass(I18NNameSuffix = "", I18NTextSuffix = "")]
    public class Config
    {
        [ConfigPageLink("Experience", "ExperienceModifiers")]
        [ConfigPageLink("BonusDrops", "BonusDropModifiers")]
        [ConfigPageLink("Professions", "ProfessionModifiers")]

        [ConfigPage("Experience")]
        [ConfigSectionTitle("ExperienceModifiers")]
        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int ExperienceFromCheckingTrash { get; set; } = 5;

        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int ExperienceFromCheckingRecycling { get; set; } = 2;

        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int ExperienceFromComposting { get; set; } = 2;

        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int ExperienceFromRecycling { get; set; } = 2;

        // Increase in chance of any drop per level.
        [ConfigPage("BonusDrops")]
        [ConfigSectionTitle("BonusDropModifiers")]
        [ConfigOption(Min = 0, Max = 1, Interval = 0.001f)]
        public float PerLevelBaseDropChanceBonus { get; set; } = 0.03f;

        // Increase in chance for rare drops (using drbirbdev.BinningSkill_RANDOM condition
        [ConfigOption(Min = 0, Max = 1, Interval = 0.001f)]
        public float PerLevelRareDropChanceBonus { get; set; } = 0.01f;

        // What level to Mega drops become available
        [ConfigOption(Min = 0, Max = 10, Interval = 1)]
        public int MegaMinLevel { get; set; } = 4;

        // What level to DoubleMega drops become available
        [ConfigOption(Min = 0, Max = 10, Interval = 1)]
        public int DoubleMegaMinLevel { get; set; } = 7;

        [ConfigPage("Professions")]
        [ConfigSectionTitle("ProfessionModifiers")]


        // Recycler
        // No configs associated
        [ConfigSectionTitle("RecyclerProfessionModifiers")]
        [ConfigParagraph("None")]


        // Environmentalist
        [ConfigSectionTitle("EnvironmentalistProfessionModifiers")]
        // Gain friendship for every N recyclables
        [ConfigOption(Min = 100, Max = 10000, Interval = 100)]
        public int RecyclingCountToGainFriendship { get; set; } = 1000;
        // Amount of friendship to gain each time
        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int RecyclingFriendshipGain { get; set; } = 10;
        // Additional friendship if prestiged
        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int RecyclingPrestigeFriendshipGain { get; set; } = 10;


        // Salvager
        [ConfigSectionTitle("SalvagerProfessionModifiers")]
        [ConfigParagraph("None")]


        // Sneak
        [ConfigSectionTitle("SneakProfessionModifiers")]
        // How quiet is sneaking.  Default noise range is 7, so a value of 7 removes all noise.
        [ConfigOption(Min = 0, Max = 25, Interval = 1)]
        public int NoiseReduction { get; set; } = 5;
        // How loud when diging through trash gives friendship.
        [ConfigOption(Min = 0, Max = 25, Interval = 1)]
        public int PrestigeNoiseIncrease { get; set; } = 0;


        // Upseller
        [ConfigSectionTitle("UpsellerProfessionModifiers")]
        [ConfigParagraph("None")]
        // No configs associated


        // Reclaimer
        [ConfigSectionTitle("ReclaimerProfessionModifiers")]
        // The amount of extra value that the reclaimer skill provides.
        [ConfigOption(Min = 0, Max = 1, Interval = 0.01f)]
        public float ReclaimerExtraValuePercent { get; set; } = 0.2f;
        [ConfigOption(Min = 0, Max = 1, Interval = 0.01f)]
        public float ReclaimerPrestigeExtraValuePercent { get; set; } = 0.2f;

    }
}
