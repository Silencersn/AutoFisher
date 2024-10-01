using AutoFisher.Common.Systems;

namespace AutoFisher.Content.InfoDisplays
{
    public abstract class APondStateInfoDisplay : AFishingInfoDisplay
    {
        public static bool Lava { get; private set; }
        public static bool Honey { get; private set; }
        public static int NumWaters { get; private set; }
        public static int ChumCount { get; private set; }

        public override bool Active()
        {
            return base.Active() && Updater.TryFlushPondStateInfo();
        }

        private class Updater : ModSystem
        {
            private static int last = 0;
            private static int current = 0;

            public override void PreUpdateWorld()
            {
                current++;
            }

            public static bool TryFlushPondStateInfo()
            {
                if (last == current) return true;
                Projectile? bobber = BobberManager.Bobber;
                if (bobber is null) return false;
                int x = (int)(bobber.Center.X / 16f);
                int y = (int)(bobber.Center.Y / 16f);
                RF_Projectile.GetFishingPondState(x, y, out bool lava, out bool honey, out int numWaters, out int chumCount);
                Lava = lava;
                Honey = honey;
                NumWaters = numWaters;
                ChumCount = chumCount;
                last = current;
                return true;
            }
        }
    }

    public class PondStateNumWatersInfoDisplay : APondStateInfoDisplay
    {
        public override bool Active()
        {
            return base.Active() && !Lava && !Honey;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            return NumWatersText.Format(NumWaters, WaterText);
        }
    }

    public class PondStateNumWatersInfoDisplay_Lava : APondStateInfoDisplay
    {
        public override bool Active()
        {
            return base.Active() && Lava;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            return NumWatersText.Format(NumWaters, LavaText);
        }
    }

    public class PondStateNumWatersInfoDisplay_Honey : APondStateInfoDisplay
    {
        public override bool Active()
        {
            return base.Active() && Honey;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            return NumWatersText.Format(NumWaters, HoneyText);
        }
    }

    public class PondStateChumCountInfoDisplay : APondStateInfoDisplay
    {
        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            return ChumCountText.Format(ChumCount);
        }
    }
}
