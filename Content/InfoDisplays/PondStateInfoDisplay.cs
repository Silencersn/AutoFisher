using AutoFisher.Common.Systems;

namespace AutoFisher.Content.InfoDisplays
{
    public abstract class APondStateInfoDisplay : InfoDisplay
    {
        public override bool Active()
        {
            return BobberManager.IsFishing;
        }
    }


    public class PondStateNumWatersInfoDisplay : APondStateInfoDisplay
    {
        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            Projectile? bobber = BobberManager.Bobber;

            if (bobber is null) return string.Empty;

            int x = (int)(bobber.Center.X / 16f);
            int y = (int)(bobber.Center.Y / 16f);
            RF_Projectile.GetFishingPondState(x, y, out bool lava, out bool honey, out int numWaters, out _);
            LocalizedText liquidText = WaterText;
            if (lava) liquidText = LavaText;
            else if (honey) liquidText = HoneyText;
            return NumWatersText.Format(numWaters, liquidText);
        }
    }

    public class PondStateChumCountInfoDisplay : APondStateInfoDisplay
    {
        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            Projectile? bobber = BobberManager.Bobber;

            if (bobber is null) return string.Empty;

            int x = (int)(bobber.Center.X / 16f);
            int y = (int)(bobber.Center.Y / 16f);
            RF_Projectile.GetFishingPondState(x, y, out _, out _, out _, out int chumCount);
            return ChumCountText.Format(chumCount);
        }
    }
}
