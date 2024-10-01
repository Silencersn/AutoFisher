namespace AutoFisher.Common.Configs.SeverConfigs
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
        public bool EnableMod = true;
        public bool EnableMultipleFishingLines = true;
        public bool EnableFilters = true;
        public bool EnableAutoOpen = true;
        public bool EnableAutoUse = true;
        public bool EnableAutoSell = true;
        public bool EnableAutoSpawnNPC = true;
        public bool EnableAutoFindBaits = true;
    }

    public class Regulation
    {
        public bool ConsumeBait = true;
        public bool BreakFishingLine = true;
        public bool FishInWater = true;
        public bool FishInShimmer = false;
        public bool ChangeHeldItemWhenFishing = false;
    }

    public class FishingPowerInfluences
    {
        private bool allLuck = true;
        private bool positiveLuck = false;

        public bool OnlyPositiveInfluences = false;
        public bool Weather = true;
        public bool Time = true;
        public bool Moon = true;
        public bool LakeSize = true;
        public bool Luck
        {
            get => allLuck;
            set
            {
                allLuck = value;
                positiveLuck &= value;
            }
        }
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
        public bool CanCatchQuestFishWhenSameOneIsInInventory = true;
        public bool CanCatchQuestFishWhenAnglerNotExists = false;
        public bool CanCatchQuestFishWhenAnglerQuestIsFinished = true;
        public bool CanPickUpQuestFishWhenSameOneIsInInventory = true;
        public bool ChangeAnglerQuestAfterThatIsFinished = false;
        [ReloadRequired]
        public bool IncreaseQuestFishMaxStack = true;
    }

    public class AnglerArmorsGenerateEffects
    {
        private bool inInventory = false;
        private bool inInventoryAndFavorited = false;

        public bool InVanitySlots = true;
        public bool InInventoryOrVoidBag
        {
            get => inInventory;
            set
            {
                inInventory = value;
                inInventoryAndFavorited &= !value;
            }
        }
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
