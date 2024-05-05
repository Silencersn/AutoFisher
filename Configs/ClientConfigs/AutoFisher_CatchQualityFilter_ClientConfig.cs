using System.ComponentModel;

namespace AutoFisher.Configs.ClientConfigs
{
    public class AutoFisher_CatchQualityFilter_ClientConfig : ModConfig, IFilterConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        bool IFilterConfig.Enable => EnableCatchQualityFilter;

        [Header("CatchQualityFilter")]
        [DefaultValue(false)]
        public bool EnableCatchQualityFilter;
        [DefaultValue(false)]
        public bool Common;
        [DefaultValue(false)]
        public bool Uncommon;
        [DefaultValue(false)]
        public bool Rare;
        [DefaultValue(false)]
        public bool VeryRare;
        [DefaultValue(false)]
        public bool ExtremelyRare;
    }
}
