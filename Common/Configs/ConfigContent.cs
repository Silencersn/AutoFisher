using System.Diagnostics;

namespace AutoFisher.Common.Configs;

public static class ConfigContent
{
    public static bool EnableMod => Server.Common.AllowPlayers.EnableMod && Client.Common.EnableMod;
    public static bool NotEnableMod => !EnableMod;

    public static bool AutoUse => Server.Common.AllowPlayers.EnableAutoUse && Client.Common.AutoUse.Enable;
    public static bool UseFishingPotions => AutoUse && Client.Common.AutoUse.AutoUseFishingPotions;
    public static bool UseCratePotions => AutoUse && Client.Common.AutoUse.AutoUseCratePotions;
    public static bool UseSonarPotions => AutoUse && Client.Common.AutoUse.AutoUseSonarPotions;
    public static bool UseAlesOrSakes => AutoUse && Client.Common.AutoUse.AutoUseAlesOrSakes;
    public static bool UseChumBuckets => AutoUse && Client.Common.AutoUse.AutoUseChumBuckets > 0;

    public static bool AutoSpawnNPC => Server.Common.AllowPlayers.EnableAutoSpawnNPC && Client.Common.AutoSpawnNPC.Enable;
    public static bool CatchNPC => AutoSpawnNPC && Client.Common.AutoSpawnNPC.AutoCatchNPC;
    public static bool KillNPC => AutoSpawnNPC && Client.Common.AutoSpawnNPC.AutoKillNPC;

    public static bool AutoFindBaits => Server.Common.AllowPlayers.EnableAutoFindBaits && Client.Common.AutoFindBaits.Enable;
    public static bool FindBaitsInVoidBag => AutoFindBaits && Client.Common.AutoFindBaits.AutoFindBaitsInVoidBag;
    public static bool FindBaitsInPiggyBank => AutoFindBaits && Client.Common.AutoFindBaits.AutoFindBaitsInPiggyBank;
    public static bool FindBaitsInSafe => AutoFindBaits && Client.Common.AutoFindBaits.AutoFindBaitsInSafe;
    public static bool FindBaitsInDefendersForge => AutoFindBaits && Client.Common.AutoFindBaits.AutoFindBaitsInDefendersForge;

    public static bool AutoOpen => Server.Common.AllowPlayers.EnableAutoOpen && Client.Common.AutoOpen.Enable;
    public static bool OpenCrates => AutoOpen && Client.Common.AutoOpen.AutoOpenCrates;
    public static bool OpenOysters => AutoOpen && Client.Common.AutoOpen.AutoOpenOysters;

    public static bool AutoSell => Server.Common.AllowPlayers.EnableAutoSell && Client.Common.AutoSell.Enable;
    public static bool SellAllCatches => AutoSell && Client.Common.AutoSell.AutoSellAllCatches;
    public static bool SellFilteredCatches => AutoSell && Client.Common.AutoSell.AutoSellFilteredCatches;
    public static bool SellUnfilteredCatches => AutoSell && Client.Common.AutoSell.AutoSellUnfilteredCatches;

    public static bool MultipleFishingLines => Server.Common.AllowPlayers.EnableMultipleFishingLines && Client.Common.MultipleFishingLines.Enable;

    public static bool EnableFilters => Server.Common.AllowPlayers.EnableFilters && Client.Common.Filters.Enable;

    public static void Modify<TConfig>(this TConfig config, Action<TConfig> action) where TConfig : ModConfig
    {
        Debug.Assert(config is not null);
        Debug.Assert(action is not null);

        action(config);
        config.SaveChanges();
    }

    public static class Client
    {
        private static AutoFisher_Common_ClientConfig? _common = null;
        private static AutoFisher_CatchQualityFilter_ClientConfig? _catchQualityFilter = null;
        private static AutoFisher_RarityFilter_ClientConfig? _rarityFilter = null;
        private static AutoFisher_SellValueFilter_ClientConfig? _sellValueFilter = null;
        private static AutoFisher_ItemTypeFilter_ClientConfig? _itemTypeFilter = null;
        private static AutoFisher_ItemIDFilter_ClientConfig? _itemIDFilter = null;
        private static AutoFisher_Recorder_ClientConfig? _recorder = null;

        public static AutoFisher_Common_ClientConfig Common
        {
            get
            {
                _common ??= ModContent.GetInstance<AutoFisher_Common_ClientConfig>();
                return _common;
            }
        }
        public static AutoFisher_CatchQualityFilter_ClientConfig CatchQualityFilter
        {
            get
            {
                _catchQualityFilter ??= ModContent.GetInstance<AutoFisher_CatchQualityFilter_ClientConfig>();
                return _catchQualityFilter;
            }
        }
        public static AutoFisher_RarityFilter_ClientConfig RarityFilter
        {
            get
            {
                _rarityFilter ??= ModContent.GetInstance<AutoFisher_RarityFilter_ClientConfig>();
                return _rarityFilter;
            }
        }
        public static AutoFisher_SellValueFilter_ClientConfig SellValueFilter
        {
            get
            {
                _sellValueFilter ??= ModContent.GetInstance<AutoFisher_SellValueFilter_ClientConfig>();
                return _sellValueFilter;
            }
        }
        public static AutoFisher_ItemTypeFilter_ClientConfig ItemTypeFilter
        {
            get
            {
                _itemTypeFilter ??= ModContent.GetInstance<AutoFisher_ItemTypeFilter_ClientConfig>();
                return _itemTypeFilter;
            }
        }
        public static AutoFisher_ItemIDFilter_ClientConfig ItemIDFilter
        {
            get
            {
                _itemIDFilter ??= ModContent.GetInstance<AutoFisher_ItemIDFilter_ClientConfig>();
                return _itemIDFilter;
            }
        }
        public static AutoFisher_Recorder_ClientConfig Recorder
        {
            get
            {
                _recorder ??= ModContent.GetInstance<AutoFisher_Recorder_ClientConfig>();
                return _recorder;
            }
        }

    }

    public static class Server
    {
        private static AutoFisher_Common_SeverConifg? _common = null;

        public static AutoFisher_Common_SeverConifg Common
        {
            get
            {
                _common ??= ModContent.GetInstance<AutoFisher_Common_SeverConifg>();
                return _common;
            }
        }
    }
}
