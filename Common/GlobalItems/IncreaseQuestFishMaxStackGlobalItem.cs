namespace AutoFisher.Common.GlobalItems;

internal class IncreaseQuestFishMaxStackGlobalItem : GlobalItem
{
    public override void SetDefaults(Item entity)
    {
        if (!entity.questItem)
            return;

        if (!ConfigContent.Server.Common.FishingQuests.IncreaseQuestFishMaxStack)
            return;

        entity.maxStack = Math.Max(entity.maxStack, Item.CommonMaxStack);
    }
}
