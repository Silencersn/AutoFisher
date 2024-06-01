namespace AutoFisher.Filters
{
    public class CatchQualityFilter : ACatchFilter<AutoFisher_CatchQualityFilter_ClientConfig>
    {
        public override bool FitsFilter(Item item, FishingAttempt attempt)
        {
            if (attempt.common && Config.Common) return true;
            if (attempt.uncommon && Config.Uncommon) return true;
            if (attempt.rare && Config.Rare) return true;
            if (attempt.veryrare && Config.VeryRare) return true;
            if (attempt.legendary && Config.ExtremelyRare) return true;
            return false;
        }
    }
}
