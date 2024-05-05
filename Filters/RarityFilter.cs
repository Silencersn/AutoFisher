using AutoFisher.Configs.ClientConfigs;

namespace AutoFisher.Filters
{
    public class RarityFilter : ACatchFilter<AutoFisher_RarityFilter_ClientConfig>
    {
        public override bool FitsFilter(Item item, FishingAttempt attempt)
        {
            if (item.rare == ItemRarityID.Gray) return Config.Gray;
            if (item.rare == ItemRarityID.White) return Config.White;
            if (item.rare == ItemRarityID.Blue) return Config.Blue;
            if (item.rare == ItemRarityID.Green) return Config.Green;
            if (item.rare == ItemRarityID.Orange) return Config.Orange;
            if (item.rare == ItemRarityID.LightRed) return Config.LightRed;
            if (item.rare == ItemRarityID.Pink) return Config.Pink;
            if (item.rare == ItemRarityID.LightPurple) return Config.LightPurple;
            if (item.rare == ItemRarityID.Lime) return Config.Lime;
            if (item.rare == ItemRarityID.Yellow) return Config.Yellow;
            if (item.rare == ItemRarityID.Cyan) return Config.Cyan;
            if (item.rare == ItemRarityID.Red) return Config.Red;
            if (item.rare == ItemRarityID.Purple) return Config.Purple;
            if (item.rare == ItemRarityID.Quest) return Config.Quest;
            if (item.rare == ItemRarityID.Expert) return Config.Expert;
            if (item.rare == ItemRarityID.Master) return Config.Master;
            return false;
        }
    }
}
