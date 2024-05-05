using AutoFisher.Common.GlobalProjectiles;
using System.ComponentModel;

namespace AutoFisher.Configs.ClientConfigs
{
    public class AutoFisher_ItemIDFilter_ClientConfig : ModConfig, IFilterConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        bool IFilterConfig.Enable => EnableItemIDFilter;

        public bool ShouldSerializeBlockList()
        {
            var list = CatchesInTheLakeWhereCurrentOrLastFishing;
            foreach (CatchItem item in list)
            {
                if (item.ShouldBeInBlockList && !BlockList.Contains(item.Catch))
                {
                    BlockList.Add(item.Catch);
                }
                else if (!item.ShouldBeInBlockList && BlockList.Contains(item.Catch))
                {
                    BlockList.Remove(item.Catch);
                }
            }
            return true;
        }

        [Header("ItemIDFilter")]
        [DefaultValue(false)]
        public bool EnableItemIDFilter;
        public bool TurnBlockListToAllowList;
        public List<ItemDefinition> BlockList = new();
        [Header("CalculateCatches")]
        [DefaultValue(2000)]
        [Range(0, 5000)]
        [Increment(500)]
        [Slider]
        public int Attempts;
        public List<CatchItem> CatchesInTheLakeWhereCurrentOrLastFishing = new();
    }

    public class CatchItem
    {
        private static AutoFisher_ItemIDFilter_ClientConfig Config => ConfigContent.Client.ItemIDFilter;

        public ItemDefinition Catch;
        public bool ShouldBeInBlockList;

        public CatchItem()
        {
            Catch = new ItemDefinition(ItemID.None);
            if (Config is null) ShouldBeInBlockList = false;
            else ShouldBeInBlockList = Config.BlockList.Contains(Catch);
        }

        public CatchItem(int type)
        {
            Catch = new ItemDefinition(type);
            if (Config is null) ShouldBeInBlockList = false;
            else ShouldBeInBlockList = Config.BlockList.Contains(Catch);
        }
    }

    public static class CatchesCalculator
    {
        public static readonly Dictionary<int, int> catches = new();

        public static void RecalculateCatches()
        {
            var config = ConfigContent.Client.ItemIDFilter;
            var calculater = FishingCatchesCalculator.Calculater;
            catches.Clear();

            if (FishingCatchesCalculator.Calculater is not null)
            {
                for (int i = config.Attempts; i > 0; i--)
                {
                    try
                    {
                        calculater.FishingCheck();
                    }
                    catch
                    {

                    }
                }

                calculater.Kill();
                FishingCatchesCalculator.Calculater = null;
            }

            config.CatchesInTheLakeWhereCurrentOrLastFishing =
                catches.OrderByDescending(pair => pair.Key)
                .Select(pair => new CatchItem(pair.Key))
                .ToList();
        }
    }
}
