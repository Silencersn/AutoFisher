using System.ComponentModel;

namespace AutoFisher.Configs.ClientConfigs
{
    public class AutoFisher_Common_ClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool EnableMod;

        public CatchesInfomation CatchesInfomation = new();
        public MultipleFishingLines MultipleFishingLines = new();
        public AutoOpen AutoOpen = new();
        public AutoUse AutoUse = new();
        public AutoSell AutoSell = new();
        public AutoSpawnNPC AutoSpawnNPC = new();
        public AutoFindBaits AutoFindBaits = new();
        public Filters Filters = new();
    }

    public class CatchesInfomation
    {
        [DefaultValue(true)]
        public bool Enable = true;

        [DefaultValue(true)]
        public bool ShowTimeInfomation = true;
        [DefaultValue(true)]
        public bool ShowFilterInfomation = true;
        [DefaultValue(true)]
        public bool ShowAutoOpenInfomation = true;
        [DefaultValue(true)]
        public bool ShowAutoSellInfomation = true;
        [DefaultValue(true)]
        public bool ShowComsumedBaitInfomation = true;
        [DefaultValue(true)]
        public bool ShowFilteredCatchesInfomation = true;
        [DefaultValue(true)]
        public bool ShowNotCaughtCatchesInfomation = true;
    }
    public class MultipleFishingLines
    {
        [DefaultValue(false)]
        public bool Enable;
        [DefaultValue(1)]
        [Range(1, 256)]
        [Slider]
        public int FishingLinesCount;
        [Range(1, 256)]
        [DefaultValue(1)]
        [Increment(1)]
        [ShowDespiteJsonIgnore]
        public int FishingLinesCountIncrement
        {
            get => FishingLinesCount;
            set => FishingLinesCount = value;
        }
    }
    public class AutoOpen
    {
        [DefaultValue(false)]
        public bool Enable;
        [DefaultValue(false)]
        public bool AutoOpenCrates;
        [DefaultValue(false)]
        public bool AutoOpenOysters;
    }
    public class AutoUse
    {
        [DefaultValue(false)]
        public bool Enable;
        [DefaultValue(false)]
        public bool AutoUseFishingPotions;
        [DefaultValue(false)]
        public bool AutoUseCratePotions;
        [DefaultValue(false)]
        public bool AutoUseSonarPotions;
        [DefaultValue(false)]
        public bool AutoUseAlesOrSakes;
        [DefaultValue(0)]
        [Range(0, 3)]
        [Increment(1)]
        public int AutoUseChumBuckets;
    }
    public class AutoSell
    {
        [DefaultValue(false)]
        public bool Enable;
        [DefaultValue(false)]
        public bool AutoSellAllCatches;
        [DefaultValue(false)]
        public bool AutoSellFilteredCatches;
        [DefaultValue(false)]
        public bool AutoSellUnfilteredCatches;
    }
    public class AutoSpawnNPC
    {
        private bool catchNPC;
        private bool killNPC;

        [DefaultValue(false)]
        public bool Enable;
        [DefaultValue(false)]
        public bool AutoCatchNPC
        {
            get => catchNPC;
            set
            {
                catchNPC = value;
                killNPC &= value;
            }
        }
        [DefaultValue(false)]
        public bool AutoKillNPC
        {
            get => killNPC;
            set
            {
                killNPC = value;
                catchNPC |= value;
            }
        }
    }
    public class AutoFindBaits
    {
        [DefaultValue(false)]
        public bool Enable;
        [DefaultValue(true)]
        public bool AutoFindBaitsInVoidBag;
        [DefaultValue(false)]
        public bool AutoFindBaitsInPiggyBank;
        [DefaultValue(false)]
        public bool AutoFindBaitsInSafe;
        [DefaultValue(false)]
        public bool AutoFindBaitsInDefendersForge;
    }
    public class Filters
    {
        [DefaultValue(false)]
        public bool Enable;
        [ShowDespiteJsonIgnore]
        public bool EnableCatchQualityFilter => ConfigContent.Client.CatchQualityFilter is not null && ConfigContent.Client.CatchQualityFilter.EnableCatchQualityFilter;
        [ShowDespiteJsonIgnore]
        public bool EnableRarityFilter => ConfigContent.Client.RarityFilter is not null && ConfigContent.Client.RarityFilter.EnableRarityFilter;
        [ShowDespiteJsonIgnore]
        public bool EnableSellValueFilter => ConfigContent.Client.SellValueFilter is not null && ConfigContent.Client.SellValueFilter.EnableSellValueFilter;
        [ShowDespiteJsonIgnore]
        public bool EnableItemTypeFilter => ConfigContent.Client.ItemTypeFilter is not null && ConfigContent.Client.ItemTypeFilter.EnableItemTypeFilter;
        [ShowDespiteJsonIgnore]
        public bool EnableItemIDFilter => ConfigContent.Client.ItemIDFilter is not null && ConfigContent.Client.ItemIDFilter.EnableItemIDFilter;
    }
}
