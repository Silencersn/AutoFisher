namespace AutoFisher.Content.Items;

public class AnglerWhistle : ModItem
{
    public override void SetDefaults()
    {
        Item.maxStack = 1;
        Item.consumable = false;
        Item.width = 18;
        Item.height = 18;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 30;
        Item.UseSound = SoundID.Item128;
        Item.useAnimation = 30;
        Item.rare = ItemRarityID.Blue;
        Item.value = 0;
    }

    public override bool? UseItem(Player player)
    {
        Main.AnglerQuestSwap();
        return true;
    }

    public override void AddRecipes()
    {
        if (ConfigContent.Server.Common.DisableNewItemOfThisMod)
            return;

        CreateRecipe().Register();
    }
}
