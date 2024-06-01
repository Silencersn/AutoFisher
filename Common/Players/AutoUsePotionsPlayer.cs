using Terraria;

namespace AutoFisher.Common.Players
{
    public class AutoUsePotionsPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (ConfigContent.NotEnableMod) return;
            if (!BobberManager.IsFishing) return;
            TryAddAllBuffAboutFishing();
        }

        private void TryAddAllBuffAboutFishing()
        {
            if (ConfigContent.UseFishingPotions) TryAddBuffByUsePotions(Player, BuffID.Fishing);
            if (ConfigContent.UseCratePotions) TryAddBuffByUsePotions(Player, BuffID.Crate);
            if (ConfigContent.UseSonarPotions) TryAddBuffByUsePotions(Player, BuffID.Sonar);
            if (ConfigContent.UseAlesOrSakes) TryAddBuffByUsePotions(Player, BuffID.Tipsy);
        }
        private static void TryAddBuffByUsePotions(Player player, int buffType)
        {
            if (player.HasBuff(buffType)) return;

            //new ModItem().ConsumeItem
            List<Item> traversed = [];
            while (true)
            {
                Item? potion = player.FindItem(item => item.buffType == buffType && !traversed.Contains(item), true, true, false, false, false);
                if (potion is null) return;
                traversed.Add(potion);

                if (!CombinedHooks.CanUseItem(player, potion)) continue;

                ItemLoader.UseItem(potion, player);
                player.AddBuff(potion.buffType, potion.buffTime);
                if (ItemLoader.ConsumeItem(potion, player) && potion.stack > 0)
                {
                    potion.stack--;
                }
                if (potion.stack <= 0)
                {
                    potion.SetDefaults(0);
                }
            }
        }
    }
}
