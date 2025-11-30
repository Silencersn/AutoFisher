namespace AutoFisher.Content.Items;

public class TerraBait : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 5;
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.MasterBait);
        Item.bait *= 2;
        Item.value *= 2;
    }

    public override void AddRecipes()
    {
        if (ConfigContent.Server.Common.DisableNewItemOfThisMod)
            return;

        CreateRecipe().AddIngredient(ItemID.MasterBait, 2).Register();
        Recipe.Create(ItemID.MasterBait, 3).AddIngredient(ItemID.JourneymanBait, 5).Register();
        Recipe.Create(ItemID.JourneymanBait).AddIngredient(ItemID.ApprenticeBait, 2).Register();
    }
}
