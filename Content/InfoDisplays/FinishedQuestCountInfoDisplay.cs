namespace AutoFisher.Content.InfoDisplays;

public class FinishedQuestCountInfoDisplay : InfoDisplay
{
    public override bool Active()
    {
        return true;
    }

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        return Main.LocalPlayer.anglerQuestsFinished.ToString();
    }
}
