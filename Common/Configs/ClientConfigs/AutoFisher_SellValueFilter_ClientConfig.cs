using System.ComponentModel;

namespace AutoFisher.Common.Configs.ClientConfigs;

public class AutoFisher_SellValueFilter_ClientConfig : ModConfig, IFilterConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;
    bool IFilterConfig.Enable => EnableSellValueFilter;

    public PromptEnableAllFilters PromptEnableAllFilters = new();

    [Header("SellValueFilter")]
    [DefaultValue(false)]
    public bool EnableSellValueFilter;
    [DefaultValue(0)]
    [Range(0, 99)]
    [Increment(1)]
    [Slider]
    public int Platinum;
    [DefaultValue(0)]
    [Range(0, 99)]
    [Increment(1)]
    [Slider]
    public int Gold;
    [DefaultValue(0)]
    [Range(0, 99)]
    [Increment(1)]
    [Slider]
    public int Silver;
    [DefaultValue(0)]
    [Range(0, 99)]
    [Increment(1)]
    [Slider]
    public int Copper;
    [ShowDespiteJsonIgnore]
    [Range(0, 99_99_99_99)]
    [Increment(1)]
    public int TotalValue
    {
        get
        {
            return Item.buyPrice(Platinum, Gold, Silver, Copper);
        }
        set
        {
            int[] coins = Utils.CoinsSplit(value);
            Copper = coins[0];
            Silver = coins[1];
            Gold = coins[2];
            Platinum = coins[3];
        }
    }
}
