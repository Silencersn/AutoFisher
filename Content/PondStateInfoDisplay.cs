using AutoFisher.Common.Systems;

namespace AutoFisher.Content
{
    // test
    public class PondStateInfoDisplay : InfoDisplay
    {
        public override bool Active()
        {
            return false;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            Projectile? first = BobberManager.WetBobbers.FirstOrDefault();

            if (!BobberManager.IsFishing || first is null)
            {
                displayColor = InactiveInfoTextColor;
                return "Not fishing";
            }
            else
            {
                int x = (int)(first.Center.X / 16f);
                int y = (int)(first.Center.Y / 16f);
                RF_Projectile.GetFishingPondState(x, y, out _, out _, out int numWaters, out int chumCount);
                return $"numWaters: {numWaters}\nchumCount: {chumCount}";
            }
        }
    }
}
