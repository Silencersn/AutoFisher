namespace AutoFisher.Filters
{
    public interface ICatchFilter<out T> where T : IFilterConfig
    {
        public T Config { get; }
        public bool FitsFilter(Item item, FishingAttempt attempt);
    }

    public abstract class ACatchFilter<T> : ICatchFilter<T> where T : ModConfig, IFilterConfig
    {
        public T Config => ModContent.GetInstance<T>();
        public abstract bool FitsFilter(Item item, FishingAttempt attempt);
    }

    public static class CatchFilters
    {
        public static RarityFilter RarityFilter { get; } = new();
        public static ItemIDFilter ItemIDFilter { get; } = new();
        public static CatchQualityFilter CatchQualityFilter { get; } = new();
        public static ItemTypeFilter ItemTypeFilter { get; } = new();
        public static SellValueFilter SellValueFilter { get; } = new();

        public static readonly ICatchFilter<IFilterConfig>[] Filters =
        [
            RarityFilter,
            ItemIDFilter,
            CatchQualityFilter,
            ItemTypeFilter,
            SellValueFilter
        ];

        public static bool FitsFilter(ICatchFilter<IFilterConfig> filter, Item item, FishingAttempt attempt)
        {
            if (filter.Config is null || !filter.Config.Enable || !ConfigContent.EnableFilters) return false;
            return filter.FitsFilter(item, attempt);
        }

        public static bool FitsFilters(IEnumerable<ICatchFilter<IFilterConfig>> filters, Item item, FishingAttempt attempt)
        {
            foreach (var filter in filters)
            {
                if (FitsFilter(filter, item, attempt)) return true;
            }
            return false;
        }

        public static bool FitsFilters(Item item, FishingAttempt attempt)
        {
            return FitsFilters(Filters, item, attempt);
        }
    }
}
