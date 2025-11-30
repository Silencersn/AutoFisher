namespace AutoFisher.Content.InfoDisplays;

public class QuestFishInfoDisplay : InfoDisplay
{
    public override bool Active()
    {
        return NPC.AnyNPCs(NPCID.Angler);
    }

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        if (Main.anglerQuestFinished)
            displayColor = InactiveInfoTextColor;

        int type = Main.anglerQuestItemNetIDs[Main.anglerQuest];
        Item questFish = new(type);
        string result = questFish.Name;
        string? chat = ItemLoader.AnglerChat(type) ?? Language.GetTextValueWith("AnglerQuestText.Quest_" + ItemID.Search.GetName(type), Lang.CreateDialogSubstitutionObject(null));
        var split = chat?.Split("\n\n");
        if (split is null || split.Length is 0)
            return result;
        return result + split[^1];
    }
}
