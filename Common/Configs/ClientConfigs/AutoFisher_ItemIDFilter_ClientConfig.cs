using System.ComponentModel;

namespace AutoFisher.Common.Configs.ClientConfigs
{
    public class AutoFisher_ItemIDFilter_ClientConfig : ModConfig, IFilterConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        bool IFilterConfig.Enable => EnableItemIDFilter;

        public bool ShouldSerializeBlockList()
        {
            foreach (CatchItem item in CatchesInTheLakeWhereCurrentOrLastFishing)
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
        public List<ItemDefinition> BlockList = [];
        [Header("CalculateCatches")]
        [DefaultValue(2000)]
        [Range(0, 5000)]
        [Increment(500)]
        [Slider]
        public int Attempts;
        [DefaultValue(true)]
        public bool CalculateImmediately;
        public List<CatchItem> CatchesInTheLakeWhereCurrentOrLastFishing = [];
    }

    public class CatchItem
    {
        private static AutoFisher_ItemIDFilter_ClientConfig Config => ConfigContent.Client.ItemIDFilter;

        public ItemDefinition Catch;
        public bool ShouldBeInBlockList;

        public CatchItem()
        {
            Catch = new ItemDefinition(ItemID.None);
            ShouldBeInBlockList = false;
        }

        public CatchItem(int type)
        {
            Catch = new ItemDefinition(type);
            if (Config is null) ShouldBeInBlockList = false;
            else ShouldBeInBlockList = Config.BlockList.Contains(Catch);
        }
    }
}
