using AutoFisher.Common.GlobalProjectiles;

namespace AutoFisher.Common.Players
{
    public class AutoUsePotionsPlayer : ModPlayer
    {
        public static bool IsFishing => MultipleFishingLines.Bobbers.Count > 0;

        public override void PreUpdate()
        {
            if (ConfigContent.NotEnableMod) return;
            if (!IsFishing) return;
            MultipleFishingLines.RemoveNotActiveProjectiles();
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
            for (int i = 0; i < player.buffType.Length; i++)
            {
                if (player.buffType[i] == buffType)
                {
                    if (player.buffTime[i] > 0) return;
                    else break;
                }
            }
            for (int i = 0; i < 58; i++)
            {
                Item item = player.inventory[i];
                if (item.stack > 0 && item.buffType == buffType)
                {
                    player.AddBuff(item.buffType, item.buffTime);
                    item.stack--;
                    if (item.stack <= 0)
                    {
                        item.SetDefaults();
                    }
                }
            }
        }
    }
}
