using System.ComponentModel;

namespace AutoFisher.Common.Configs.ClientConfigs
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
        public bool Enable = true;
        public bool ShowTimeInfomation = true;
        public bool ShowFilterInfomation = true;
        public bool ShowAutoOpenInfomation = true;
        public bool ShowAutoSellInfomation = true;
        public bool ShowComsumedBaitInfomation = true;
        public bool ShowFilteredCatchesInfomation = true;
        public bool ShowNotCaughtCatchesInfomation = true;
    }
    public class MultipleFishingLines
    {
        public bool Enable = false;
        [Range(1, 256)]
        [Slider]
        public int FishingLinesCount = 1;
        [Range(1, 256)]
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
        public bool Enable = false;
        public bool AutoOpenCrates = false;
        public bool AutoOpenOysters = false;
    }
    public class AutoUse
    {
        public bool Enable = false;
        public bool AutoUseFishingPotions = false;
        public bool AutoUseCratePotions = false;
        public bool AutoUseSonarPotions = false;
        public bool AutoUseAlesOrSakes = false;
        [Range(0, 3)]
        [Increment(1)]
        public int AutoUseChumBuckets = 0;
    }
    public class AutoSell
    {
        public bool Enable = false;
        public bool AutoSellAllCatches = false;
        public bool AutoSellFilteredCatches = false;
        public bool AutoSellUnfilteredCatches = false;
    }
    public class AutoSpawnNPC
    {
        private bool catchNPC = false;
        private bool killNPC = false;

        public bool Enable = false;
        public bool AutoCatchNPC
        {
            get => catchNPC;
            set
            {
                catchNPC = value;
                killNPC &= value;
            }
        }
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
        public bool Enable = true;
        public bool AutoFindBaitsInVoidBag = true;
        public bool AutoFindBaitsInPiggyBank = false;
        public bool AutoFindBaitsInSafe = false;
        public bool AutoFindBaitsInDefendersForge = false;
    }
    public class Filters
    {
        public bool Enable = false;
        [ShowDespiteJsonIgnore]
#pragma warning disable CA1822 // 将成员标记为 static
        public bool EnableCatchQualityFilter => (ConfigContent.Client.CatchQualityFilter?.EnableCatchQualityFilter).GetValueOrDefault();
        [ShowDespiteJsonIgnore]
        public bool EnableRarityFilter => (ConfigContent.Client.RarityFilter?.EnableRarityFilter).GetValueOrDefault();
        [ShowDespiteJsonIgnore]
        public bool EnableSellValueFilter => (ConfigContent.Client.SellValueFilter?.EnableSellValueFilter).GetValueOrDefault();
        [ShowDespiteJsonIgnore]
        public bool EnableItemTypeFilter => (ConfigContent.Client.ItemTypeFilter?.EnableItemTypeFilter).GetValueOrDefault();
        [ShowDespiteJsonIgnore]
        public bool EnableItemIDFilter => (ConfigContent.Client.ItemIDFilter?.EnableItemIDFilter).GetValueOrDefault();
#pragma warning restore CA1822 // 将成员标记为 static
    }
}
