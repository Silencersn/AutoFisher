using Terraria.GameContent.ItemDropRules;

namespace AutoFisher.Content.Items;

public class FisherCrate : ModItem
{
    public override bool CanRightClick()
    {
        return true;
    }

    public override void ModifyItemLoot(ItemLoot itemLoot)
    {
        const int ApprenticeBaitCount = 20;

        itemLoot.Add(ItemDropRule.Common(ItemID.ReinforcedFishingPole));
        itemLoot.Add(ItemDropRule.Common(ItemID.ApprenticeBait, minimumDropped: ApprenticeBaitCount, maximumDropped: ApprenticeBaitCount));
        itemLoot.Add(ItemDropRule.Common(ItemID.AnglerHat));
        itemLoot.Add(ItemDropRule.Common(ItemID.AnglerVest));
        itemLoot.Add(ItemDropRule.Common(ItemID.AnglerPants));
        itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AnglerWhistle>()));
    }
}
