using AutoFisher.Configs.ClientConfigs;

namespace AutoFisher.Filters
{
    public class SellValueFilter : ACatchFilter<AutoFisher_SellValueFilter_ClientConfig>
    {
        public override bool FitsFilter(Item item, FishingAttempt attempt)
        {
            int sellvalue = item.value / 5;
            int value = Item.buyPrice(Config.Platinum, Config.Gold, Config.Silver, Config.Copper);
            return sellvalue < value;
        }
    }
}
