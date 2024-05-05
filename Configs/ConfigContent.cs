using AutoFisher.Configs.ClientConfigs;
using AutoFisher.Configs.SeverConfigs;

namespace AutoFisher.Configs
{
    public static class ConfigContent
    {
        public static T GetConfigInstance<T>() where T : ModConfig
        {
            return ModContent.GetInstance<T>();
        }

        public static bool EnableMod => Sever.Common.AllowPlayers.EnableMod && Client.Common.EnableMod;
        public static bool NotEnableMod => !EnableMod;

        public static bool AutoUse => Sever.Common.AllowPlayers.EnableAutoUse && Client.Common.AutoUse.Enable;
        public static bool UseFishingPotions => AutoUse && Client.Common.AutoUse.AutoUseFishingPotions;
        public static bool UseCratePotions => AutoUse && Client.Common.AutoUse.AutoUseCratePotions;
        public static bool UseSonarPotions => AutoUse && Client.Common.AutoUse.AutoUseSonarPotions;
        public static bool UseAlesOrSakes => AutoUse && Client.Common.AutoUse.AutoUseAlesOrSakes;
        public static bool UseChumBuckets => AutoUse && Client.Common.AutoUse.AutoUseChumBuckets > 0;

        public static bool AutoSpawnNPC => Sever.Common.AllowPlayers.EnableAutoSpawnNPC && Client.Common.AutoSpawnNPC.Enable;
        public static bool CatchNPC => AutoSpawnNPC && Client.Common.AutoSpawnNPC.AutoCatchNPC;
        public static bool KillNPC => AutoSpawnNPC && Client.Common.AutoSpawnNPC.AutoKillNPC;

        public static bool AutoFindBaits => Sever.Common.AllowPlayers.EnableAutoFindBaits && Client.Common.AutoFindBaits.Enable;
        public static bool FindBaitsInVoidBag => AutoFindBaits && Client.Common.AutoFindBaits.AutoFindBaitsInVoidBag;
        public static bool FindBaitsInPiggyBank => AutoFindBaits && Client.Common.AutoFindBaits.AutoFindBaitsInPiggyBank;
        public static bool FindBaitsInSafe => AutoFindBaits && Client.Common.AutoFindBaits.AutoFindBaitsInSafe;
        public static bool FindBaitsInDefendersForge => AutoFindBaits && Client.Common.AutoFindBaits.AutoFindBaitsInDefendersForge;

        public static bool AutoOpen => Sever.Common.AllowPlayers.EnableAutoOpen && Client.Common.AutoOpen.Enable;
        public static bool OpenCrates => AutoOpen && Client.Common.AutoOpen.AutoOpenCrates;
        public static bool OpenOysters => AutoOpen && Client.Common.AutoOpen.AutoOpenOysters;

        public static bool AutoSell => Sever.Common.AllowPlayers.EnableAutoSell && Client.Common.AutoSell.Enable;
        public static bool SellAllCatches => AutoSell && Client.Common.AutoSell.AutoSellAllCatches;
        public static bool SellFilteredCatches => AutoSell && Client.Common.AutoSell.AutoSellFilteredCatches;
        public static bool SellUnfilteredCatches => AutoSell && Client.Common.AutoSell.AutoSellUnfilteredCatches;

        public static bool MultipleFishingLines => Sever.Common.AllowPlayers.EnableMultipleFishingLines && Client.Common.MultipleFishingLines.Enable;

        public static bool EnableFilters => Sever.Common.AllowPlayers.EnableFilters && Client.Common.Filters.Enable;

        public static class Client
        {
            public static AutoFisher_Common_ClientConfig Common => GetConfigInstance<AutoFisher_Common_ClientConfig>();
            public static AutoFisher_CatchQualityFilter_ClientConfig CatchQualityFilter => GetConfigInstance<AutoFisher_CatchQualityFilter_ClientConfig>();
            public static AutoFisher_RarityFilter_ClientConfig RarityFilter => GetConfigInstance<AutoFisher_RarityFilter_ClientConfig>();
            public static AutoFisher_SellValueFilter_ClientConfig SellValueFilter => GetConfigInstance<AutoFisher_SellValueFilter_ClientConfig>();
            public static AutoFisher_ItemTypeFilter_ClientConfig ItemTypeFilter => GetConfigInstance<AutoFisher_ItemTypeFilter_ClientConfig>();
            public static AutoFisher_ItemIDFilter_ClientConfig ItemIDFilter => GetConfigInstance<AutoFisher_ItemIDFilter_ClientConfig>();
            public static AutoFisher_Recorder_ClientConfig Recorder => GetConfigInstance<AutoFisher_Recorder_ClientConfig>();
        }

        public static class Sever
        {
            public static AutoFisher_Common_SeverConifg Common => GetConfigInstance<AutoFisher_Common_SeverConifg>();
        }
    }
}
