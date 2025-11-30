namespace AutoFisher.Common.Configs;

public interface IFilterConfig
{
    public bool Enable { get; }
}

public class PromptEnableAllFilters
{
    public bool EnableAllFilters
    {
        get => ConfigContent.Client.Common?.Filters.Enable ?? false;
    }
}
