using System.ComponentModel;
using System.Text;

namespace AutoFisher.Common.Configs.ClientConfigs;

public class AutoFisher_Recorder_ClientConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    public bool ShouldSerializeClearData()
    {
        if (ClearData)
        {
            ClearData = false;
            CatchesRecorder.ClearLocalPlayerData(true);
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
    [Dropdown]
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
            var totalCoins = CatchesRecorder.GetLocalPlayerTotalCoins();
            Span<int> coins = stackalloc int[4];
            AutoFisherUtils.SplitCoins(totalCoins, coins);

            var builder = new StringBuilder();

            for (int i = 3; i >= 0; i--)
            {
                if (coins[i] > 0)
                    builder.AppendFormat("[i/s{0}:{1}]", coins[i], ItemID.CopperCoin + i);
            }

            if (builder.Length is 0)
                return NoneText?.Value ?? string.Empty;

            return builder.ToString();
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
        var list = dict.Select(pair => new ItemCounter(pair.Key, pair.Value));

        return [.. (SortBy switch
        {
            SortBy.CountDescending => list.OrderByDescending(counter => counter.Count),
            SortBy.CountAscending => list.OrderBy(counter => counter.Count),
            SortBy.IDDescending => list.OrderByDescending(counter => counter.ItemDefinition.Type),
            SortBy.IDAscending => list.OrderBy(counter => counter.ItemDefinition.Type),
            SortBy.NameDescending => list.OrderByDescending(counter => counter.ItemDefinition.DisplayName),
            SortBy.NameAscending => list.OrderBy(counter => counter.ItemDefinition.DisplayName),
            _ => list.OrderByDescending(counter => counter.Count),
        }).ThenBy(counter => counter.ItemDefinition.Type).Take(MaxDisplayCount)];
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
