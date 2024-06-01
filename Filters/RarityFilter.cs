namespace AutoFisher.Filters
{
    public class RarityFilter : ACatchFilter<AutoFisher_RarityFilter_ClientConfig>
    {
        public override bool FitsFilter(Item item, FishingAttempt attempt)
        {
            if (item.rare is ItemRarityID.Gray) return Config.Gray;
            if (item.rare is ItemRarityID.White) return Config.White;
            if (item.rare is ItemRarityID.Blue) return Config.Blue;
            if (item.rare is ItemRarityID.Green) return Config.Green;
            if (item.rare is ItemRarityID.Orange) return Config.Orange;
            if (item.rare is ItemRarityID.LightRed) return Config.LightRed;
            if (item.rare is ItemRarityID.Pink) return Config.Pink;
            if (item.rare is ItemRarityID.LightPurple) return Config.LightPurple;
            if (item.rare is ItemRarityID.Lime) return Config.Lime;
            if (item.rare is ItemRarityID.Yellow) return Config.Yellow;
            if (item.rare is ItemRarityID.Cyan) return Config.Cyan;
            if (item.rare is ItemRarityID.Red) return Config.Red;
            if (item.rare is ItemRarityID.Purple) return Config.Purple;
            if (item.rare is ItemRarityID.Quest) return Config.Quest;
            if (item.rare is ItemRarityID.Expert) return Config.Expert;
            if (item.rare is ItemRarityID.Master) return Config.Master;
            return false;
        }
    }
}
