namespace AutoFisher.Content.InfoDisplays;

public abstract class AFishingInfoDisplay : InfoDisplay
{
    public override bool Active()
    {
        return BobberManager.IsFishing;
    }
}
