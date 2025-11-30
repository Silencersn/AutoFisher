namespace AutoFisher.Content.InfoDisplays;

public class CatchesRecorderInfoDisplay : AFishingInfoDisplay
{
    public override bool Active()
    {
        return BobberManager.IsFishing && CatchesRecorder.TryGetLocalPlayerLastCatchedItem(out _, out _);
    }

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        CatchesRecorder.TryGetLocalPlayerLastCatchedItem(out var item, out var count);
        return $"{item!.DisplayName}: {count}";
    }
}
