using System.ComponentModel;

namespace AutoFisher.Common.Configs.ClientConfigs
{
    public class AutoFisher_RarityFilter_ClientConfig : ModConfig, IFilterConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        bool IFilterConfig.Enable => EnableRarityFilter;

        public PromptEnableAllFilters PromptEnableAllFilters = new();

        [Header("RarityFilter")]
        [DefaultValue(false)]
        public bool EnableRarityFilter;
        [DefaultValue(false)]
        public bool Gray;
        [DefaultValue(false)]
        public bool White;
        [DefaultValue(false)]
        public bool Blue;
        [DefaultValue(false)]
        public bool Green;
        [DefaultValue(false)]
        public bool Orange;
        [DefaultValue(false)]
        public bool LightRed;
        [DefaultValue(false)]
        public bool Pink;
        [DefaultValue(false)]
        public bool LightPurple;
        [DefaultValue(false)]
        public bool Lime;
        [DefaultValue(false)]
        public bool Yellow;
        [DefaultValue(false)]
        public bool Cyan;
        [DefaultValue(false)]
        public bool Red;
        [DefaultValue(false)]
        public bool Purple;
        [DefaultValue(false)]
        public bool Quest;
        [DefaultValue(false)]
        public bool Expert;
        [DefaultValue(false)]
        public bool Master;
    }
}
