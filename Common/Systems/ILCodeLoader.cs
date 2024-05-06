using AutoFisher.Common.GlobalProjectiles;
using AutoFisher.Configs.ClientConfigs;
using AutoFisher.Filters;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Audio;
using Terraria.UI;

namespace AutoFisher.Common.Systems
{
    public class ILCodeLoader : ModSystem
    {
        public override void Load()
        {
            try
            {
                IL_Projectile.FishingCheck += IL_Projectile_FishingCheck;
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
            try
            {
                IL_Projectile.AI_061_FishingBobber += IL_Projectile_AI_061_FishingBobber;
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
            try
            {
                IL_Player.GetItem += IL_Player_GetItem;
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
            try
            {
                IL_Projectile.FishingCheck_ProbeForQuestFish += IL_Projectile_FishingCheck_ProbeForQuestFish;
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
            try
            {
                IL_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += IL_ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color;
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
        }

        /// <summary>
        /// 在微光中钓鱼
        /// </summary>
        /// <param name="il"></param>
        private void IL_Projectile_AI_061_FishingBobber(ILContext il)
        {
            try
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Entity>(nameof(Entity.shimmerWet)));
                c.EmitDelegate(ShimmerWet);
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
        }
        private static bool ShimmerWet(bool source)
        {
            if (ConfigContent.NotEnableMod) return source;
            if (ConfigContent.Sever.Common.Regulation.FishInShimmer) return false;
            return source;
        }

        /// <summary>
        /// 在物品栏中显示鱼饵数量
        /// </summary>
        /// <param name="il"></param>
        private void IL_ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color(ILContext il)
        {
            try
            {
                const byte index = 30;
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Item>(nameof(Item.fishingPole)));
                c.GotoNext(MoveType.After, i => i.MatchBlt(out ILLabel label));
                c.Emit(OpCodes.Ldloc_S, index);
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate(CountBaits);
                c.Emit(OpCodes.Stloc_S, index);
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
        }
        private static int CountBaits(int source, Player player)
        {
            if (ConfigContent.NotEnableMod) return source;
            return player.CountItem(item => item.bait > 0, true,
                ConfigContent.FindBaitsInVoidBag,
                ConfigContent.FindBaitsInPiggyBank,
                ConfigContent.FindBaitsInSafe,
                ConfigContent.FindBaitsInDefendersForge);
        }

        /// <summary>
        /// 在有任务鱼时钓起任务鱼<br/>
        /// 在渔夫不在时钓起任务鱼<br/>
        /// 在任务完成时钓起任务鱼
        /// </summary>
        /// <param name="il"></param>
        private void IL_Projectile_FishingCheck_ProbeForQuestFish(ILContext il)
        {
            try
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchCallvirt<Player>(nameof(Player.HasItem)));
                c.EmitDelegate(WhetherHasQuestFish);
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }

            try
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchCall<NPC>(nameof(NPC.AnyNPCs)));
                c.EmitDelegate(WhetherExistsAngler);
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }

            try
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdsfld<Main>(nameof(Main.anglerQuestFinished)));
                c.EmitDelegate(IsAnglerQuestFinished);
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
        }
        private static bool WhetherHasQuestFish(bool hasItem)
        {
            if (ConfigContent.NotEnableMod) return hasItem;
            return !ConfigContent.Sever.Common.FishingQuests.CanCatchQuestFishWhenSameOneIsInInventory;
        }
        private static bool WhetherExistsAngler(bool anynpcs)
        {
            if (ConfigContent.NotEnableMod) return anynpcs;
            return anynpcs || ConfigContent.Sever.Common.FishingQuests.CanCatchQuestFishWhenAnglerNotExists;
        }
        private static bool IsAnglerQuestFinished(bool anglerQuestFinished)
        {
            if (ConfigContent.NotEnableMod) return anglerQuestFinished;
            return !ConfigContent.Sever.Common.FishingQuests.CanCatchQuestFishWhenAnglerQuestIsFinished;
        }

        /// <summary>
        /// 任务鱼可重复拾起
        /// </summary>
        /// <param name="il"></param>
        private void IL_Player_GetItem(ILContext il)
        {
            try
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Item>(nameof(Item.uniqueStack)));
                c.EmitDelegate(IsUniqueStack);
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
        }
        private static bool IsUniqueStack(bool uniqueStack)
        {
            if (ConfigContent.NotEnableMod) return uniqueStack;
            return !ConfigContent.Sever.Common.FishingQuests.CanPickUpQuestFishWhenSameOneIsInInventory;
        }

        /// <summary>
        /// 自动钓鱼<br/>
        /// 自动钓怪<br/>
        /// 在玩家在液体钓鱼<br/>
        /// 不受幸运影响
        /// </summary>
        /// <param name="il"></param>
        private void IL_Projectile_FishingCheck(ILContext il)
        {
            try
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Entity>(nameof(Entity.wet)));
                c.EmitDelegate(IsWet);
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
            try
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<FishingAttempt>(nameof(FishingAttempt.rolledItemDrop)));
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate(DropItem);
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
            try
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<FishingAttempt>(nameof(FishingAttempt.rolledEnemySpawn)));
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate(SpawnNPC);
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
            try
            {
                ILCursor c = new(il);
                while (c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>(nameof(Player.luck))))
                {
                    c.EmitDelegate(Luck);
                }
            }
            catch (Exception ex)
            {
                ExceptionReporter.Add(ex);
            }
        }
        private static bool IsWet(bool wet)
        {
            if (ConfigContent.NotEnableMod) return wet;
            if (ConfigContent.Sever.Common.Regulation.FishInWater) return false;
            return wet;
        }
        private static int DropItem(int itemDrop, Projectile bobber, FishingAttempt attempt)
        {
            //Main.NewText("item: " + itemDrop);

            if (ConfigContent.NotEnableMod) return itemDrop;
            if (itemDrop <= 0) return itemDrop;
            if (bobber == FishingCatchesCalculator.Calculater)
            {
                var catches = CatchesCalculator.catches;
                catches.TryGetValue(itemDrop, out int num);
                catches[itemDrop] = num + 1;

                return 0;
            }

            Player player = Main.player[bobber.owner];

            CatchesInfo info = default;
            info.itemDrop = itemDrop;
            info.bait = attempt.playerFishingConditions.BaitItemType;

            Item item = DropItem_GetCatches(attempt.playerFishingConditions.FinalFishingLevel, itemDrop, player);
            info.stack = item.stack;

            info.filtered = CatchFilters.FitsFilters(item, attempt);
            info.fishingLineBreaks =
                ConfigContent.Sever.Common.Regulation.BreakFishingLine &&
                !player.accTackleBox &&
                !(player.sonarPotion && info.filtered) &&
                Main.rand.NextBool(7);

            info.catchSuccessfully = !(info.fishingLineBreaks || player.sonarPotion && info.filtered);


            if (info.catchSuccessfully)
            {
                if (!info.filtered) info.autoOpened = DropItem_AutoOpen(player, itemDrop, info.stack);
                if (!info.autoOpened) info.autoSold = DropItem_AutoSell(player, item, ref info);
                if (!info.filtered && !info.autoOpened && !info.autoSold)
                {
                    //bobber.AI_061_FishingBobber_GiveItemToPlayer(player, itemDrop);
                    AutoFisherUtils.TryGiveItemToPlayerElseDropItem(bobber, player, item, true);
                }
            }

            info.consumedBait = ConfigContent.Sever.Common.Regulation.ConsumeBait;
            if (info.filtered && !info.autoSold && player.sonarPotion) info.consumedBait = false;
            else bobber.ReduceRemainingChumsInPool();
            if (info.consumedBait)
            {
                //player.ItemCheck_CheckFishingBobber_PickAndConsumeBait(bobber, out _, out _);
                DropItemAndSpawnNPC_PickAndConsumeBait(bobber, attempt.playerFishingConditions.Bait, out info.consumedBait);
            }

            CatchesInformation.Show(info);

            return 0;
        }
        public static Item DropItem_GetCatches(int finalFishingLevel, int type, Player player)
        {
            #region Projectile.AI_061_FishingBobber_GiveItemToPlayer
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
            #endregion
            PlayerLoader.ModifyCaughtFish(player, item);
            ItemLoader.CaughtFishStack(item);
            return item;
        }
        private static bool DropItem_AutoOpen(Player player, int type, int stack)
        {
            if (ConfigContent.OpenCrates)
            {
                if (ItemID.Sets.IsFishingCrate[type])
                {
                    for (int i = 0; i < stack; i++)
                    {
                        player.OpenFishingCrate(type);
                    }
                    return true;
                }
            }
            if (ConfigContent.OpenOysters)
            {
                if (type == ItemID.Oyster)
                {
                    for (int i = 0; i < stack; i++)
                    {
                        player.OpenOyster(type);
                    }
                    return true;
                }
            }
            return false;
        }
        private static bool DropItem_AutoSell(Player player, Item item, ref CatchesInfo info)
        {
            if (ConfigContent.SellAllCatches || ConfigContent.SellFilteredCatches && info.filtered || ConfigContent.SellUnfilteredCatches && !info.filtered)
            {
                long value = (item.value * item.stack) / 5;
                int[] coins = Utils.CoinsSplit(value);
                info.coins = coins;

                if (player.SellItem(item))
                {
                    SoundEngine.PlaySound(SoundID.Coins);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (coins[i] <= 0) continue;
                        Item coin = new(ItemID.CopperCoin + i, coins[i]);
                        AutoFisherUtils.TryGiveItemToPlayerElseDropItem(player, player, coin, false);
                    }
                }
                item.TurnToAir();


                return true;
            }
            return false;
        }
        private static int SpawnNPC(int npcSpawn, Projectile bobber, FishingAttempt attempt)
        {
            if (ConfigContent.NotEnableMod) return npcSpawn;
            if (npcSpawn <= 0) return npcSpawn;
            if (!ConfigContent.CatchNPC) return 0;
            if (bobber == FishingCatchesCalculator.Calculater) return 0;

            Player player = Main.player[bobber.owner];

            CatchesInfo info = default;
            info.bait = attempt.playerFishingConditions.BaitItemType;
            info.fishingLineBreaks = ConfigContent.Sever.Common.Regulation.BreakFishingLine && !player.accTackleBox && Main.rand.NextBool(7);
            info.catchSuccessfully = !info.fishingLineBreaks;

            if (!info.fishingLineBreaks)
            {
                Point point = new((int)bobber.position.X, (int)bobber.position.Y);
                if (npcSpawn is NPCID.BloodNautilus)
                {
                    point.Y += 64;
                }
                if (Main.netMode is NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = AutoFisher.Instance.GetPacket();
                    packet.Write((byte)AFMessageType.AutoSpawnNPC);
                    packet.Write(point.X / 16);
                    packet.Write(point.Y / 16);
                    packet.Write(npcSpawn);
                    packet.Write(ConfigContent.KillNPC);
                    packet.Send();
                }
                else
                {
                    if (npcSpawn is NPCID.TownSlimeRed) NPC.unlockedSlimeRedSpawn = true;
                    int npc = NPC.NewNPC(new EntitySource_AutoSpawnNPC(player, ConfigContent.KillNPC), point.X, point.Y, npcSpawn);
                    if (npcSpawn is NPCID.TownSlimeRed) WorldGen.CheckAchievement_RealEstateAndTownSlimes();
                    else if (ConfigContent.KillNPC) Main.npc[npc].StrikeInstantKill();
                }
            }

            if (ConfigContent.Sever.Common.Regulation.ConsumeBait)
            {
                //player.ItemCheck_CheckFishingBobber_PickAndConsumeBait(bobber, out _, out _);
                DropItemAndSpawnNPC_PickAndConsumeBait(bobber, attempt.playerFishingConditions.Bait, out info.consumedBait);
            }

            bobber.ReduceRemainingChumsInPool();

            info.npcSpawn = npcSpawn;
            info.autoKilled = ConfigContent.KillNPC;

            CatchesInformation.Show(info);

            return 0;
        }
        /// <summary>
        /// Source Code: <see cref="Player"/>.ItemCheck_CheckFishingBobber_PickAndConsumeBait
        /// </summary>
        public static void DropItemAndSpawnNPC_PickAndConsumeBait(Projectile bobber, Item bait, out bool consumeBait)
        {
            consumeBait = false;
            Player player = Main.player[bobber.owner];

            float chance = 1f + bait.bait / 6f;
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
                Item item = new();
                item.SetDefaults((int)bobber.localAI[1]);
                Main.NewText(AutoFisherUtils.GetItemIconString(item.type, 1));
                if (item.rare < ItemRarityID.White)
                {
                    consumeBait = false;
                }
            }

            if (bait.type is ItemID.GoldWorm)
            {
                consumeBait = Main.rand.NextBool(20);
            }
            if (bait.type is ItemID.TruffleWorm)
            {
                consumeBait = true;
            }
            if (CombinedHooks.CanConsumeBait(player, bait).GetValueOrDefault(consumeBait))
            {
                if (bait.type is ItemID.LadyBug || bait.type is ItemID.GoldLadyBug)
                {
                    NPC.LadyBugKilled(player.Center, bait.type is ItemID.GoldLadyBug);
                }
                bait.stack--;
                if (bait.stack <= 0)
                {
                    bait.SetDefaults();
                }
            }

        }
        private static float Luck(float luck)
        {
            if (ConfigContent.NotEnableMod) return luck;
            if (ConfigContent.Sever.Common.FishingPowerInfluences.Luck) return luck;
            if (ConfigContent.Sever.Common.FishingPowerInfluences.OnlyPositiveLuck) return Math.Max(luck, 0);
            return 0f;
        }
    }
}
