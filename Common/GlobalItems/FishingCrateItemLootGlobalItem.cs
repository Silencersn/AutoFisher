using AutoFisher.Content.Items;
using Terraria.GameContent.ItemDropRules;

namespace AutoFisher.Common.GlobalItems;

public class FishingCrateItemLootGlobalItem : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (ConfigContent.Server.Common.DisableNewItemOfThisMod)
            return;

        if (ItemID.Sets.IsFishingCrate[item.type])
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TerraBait>(), 3, 1, 2));

            if (ItemID.Sets.IsFishingCrateHardmode[item.type])
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ZenithBait>(), 3, 1, 2));
            }
        }
    }
}
