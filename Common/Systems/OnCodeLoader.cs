namespace AutoFisher.Common.Systems
{
    public class OnCodeLoader : ModSystem
    {
        public override void Load()
        {
            TryCatch(() => On_Projectile.GetFishingPondState += On_Projectile_GetFishingPondState, nameof(On_Projectile_GetFishingPondState));
            TryCatch(() => On_Player.Fishing_GetBait += On_Player_Fishing_GetBait, nameof(On_Player_Fishing_GetBait));
            TryCatch(() => On_Player.Fishing_GetPowerMultiplier += On_Player_Fishing_GetPowerMultiplier, nameof(On_Player_Fishing_GetPowerMultiplier));
            TryCatch(() => On_Projectile.FishingCheck += On_Projectile_FishingCheck, nameof(On_Projectile_FishingCheck));
            TryCatch(() => On_Item.DefaultToQuestFish += On_Item_DefaultToQuestFish, nameof(On_Item_DefaultToQuestFish));
        }

        /// <summary>
        /// 提高任务鱼的最大堆叠数
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void On_Item_DefaultToQuestFish(On_Item.orig_DefaultToQuestFish orig, Item self)
        {
            orig(self);
            if (ConfigContent.Server.Common.FishingQuests.IncreaseQuestFishMaxStack) self.maxStack = Math.Max(self.maxStack, Item.CommonMaxStack);
        }

        /// <summary>
        /// 自动使用鱼饵桶
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void On_Projectile_FishingCheck(On_Projectile.orig_FishingCheck orig, Projectile self)
        {
            TryUseChumBuckets(self);
            orig(self);
        }
        private static void TryUseChumBuckets(Projectile projectile)
        {
            if (ConfigContent.NotEnableMod) return;
            if (!ConfigContent.UseChumBuckets) return;
            if (!BobberManager.WetBobbers.Contains(projectile)) return;
            if (projectile == BobberManager.Calculater) return;

            int x = (int)(projectile.Center.X / 16f);
            int y = (int)(projectile.Center.Y / 16f);
            RF_Projectile.GetFishingPondState(x, y, out _, out _, out _, out int chumCount);

            int need = ConfigContent.Client.Common.AutoUse.AutoUseChumBuckets - chumCount;
            if (need <= 0) return;

            Player player = Main.player[projectile.owner];
            for (int i = 0; i < need; i++)
            {
                int index = player.FindItemInInventoryOrOpenVoidBag(ItemID.ChumBucket, out bool inVoidBag);
                if (index is -1) return;

                Item chumBucket = (inVoidBag ? player.bank4.item : player.inventory)[index];
                _ = Projectile.NewProjectile(new EntitySource_ChumBucket(player, chumBucket), projectile.Bottom, Vector2.UnitY * 8f, ProjectileID.ChumBucket, 0, 0, player.whoAmI);
                chumBucket.stack--;
                if (chumBucket.stack <= 0) chumBucket.TurnToAir();
            }
        }

        /// <summary>
        /// 渔力不受池塘大小影响
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="lava"></param>
        /// <param name="honey"></param>
        /// <param name="numWaters"></param>
        /// <param name="chumCount"></param>
        private void On_Projectile_GetFishingPondState(On_Projectile.orig_GetFishingPondState orig, int x, int y, out bool lava, out bool honey, out int numWaters, out int chumCount)
        {
            orig(x, y, out lava, out honey, out numWaters, out chumCount);
            if (ConfigContent.NotEnableMod) return;
            if (!ConfigContent.Server.Common.FishingPowerInfluences.LakeSize) numWaters = Math.Max(numWaters, 300);
        }

        /// <summary>
        /// 渔力不受天气、时间、月相影响
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="pole"></param>
        /// <param name="bait"></param>
        /// <returns></returns>
        private float On_Player_Fishing_GetPowerMultiplier(On_Player.orig_Fishing_GetPowerMultiplier orig, Player self, Item pole, Item bait)
        {
            if (ConfigContent.NotEnableMod)
            {
                return orig(self, pole, bait);
            }

            float levelMultipliers = 1f;
            if (ConfigContent.Server.Common.FishingPowerInfluences.Weather)
            {
                if (Main.raining)
                {
                    levelMultipliers *= 1.2f;
                }
                if (Main.cloudBGAlpha > 0f)
                {
                    levelMultipliers *= 1.1f;
                }
            }

            if (ConfigContent.Server.Common.FishingPowerInfluences.Time)
            {
                if (Main.dayTime && (Main.time < 5400.0 || Main.time > 48600.0))
                {
                    levelMultipliers *= 1.3f;
                }
                if (Main.dayTime && Main.time > 16200.0 && Main.time < 37800.0)
                {
                    levelMultipliers *= 0.8f;
                }
                if (!Main.dayTime && Main.time > 6480.0 && Main.time < 25920.0)
                {
                    levelMultipliers *= 0.8f;
                }
            }

            if (ConfigContent.Server.Common.FishingPowerInfluences.Moon)
            {
                if (Main.moonPhase == 0)
                {
                    levelMultipliers *= 1.1f;
                }
                if (Main.moonPhase == 1 || Main.moonPhase == 7)
                {
                    levelMultipliers *= 1.05f;
                }
                if (Main.moonPhase == 3 || Main.moonPhase == 5)
                {
                    levelMultipliers *= 0.95f;
                }
                if (Main.moonPhase == 4)
                {
                    levelMultipliers *= 0.9f;
                }
                if (Main.bloodMoon) // TODO: 单独配置
                {
                    levelMultipliers *= 1.1f;
                }
            }

            PlayerLoader.GetFishingLevel(self, pole, bait, ref levelMultipliers);
            if (ConfigContent.Server.Common.FishingPowerInfluences.OnlyPositiveInfluences) levelMultipliers = Math.Max(1f, levelMultipliers);
            return levelMultipliers;
        }

        /// <summary>
        /// 在虚空袋、小猪储蓄罐、保险箱、护卫熔炉中寻找鱼饵
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="bait"></param>
        private void On_Player_Fishing_GetBait(On_Player.orig_Fishing_GetBait orig, Player self, out Item? bait)
        {
            orig(self, out bait);
            if (ConfigContent.NotEnableMod) return;
            bait ??= self.FindItem(item => item.bait > 0, false,
                ConfigContent.FindBaitsInVoidBag,
                ConfigContent.FindBaitsInPiggyBank,
                ConfigContent.FindBaitsInSafe,
                ConfigContent.FindBaitsInDefendersForge);
        }
    }
}
