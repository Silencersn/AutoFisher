namespace AutoFisher.Content.Items;

public class ZenithBait : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 5;
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.MasterBait);
        Item.bait *= 4;
        Item.value *= 4;
    }

    public override void AddRecipes()
    {
        if (ConfigContent.Server.Common.DisableNewItemOfThisMod)
            return;

        CreateRecipe()
            .AddIngredient<TerraBait>()
            .AddIngredient(ItemID.MasterBait)
            .AddIngredient(ItemID.JourneymanBait)
            .AddIngredient(ItemID.ApprenticeBait)
            .Register();
    }

    public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
    {
        itemGroup = ContentSamples.CreativeHelper.ItemGroup.Crates;
    }
}
