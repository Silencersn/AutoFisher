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

        public PromptEnableAllFilters PromptEnableAllFilters = new();

        [Header("ItemIDFilter")]
        [DefaultValue(false)]
        public bool EnableItemIDFilter;
        [DefaultValue(false)]
        public bool TurnBlockListToAllowList;
        public List<ItemDefinition> BlockList = [];
        [Header("CalculateCatches")]
        [DefaultValue(2000)]
        [Range(0, 5000)]
        [Increment(500)]
        [Slider]
        [DrawTicks]
        public int Attempts;
        [DefaultValue(true)]
        public bool CalculateImmediately;
        public List<CatchItem> CatchesInTheLakeWhereCurrentOrLastFishing = [];
    }

    public class CatchItem
    {
        private static AutoFisher_ItemIDFilter_ClientConfig Config => ConfigContent.Client.ItemIDFilter;

        private string _probability;

        public ItemDefinition Catch;
        public bool ShouldBeInBlockList;
        public string Probability
        {
            get
            {
                return _probability ??= UnknownText?.Value!;
            }
            set
            {
                if (_probability is null || _probability == UnknownText.Value)
                {
                    _probability = value;
                }
            }
        }

        public CatchItem()
        {
            Catch = new ItemDefinition(ItemID.None);
            ShouldBeInBlockList = false;
            _probability = null!;
        }

        public CatchItem(int type, int count, int totalCount)
        {
            Catch = new ItemDefinition(type);
            if (Config is null) ShouldBeInBlockList = false;
            else ShouldBeInBlockList = Config.BlockList.Contains(Catch);
            _probability = double.Clamp((double)count / totalCount, 0, 1).ToString("0.##%");
        }
    }
}
