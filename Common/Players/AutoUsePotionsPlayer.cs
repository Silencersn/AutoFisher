using Terraria;

namespace AutoFisher.Common.Players;

public class AutoUsePotionsPlayer : ModPlayer
{
    public override void PreUpdate()
    {
        if (ConfigContent.NotEnableMod)
            return;

        if (!BobberManager.IsFishing)
            return;

        TryAddAllBuffAboutFishing();
    }

    private void TryAddAllBuffAboutFishing()
    {
        if (ConfigContent.UseFishingPotions && !Player.HasBuff(BuffID.Fishing))
            TryUsePotion(ItemID.FishingPotion);

        if (ConfigContent.UseFishingPotions && !Player.HasBuff(BuffID.Crate))
            TryUsePotion(ItemID.CratePotion);

        if (ConfigContent.UseFishingPotions && !Player.HasBuff(BuffID.Sonar))
            TryUsePotion(ItemID.SonarPotion);

        if (ConfigContent.UseFishingPotions && !Player.HasBuff(BuffID.Tipsy))
            TryUsePotion(ItemID.Sake);

        if (ConfigContent.UseFishingPotions && !Player.HasBuff(BuffID.Tipsy))
            TryUsePotion(ItemID.Ale);
    }

    private void TryUsePotion(int type)
    {
        var itemIndex = Player.FindItemInInventoryOrOpenVoidBag(type, out var inVoidBag);
        if (itemIndex is -1)
            return;
        var item = inVoidBag ? Player.bank4.item[itemIndex] : Player.inventory[itemIndex];
        if (ItemLoader.UseItem(item, Player) is not false)
        {
            Player.AddBuff(item.buffType, item.buffTime);
            if (ItemLoader.ConsumeItem(item, Player))
            {
                item.stack--;
                if (item.stack <= 0)
                    item.TurnToAir();
            }
        }
    }
}
