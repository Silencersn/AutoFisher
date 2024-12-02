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
            TryCatch(() => IL_Projectile.FishingCheck += IL_Projectile_FishingCheck, nameof(IL_Projectile_FishingCheck));
            TryCatch(() => IL_Projectile.AI_061_FishingBobber += IL_Projectile_AI_061_FishingBobber, nameof(IL_Projectile_AI_061_FishingBobber));
            TryCatch(() => IL_Player.GetItem += IL_Player_GetItem, nameof(IL_Player_GetItem));
            TryCatch(() => IL_Projectile.FishingCheck_ProbeForQuestFish += IL_Projectile_FishingCheck_ProbeForQuestFish, nameof(IL_Projectile_FishingCheck_ProbeForQuestFish));
            TryCatch(() => IL_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += IL_ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color, nameof(IL_ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color));
            TryCatch(() => IL_Main.DrawProj_DrawExtras += IL_Main_DrawProj_DrawExtras, nameof(IL_Main_DrawProj_DrawExtras));
            TryCatch(() => IL_Main.DrawProj_FishingLine += IL_Main_DrawProj_FishingLine, nameof(IL_Main_DrawProj_FishingLine));
            TryCatch(() => IL_Player.ItemSpace += IL_Player_ItemSpace, nameof(IL_Player_ItemSpace));
        }

        /// <summary>
        /// 修改钓鱼时切换物品后绘制鱼线时的鱼线颜色
        /// </summary>
        /// <param name="il"></param>
        private void IL_Main_DrawProj_FishingLine(ILContext il)
        {
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdloc2());
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate(FishingLineColor);
            }, nameof(FishingLineColor));
        }
        private static Color FishingLineColor(Color source, Projectile proj)
        {
            if (ConfigContent.NotEnableMod) return source;
            if (!ConfigContent.Server.Common.Regulation.ChangeHeldItemWhenFishing) return source;

            int type = proj.type;
            if (type is ProjectileID.BobberWooden) source = new Color(200, 200, 200, 100);
            else if (type is ProjectileID.BobberGolden) source = new Color(100, 180, 230, 100);
            else if (type is ProjectileID.BobberMechanics) source = new Color(250, 90, 70, 100);
            else if (type is ProjectileID.BobberFisherOfSouls) source = new Color(203, 190, 210, 100);
            else if (type is ProjectileID.BobberFleshcatcher) source = new Color(183, 77, 112, 100);
            else if (type is ProjectileID.BobberHotline) source = new Color(255, 226, 116, 100);
            else if (type is ProjectileID.BobberBloody) source = new Color(200, 100, 100, 100);
            else if (type is ProjectileID.BobberScarab) source = new Color(100, 100, 200, 100);
            float _ = default;
#pragma warning disable CS0618 // 类型或成员已过时
            ProjectileLoader.ModifyFishingLine(proj, ref _, ref _, ref source);
#pragma warning restore CS0618 // 类型或成员已过时
            FishingLineColor_ModifyFishingLine(proj, ref source);
            return source;
        }
        private static void FishingLineColor_ModifyFishingLine(Projectile proj, ref Color lineColor)
        {
            if (BobberManager.OwnerFishingRodOfBobbers.TryGetValue(proj, out Item? item))
            {
                ModItem modItem = item.ModItem;
                if (modItem is null) return;
                Vector2 _ = default;
                modItem.ModifyFishingLine(proj, ref _, ref lineColor);
            }
        }

        /// <summary>
        /// 钓鱼时切换物品后绘制鱼线
        /// </summary>
        /// <param name="il"></param>
        private void IL_Main_DrawProj_DrawExtras(ILContext il)
        {
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Item>(nameof(Item.holdStyle)));
                c.EmitDelegate(HoldStyle);
            }, nameof(HoldStyle));
        }
        private static int HoldStyle(int source)
        {
            if (ConfigContent.NotEnableMod) return source;
            return ConfigContent.Server.Common.Regulation.ChangeHeldItemWhenFishing ? -1 : source;
        }

        /// <summary>
        /// 在微光中钓鱼<br/>
        /// 钓鱼时可切换物品
        /// </summary>
        /// <param name="il"></param>
        private void IL_Projectile_AI_061_FishingBobber(ILContext il)
        {
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdcI4(out int value) && value is 1);
                c.EmitDelegate(KillBobber);
                c.GotoNext(MoveType.After, i => i.MatchLdcI4(out int value) && value is 1);
                c.EmitDelegate(KillBobber);
            }, nameof(KillBobber));
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Entity>(nameof(Entity.shimmerWet)));
                c.EmitDelegate(ShimmerWet);
            }, nameof(ShimmerWet));
        }
        private static bool KillBobber(bool source)
        {
            if (ConfigContent.NotEnableMod) return source;
            return !ConfigContent.Server.Common.Regulation.ChangeHeldItemWhenFishing;
        }
        private static bool ShimmerWet(bool source)
        {
            if (ConfigContent.NotEnableMod) return source;
            if (ConfigContent.Server.Common.Regulation.FishInShimmer) return false;
            return source;
        }

        /// <summary>
        /// 在物品栏中显示鱼饵数量
        /// </summary>
        /// <param name="il"></param>
        private void IL_ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color(ILContext il)
        {
            TryCatch(() =>
            {
                const byte index = 30;
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Item>(nameof(Item.fishingPole)));
                c.GotoNext(MoveType.After, i => i.MatchBlt(out _));
                c.Emit(OpCodes.Ldloc_S, index);
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate(CountBaits);
                c.Emit(OpCodes.Stloc_S, index);
            }, nameof(CountBaits));
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
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchCallvirt<Player>(nameof(Player.HasItem)));
                c.EmitDelegate(WhetherHasQuestFish);
            }, nameof(WhetherHasQuestFish));
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchCall<NPC>(nameof(NPC.AnyNPCs)));
                c.EmitDelegate(WhetherExistsAngler);
            }, nameof(WhetherExistsAngler));
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdsfld<Main>(nameof(Main.anglerQuestFinished)));
                c.EmitDelegate(IsAnglerQuestFinished);
            }, nameof(IsAnglerQuestFinished));
        }
        private static bool WhetherHasQuestFish(bool hasItem)
        {
            if (ConfigContent.NotEnableMod) return hasItem;
            return !ConfigContent.Server.Common.FishingQuests.CanCatchQuestFishWhenSameOneIsInInventory;
        }
        private static bool WhetherExistsAngler(bool anynpcs)
        {
            if (ConfigContent.NotEnableMod) return anynpcs;
            return anynpcs || ConfigContent.Server.Common.FishingQuests.CanCatchQuestFishWhenAnglerNotExists;
        }
        private static bool IsAnglerQuestFinished(bool anglerQuestFinished)
        {
            if (ConfigContent.NotEnableMod) return anglerQuestFinished;
            return !ConfigContent.Server.Common.FishingQuests.CanCatchQuestFishWhenAnglerQuestIsFinished;
        }

        /// <summary>
        /// 任务鱼可重复拾起
        /// </summary>
        /// <param name="il"></param>
        private void IL_Player_GetItem(ILContext il)
        {
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Item>(nameof(Item.uniqueStack)));
                c.EmitDelegate(IsUniqueStack);
            }, nameof(IsUniqueStack));
        }
        private void IL_Player_ItemSpace(ILContext il)
        {
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Item>(nameof(Item.uniqueStack)));
                c.EmitDelegate(IsUniqueStack);
            }, nameof(IsUniqueStack));
        }
        private static bool IsUniqueStack(bool uniqueStack)
        {
            if (ConfigContent.NotEnableMod) return uniqueStack;
            return uniqueStack && !ConfigContent.Server.Common.FishingQuests.CanPickUpQuestFishWhenSameOneIsInInventory;
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
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<Entity>(nameof(Entity.wet)));
                c.EmitDelegate(IsWet);
            }, nameof(IsWet));
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<FishingAttempt>(nameof(FishingAttempt.rolledItemDrop)));
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate(DropItem);
            }, nameof(DropItem));
            TryCatch(() =>
            {
                ILCursor c = new(il);
                c.GotoNext(MoveType.After, i => i.MatchLdfld<FishingAttempt>(nameof(FishingAttempt.rolledEnemySpawn)));
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate(SpawnNPC);
            }, nameof(SpawnNPC));
            TryCatch(() =>
            {
                ILCursor c = new(il);
                while (c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>(nameof(Player.luck))))
                {
                    c.EmitDelegate(Luck);
                }
            }, nameof(Luck));
        }
        private static bool IsWet(bool wet)
        {
            if (ConfigContent.NotEnableMod) return wet;
            if (ConfigContent.Server.Common.Regulation.FishInWater) return false;
            return wet;
        }
        private static readonly object _lockCalculater = new();
        private static int DropItem(int itemDrop, Projectile bobber, FishingAttempt attempt)
        {
            if (ConfigContent.NotEnableMod) return itemDrop;
            if (itemDrop <= 0) return itemDrop;
            if (bobber == BobberManager.Calculater)
            {
                var catches = FishingCatchesCalculator.Catches;

                lock (_lockCalculater)
                {
                    catches.TryGetValue(itemDrop, out int num);
                    catches[itemDrop] = num + 1;
                }

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
                ConfigContent.Server.Common.Regulation.BreakFishingLine &&
                !player.accFishingLine &&
                !(player.sonarPotion && info.filtered) &&
                Main.rand.NextBool(7);

            info.catchSuccessfully = !(info.fishingLineBreaks || player.sonarPotion && info.filtered);

            if (info.catchSuccessfully)
            {
                info.autoSold = DropItem_AutoSell(player, item, ref info);
                if (!info.filtered && !info.autoSold) info.autoOpened = DropItem_AutoOpen(player, itemDrop, info.stack);
                if (!info.filtered && !info.autoOpened && !info.autoSold)
                {
                    //bobber.AI_061_FishingBobber_GiveItemToPlayer(player, itemDrop);
                    AutoFisherUtils.TryGiveItemToPlayerElseDropItem(bobber, player, item, true);
                }
            }

            info.consumedBait = ConfigContent.Server.Common.Regulation.ConsumeBait;
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
        private static Item DropItem_GetCatches(int finalFishingLevel, int type, Player player)
        {
            #region Projectile.AI_061_FishingBobber_GiveItemToPlayer
            Item item = new(type);
            if (type is ItemID.BombFish)
            {
                int minValue = (finalFishingLevel / 20 + 3) / 2;
                int maxValue = (finalFishingLevel / 10 + 6) / 2;
                if (Main.rand.Next(50) < finalFishingLevel) maxValue++;
                if (Main.rand.Next(100) < finalFishingLevel) maxValue++;
                if (Main.rand.Next(150) < finalFishingLevel) maxValue++;
                if (Main.rand.Next(200) < finalFishingLevel) maxValue++;
                item.stack = Main.rand.Next(minValue, maxValue + 1);
            }
            if (type is ItemID.FrostDaggerfish)
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
            if (bobber == BobberManager.Calculater) return 0;

            Player player = Main.player[bobber.owner];

            CatchesInfo info = default;
            info.bait = attempt.playerFishingConditions.BaitItemType;
            info.fishingLineBreaks = ConfigContent.Server.Common.Regulation.BreakFishingLine && !player.accTackleBox && Main.rand.NextBool(7);
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
                    if (ConfigContent.KillNPC)
                    {
                        NPC npc = new();
                        npc.SetDefaults(npcSpawn);
                        if (npc.netID != npc.type) npc.SetDefaults(npc.netID);
                        int banner = Item.NPCtoBanner(npc.BannerID());
                        player.lastCreatureHit = banner;
                    }
                }
                else
                {
                    if (npcSpawn is NPCID.TownSlimeRed) NPC.unlockedSlimeRedSpawn = true;
                    int npcIndex = NPC.NewNPC(new EntitySource_AutoSpawnNPC(player, ConfigContent.KillNPC), point.X, point.Y, npcSpawn);
                    NPC npc = Main.npc[npcIndex];
                    if (npcSpawn is NPCID.TownSlimeRed) WorldGen.CheckAchievement_RealEstateAndTownSlimes();
                    else if (ConfigContent.KillNPC)
                    {
                        npc.playerInteraction[bobber.owner] = true;
                        int banner = Item.NPCtoBanner(npc.BannerID());
                        player.lastCreatureHit = banner;
                        npc.StrikeInstantKill();
                    }
                }
            }

            if (ConfigContent.Server.Common.Regulation.ConsumeBait)
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
        private static void DropItemAndSpawnNPC_PickAndConsumeBait(Projectile bobber, Item bait, out bool consumeBait)
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
            if (ConfigContent.Server.Common.FishingPowerInfluences.Luck) return luck;
            if (ConfigContent.Server.Common.FishingPowerInfluences.OnlyPositiveLuck) return Math.Max(luck, 0);
            return 0f;
        }
    }
}
