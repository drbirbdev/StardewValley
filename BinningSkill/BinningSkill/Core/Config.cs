using BirbShared.Config;

namespace BinningSkill
{
    [ConfigClass(I18NNameSuffix = "")]
    public class Config
    {
        [ConfigPageLink("Experience", "Experience Modifiers")]
        [ConfigPageLink("BonusDrops", "Bonus Drop Modifiers")]
        [ConfigPageLink("Professions", "Profession Modifiers")]

        [ConfigPage("Experience")]
        [ConfigSectionTitle("Experience Modifiers")]
        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int ExperienceFromCheckingTrash { get; set; } = 5;

        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int ExperienceFromCheckingRecycling { get; set; } = 2;

        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int ExperienceFromComposting { get; set; } = 3;

        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int ExperienceFromRecycling { get; set; } = 2;

        [ConfigOption]
        public bool AutomateGrantsXp { get; set; } = true;

        // Increase in chance of bonus drop per level.
        [ConfigPage("BonusDrops")]
        [ConfigSectionTitle("Bonus Drop Modifiers")]
        [ConfigOption(Min = 0, Max = 10, Interval = 1)]
        public int PerLevelBonusDropChance { get; set; } = 3;

        // If getting an bonus drop, what is the chance it is rare?
        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int RareDropChance { get; set; } = 20;

        // If getting a rare bonus drop, what is the chance it is super rare?
        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int SuperRareDropChance { get; set; } = 20;

        // If getting a super rare bonus drop, what is the chance it is ultra rare?
        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int UltraRareDropChance { get; set; } = 20;

        [ConfigPage("Professions")]
        [ConfigSectionTitle("Profession Modifiers")]
        // Recycler
        // No configs associated
        [ConfigSectionTitle("Recycler Profession Modifiers")]
        [ConfigParagraph("None")]

        // Environmentalist
        [ConfigSectionTitle("Environmentalist Profession Modifiers")]
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
        [ConfigSectionTitle("Salvager Profession Modifiers")]
        // Chance of rare recyleable output when salvaging
        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int SalvagerRareDropChance { get; set; } = 20;
        // If getting a rare salvage, what is the chance it is super rare?
        [ConfigOption(Min = 0, Max = 100, Interval = 1)]
        public int SalvagerSuperRareDropChance { get; set; } = 20;


        // Sneak
        [ConfigSectionTitle("Sneak Profession Modifiers")]
        // The amount of friendship needed to undo getting caught digging through a garbage can.
        // If this is customized in another mod, then adjust this value to have no impact on friendship when using Sneak profession.
        [ConfigOption(Min = 0)]
        public int FriendshipRecovery { get; set; } = 25;

        // Upseller
        [ConfigSectionTitle("Upseller Profession Modifiers")]
        [ConfigParagraph("None")]
        // No configs associated

        // Reclaimer
        [ConfigSectionTitle("Reclaimer Profession Modifiers")]
        // The amount of extra value that the reclaimer skill provides.
        // TODO: Apply
        [ConfigOption(Min = 0, Max = 35, Interval = 1)]
        public int ReclaimerExtraValuePercent { get; set; } = 20;


    }
}
