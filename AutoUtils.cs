using AutoFisher.Common.Systems;
using Terraria.Audio;

namespace AutoFisher
{
    public static class AutoUtils
    {
        public static Item FindBait(Player self, bool includeInventory)
        {
            Item bait = null;
            if (includeInventory)
            {
                for (int i = 54; i < 58; i++)
                {
                    if (self.inventory[i].stack > 0 && self.inventory[i].bait > 0)
                    {
                        bait = self.inventory[i];
                        return bait;
                    }
                }
                if (bait is null)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        if (self.inventory[j].stack > 0 && self.inventory[j].bait > 0)
                        {
                            bait = self.inventory[j];
                            return bait;
                        }
                    }
                }
            }
            bait = self.FindItem(IsBait, false, ConfigContent.FindBaitsInVoidBag, ConfigContent.FindBaitsInPiggyBank, ConfigContent.FindBaitsInSafe, ConfigContent.FindBaitsInDefendersForge);
            return bait;
        }
        public static bool IsBait(Item item)
        {
            return item.bait > 0;
        }
        public static int CountBait(Player self, bool includeInventory)
        {
            return self.CountItem(IsBait, includeInventory, ConfigContent.FindBaitsInVoidBag, ConfigContent.FindBaitsInPiggyBank, ConfigContent.FindBaitsInSafe, ConfigContent.FindBaitsInDefendersForge);
        }
        /// <summary>
        /// Source Code: <see cref="Player"/>.ItemCheck_CheckFishingBobber_PickAndConsumeBait
        /// </summary>
        public static void PickAndConsumeBait(Projectile bobber, out bool consumeBait)
        {
            consumeBait = false;
            Player player = Main.player[bobber.owner];
            Item bait = FindBait(player, true);
            if (bait != null)
            {
                Item baitItem = bait;
                float chance = 1f + baitItem.bait / 6f;
                if (chance < 1f)
                {
                    chance = 1f;
                }
                if (player.accTackleBox)
                {
                    chance += 1f;
                }
                if (Main.rand.NextFloat() * chance < 1f)
                {
                    consumeBait = true;
                }
                if (bobber.localAI[1] is -1f)
                {
                    consumeBait = true;
                }
                if (bobber.localAI[1] > 0f)
                {
                    Item item2 = new();
                    item2.SetDefaults((int)bobber.localAI[1]);
                    if (item2.rare < ItemRarityID.White)
                    {
                        consumeBait = false;
                    }
                }
                if (baitItem.type is ItemID.GoldWorm)
                {
                    consumeBait = Main.rand.NextBool(20);
                }
                if (baitItem.type is ItemID.TruffleWorm)
                {
                    consumeBait = true;
                }
                if (consumeBait)
                {
                    if (baitItem.type is ItemID.LadyBug || baitItem.type is ItemID.GoldLadyBug)
                    {
                        NPC.LadyBugKilled(player.Center, baitItem.type is ItemID.GoldLadyBug);
                    }
                    baitItem.stack--;
                    if (baitItem.stack <= 0)
                    {
                        baitItem.SetDefaults();
                    }
                }
            }
        }

        /// <summary>
        /// Source Code: <see cref="Projectile"/>.AI_061_FishingBobber_GiveItemToPlayer
        /// </summary>
        public static Item GetCatches_Vanilla(int finalFishingLevel, int type)
        {
            Item item = new(type);
            if (type == ItemID.BombFish)
            {
                int minValue = (finalFishingLevel / 20 + 3) / 2;
                int maxValue = (finalFishingLevel / 10 + 6) / 2;
                if (Main.rand.Next(50) < finalFishingLevel) maxValue++;
                if (Main.rand.Next(100) < finalFishingLevel) maxValue++;
                if (Main.rand.Next(150) < finalFishingLevel) maxValue++;
                if (Main.rand.Next(200) < finalFishingLevel) maxValue++;
                item.stack = Main.rand.Next(minValue, maxValue + 1);
            }
            if (type == ItemID.FrostDaggerfish)
            {
                int minValue = (finalFishingLevel / 4 + 15) / 2;
                int maxValue = (finalFishingLevel / 2 + 40) / 2;
                if (Main.rand.Next(50) < finalFishingLevel) maxValue += 6;
                if (Main.rand.Next(100) < finalFishingLevel) maxValue += 6;
                if (Main.rand.Next(150) < finalFishingLevel) maxValue += 6;
                if (Main.rand.Next(200) < finalFishingLevel) maxValue += 6;
                item.stack = Main.rand.Next(minValue, maxValue + 1);
            }
            return item;
        }

    }
}
