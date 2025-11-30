using AutoFisher.Common.Systems;

namespace AutoFisher.Content.InfoDisplays;

public class PondStateNumWatersInfoDisplay : AFishingInfoDisplay
{
    public bool Lava;
    public bool Honey;
    public int NumWaters;
    public int ChumCount;

    public string LavaTexture => base.Texture + "_Lava";
    public string HoneyTexture => base.Texture + "_Honey";
    public override string Texture => Lava ? LavaTexture : Honey ? HoneyTexture : base.Texture;

    public override bool Active()
    {
        if (!base.Active()) return false;
        return TryFlushPondStateInfo();
    }

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        return NumWatersText.Format(NumWaters, Lava ? LavaText : Honey ? HoneyText : WaterText);
    }

    public bool TryFlushPondStateInfo()
    {
        Projectile? bobber = BobberManager.Bobber;
        if (bobber is null) return false;
        int x = (int)(bobber.Center.X / 16f);
        int y = (int)(bobber.Center.Y / 16f);
        RF_Projectile.GetFishingPondState(x, y, out Lava, out Honey, out NumWaters, out ChumCount);
        return true;
    }
}

public class PondStateChumCountInfoDisplay : AFishingInfoDisplay
{
    public int ChumCount;

    public override bool Active()
    {
        if (!base.Active()) return false;
        return TryFlushPondStateInfo();
    }

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        return ChumCountText.Format(ChumCount);
    }

    public bool TryFlushPondStateInfo()
    {
        Projectile? bobber = BobberManager.Bobber;
        if (bobber is null) return false;
        int x = (int)(bobber.Center.X / 16f);
        int y = (int)(bobber.Center.Y / 16f);
        RF_Projectile.GetFishingPondState(x, y, out _, out _, out _, out ChumCount);
        return true;
    }
}
