namespace AutoFisher.Common.Configs
{
    public interface IFilterConfig
    {
        public bool Enable { get; }
    }

    public class PromptEnableAllFilters
    {
#pragma warning disable CA1822 // 将成员标记为 static
        public bool EnableAllFilters => (ConfigContent.Client.Common?.Filters.Enable).GetValueOrDefault();
#pragma warning restore CA1822 // 将成员标记为 static
    }
}
