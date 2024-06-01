namespace AutoFisher.Filters
{
    public class ItemIDFilter : ACatchFilter<AutoFisher_ItemIDFilter_ClientConfig>
    {
        public override bool FitsFilter(Item item, FishingAttempt attempt)
        {
            int type = item.type;
            foreach (ItemDefinition itemDefinition in Config.BlockList)
            {
                if (itemDefinition.Type == type)
                {
                    return !Config.TurnBlockListToAllowList;
                }
            }
            return Config.TurnBlockListToAllowList;
        }
    }
}
