using System.ComponentModel;

namespace AutoFisher.Common.Configs.ClientConfigs
{
    public class AutoFisher_Recorder_ClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public bool ShouldSerializeClearData()
        {
            if (ClearData)
            {
                ClearData = false;
                CatchesRecorder.ClearLocalPlayerData();
            }

            return false;
        }

        [DefaultValue(false)]
        public bool ClearData = false;
        [DefaultValue(100)]
        [Range(100, 1000)]
        [Increment(100)]
        [Slider]
        public int MaxDisplayCount;
        [DefaultValue(SortBy.CountDescending)]
        public SortBy SortBy;
        [ShowDespiteJsonIgnore]
#pragma warning disable CA1822 // 将成员标记为 static
        public string Character => Main.LocalPlayer.name;
        [ShowDespiteJsonIgnore]
        public string TotalCoins
        {
            get
            {
                var coins = CatchesRecorder.GetLocalPlayerCoins();
                string result = string.Empty;
                if (coins[0] > 0) result += $"[i/s{coins[0]}:74]";
                if (coins[1] > 0) result += $"[i/s{coins[1]}:73]";
                if (coins[2] > 0) result += $"[i/s{coins[2]}:72]";
                if (coins[3] > 0) result += $"[i/s{coins[3]}:71]";
                if (result == string.Empty) result = "None";
                return result;
            }
        }
#pragma warning restore CA1822 // 将成员标记为 static
        [ShowDespiteJsonIgnore]
        public List<ItemCounter> TotalCatches
        {
            get => Sort(CatchesRecorder.GetLocalPlayerTotalCatches());
        }
        [ShowDespiteJsonIgnore]
        public List<ItemCounter> CurrentOrLastCatches
        {
            get => Sort(CatchesRecorder.GetLocalPlayerCurrentOrLastCatches());
        }

        private List<ItemCounter> Sort(Dictionary<ItemDefinition, int> dict)
        {
            var list = dict.Where(pair => pair.Key.Type is not 0)
                .Select(pair => new ItemCounter(pair.Key, pair.Value));
            if (SortBy is SortBy.CountDescending) list = list.OrderByDescending(counter => counter.Count);
            else if (SortBy is SortBy.CountAscending) list = list.OrderBy(counter => counter.Count);
            else if (SortBy is SortBy.IDDescending) list = list.OrderByDescending(counter => counter.ItemDefinition.Type);
            else if (SortBy is SortBy.IDAscending) list = list.OrderBy(counter => counter.ItemDefinition.Type);
            else if (SortBy is SortBy.NameDescending) list = list.OrderByDescending(counter => counter.ItemDefinition.DisplayName);
            else if (SortBy is SortBy.NameAscending) list = list.OrderBy(counter => counter.ItemDefinition.DisplayName);
            return ((IOrderedEnumerable<ItemCounter>)list).ThenBy(counter => counter.ItemDefinition.Type).Take(MaxDisplayCount).ToList();
        }
    }

    public enum SortBy
    {
        CountDescending,
        CountAscending,
        IDDescending,
        IDAscending,
        NameDescending,
        NameAscending
    }

    public class ItemCounter
    {
        [ShowDespiteJsonIgnore]
        public ItemDefinition ItemDefinition = new(ItemID.None);
        [ShowDespiteJsonIgnore]
        [Increment(0)]
        public int Count = 0;

        public ItemCounter()
        {

        }
        public ItemCounter(ItemDefinition itemDefinition, int count)
        {
            ItemDefinition = itemDefinition;
            Count = count;
        }
    }
}
