using AutoFisher.Common.Players;
using System.ComponentModel;

namespace AutoFisher.Configs.ClientConfigs
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
        public string Character => Main.LocalPlayer.name;
        [ShowDespiteJsonIgnore]
        public List<ItemCounter> TotalCatches
        {
            get
            {
                var list = CatchesRecorder.GetLocalPlayerTotalCatches()
                    .Select(pair => new ItemCounter(pair.Key, pair.Value));
                if (SortBy is SortBy.CountDescending) list = list.OrderByDescending(counter => counter.Count);
                else if (SortBy is SortBy.CountAscending) list = list.OrderBy(counter => counter.Count);
                else if (SortBy is SortBy.IDDescending) list = list.OrderByDescending(counter => counter.ItemDefinition.Type);
                else if (SortBy is SortBy.IDAscending) list = list.OrderBy(counter => counter.ItemDefinition.Type);
                else if (SortBy is SortBy.NameDescending) list = list.OrderByDescending(counter => counter.ItemDefinition.DisplayName);
                else if (SortBy is SortBy.NameAscending) list = list.OrderBy(counter => counter.ItemDefinition.DisplayName);
                return list.Take(MaxDisplayCount).ToList();
            }
        }

        [ShowDespiteJsonIgnore]
        public List<ItemCounter> CurrentOrLastCatches
        {
            get
            {
                var list = CatchesRecorder.GetLocalPlayerCurrentOrLastCatches()
                    .Select(pair => new ItemCounter(pair.Key, pair.Value));
                if (SortBy is SortBy.CountDescending) list = list.OrderByDescending(counter => counter.Count);
                else if (SortBy is SortBy.CountAscending) list = list.OrderBy(counter => counter.Count);
                else if (SortBy is SortBy.IDDescending) list = list.OrderByDescending(counter => counter.ItemDefinition.Type);
                else if (SortBy is SortBy.IDAscending) list = list.OrderBy(counter => counter.ItemDefinition.Type);
                else if (SortBy is SortBy.NameDescending) list = list.OrderByDescending(counter => counter.ItemDefinition.DisplayName);
                else if (SortBy is SortBy.NameAscending) list = list.OrderBy(counter => counter.ItemDefinition.DisplayName);
                return list.Take(MaxDisplayCount).ToList();
            }
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
        public ItemDefinition ItemDefinition;
        [ShowDespiteJsonIgnore]
        [Increment(0)]
        public int Count;

        public ItemCounter() : this(new ItemDefinition(ItemID.None), 0)
        {

        }
        public ItemCounter(ItemDefinition itemDefinition, int count)
        {
            ItemDefinition = itemDefinition;
            Count = count;
        }
    }
}
