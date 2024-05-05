using System.ComponentModel;

namespace AutoFisher.Configs.SeverConfigs
{
    public class AutoFisher_Common_SeverConifg : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public AllowPlayers AllowPlayers = new();
        public Regulation Regulation = new();
        public FishingPowerInfluences FishingPowerInfluences = new();
        public FishingQuests FishingQuests = new();
        public AnglerArmorsGenerateEffects AnglerArmorsGenerateEffects = new();

    }

    public class AllowPlayers
    {
        [DefaultValue(true)]
        public bool EnableMod = true;
        [DefaultValue(true)]
        public bool EnableMultipleFishingLines = true;
        [DefaultValue(true)]
        public bool EnableFilters = true;
        [DefaultValue(true)]
        public bool EnableAutoOpen = true;
        [DefaultValue(true)]
        public bool EnableAutoUse = true;
        [DefaultValue(true)]
        public bool EnableAutoSell = true;
        [DefaultValue(true)]
        public bool EnableAutoSpawnNPC = true;
        [DefaultValue(true)]
        public bool EnableAutoFindBaits = true;
    }

    public class Regulation
    {
        [DefaultValue(true)]
        public bool ConsumeBait = true;
        [DefaultValue(true)]
        public bool BreakFishingLine = true;
        [DefaultValue(true)]
        public bool FishInWater = true;
        [DefaultValue(false)]
        public bool FishInShimmer = false;

    }

    public class FishingPowerInfluences
    {
        private bool allLuck = true;
        private bool positiveLuck = false;

        [DefaultValue(true)]
        public bool Weather = true;
        [DefaultValue(true)]
        public bool Time = true;
        [DefaultValue(true)]
        public bool Moon = true;
        [DefaultValue(true)]
        public bool LakeSize = true;
        [DefaultValue(true)]
        public bool Luck
        {
            get => allLuck;
            set
            {
                allLuck = value;
                positiveLuck &= value;
            }
        }
        [DefaultValue(false)]
        public bool OnlyPositiveLuck
        {
            get => positiveLuck;
            set
            {
                positiveLuck = value;
                allLuck |= value;
            }
        }
    }

    public class FishingQuests
    {
        [DefaultValue(true)]
        public bool CanCatchQuestFishWhenSameOneIsInInventory = true;
        [DefaultValue(false)]
        public bool CanCatchQuestFishWhenAnglerNotExists = false;
        [DefaultValue(true)]
        public bool CanCatchQuestFishWhenAnglerQuestIsFinished = true;
        [DefaultValue(true)]
        public bool CanPickUpQuestFishWhenSameOneIsInInventory = true;
        [DefaultValue(false)]
        public bool ChangeAnglerQuestAfterThatIsFinished = false;
    }

    public class AnglerArmorsGenerateEffects
    {
        private bool inInventory = false;
        private bool inInventoryAndFavorited = false;

        [DefaultValue(true)]
        public bool InVanitySlots = true;
        [DefaultValue(true)]
        public bool InInventoryOrVoidBag
        {
            get => inInventory;
            set
            {
                inInventory = value;
                inInventoryAndFavorited &= !value;
            }
        }
        [DefaultValue(false)]
        public bool InInventoryOrVoidBagAndFavorited
        {
            get => inInventoryAndFavorited;
            set
            {
                inInventoryAndFavorited = value;
                inInventory &= !value;
            }
        }
    }
}
