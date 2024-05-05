using AutoFisher.Common.GlobalProjectiles;
using AutoFisher.Configs.ClientConfigs;
using AutoFisher.Filters;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Audio;
using Terraria.UI;
using static AutoFisher.AutoUtils;
using static Terraria.ID.ContentSamples.CreativeHelper;

namespace AutoFisher.Common.Systems
{
    public class ILCodeLoader : ModSystem
    {
        public override void Load()
        {
            IL_Projectile.FishingCheck += IL_Projectile_FishingCheck;
            IL_Projectile.AI_061_FishingBobber += IL_Projectile_AI_061_FishingBobber;
            IL_Player.GetItem += IL_Player_GetItem;
            IL_Projectile.FishingCheck_ProbeForQuestFish += IL_Projectile_FishingCheck_ProbeForQuestFish;
            IL_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += IL_ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color;
        }

        /// <summary>
        /// 在微光中钓鱼
        /// </summary>
        /// <param name="il"></param>
        private void IL_Projectile_AI_061_FishingBobber(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Entity>(nameof(Entity.shimmerWet))))
            {
                c.EmitDelegate(ShimmerWet);
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
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Item>(nameof(Item.fishingPole))))
            {
                if (c.TryGotoNext(MoveType.After, i => i.MatchBlt(out ILLabel label)))
                {
                    c.Emit(OpCodes.Ldloc_S, (byte)30);
                    c.EmitDelegate(CountBaits);
                    c.Emit(OpCodes.Stloc_S, (byte)30);
                }
            }
        }
        private static int CountBaits(int source)
        {
            if (ConfigContent.NotEnableMod) return source;
            return CountBait(Main.LocalPlayer, true);
        }

        /// <summary>
        /// 在有任务鱼时钓起任务鱼<br/>
        /// 在渔夫不在时钓起任务鱼<br/>
        /// 在任务完成时钓起任务鱼
        /// </summary>
        /// <param name="il"></param>
        private void IL_Projectile_FishingCheck_ProbeForQuestFish(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchCallvirt<Player>(nameof(Player.HasItem))))
            {
                c.EmitDelegate(WhetherHasQuestFish);
            }

            c = new(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchCall<NPC>(nameof(NPC.AnyNPCs))))
            {
                c.EmitDelegate(WhetherExistsAngler);
            }

            c = new(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdsfld<Main>(nameof(Main.anglerQuestFinished))))
            {
                c.EmitDelegate(IsAnglerQuestFinished);
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
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Item>(nameof(Item.uniqueStack))))
            {
                c.EmitDelegate(IsUniqueStack);
            }
        }
        private static bool IsUniqueStack(bool uniqueStack)
        {
            if (ConfigContent.NotEnableMod) return uniqueStack;
            return !ConfigContent.Sever.Common.FishingQuests.CanPickUpQuestFishWhenSameOneIsInInventory;
        }

        public static int counterItem = 0, counterItemAll = 0, counterNPC = 0, counterNPCAll = 0; 
        /// <summary>
        /// 自动钓鱼<br/>
        /// 自动钓怪<br/>
        /// 在玩家在液体钓鱼<br/>
        /// 不受幸运影响
        /// </summary>
        /// <param name="il"></param>
        private void IL_Projectile_FishingCheck(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Entity>(nameof(Entity.wet))))
            {
                c.EmitDelegate(IsWet);
            }

            c = new(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdfld<FishingAttempt>(nameof(FishingAttempt.rolledItemDrop))))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate(DropItem);
            }

            c = new(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdfld<FishingAttempt>(nameof(FishingAttempt.rolledEnemySpawn))))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate(SpawnNPC);
            }

            c = new(il);
            while (c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>(nameof(Player.luck))))
            {
                c.EmitDelegate(Luck);
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

            CatchesInfo info = default;
            info.itemDrop = itemDrop;
            info.bait = attempt.playerFishingConditions.BaitItemType;

            Player player = Main.player[bobber.owner];

            Item item = GetCatches_Vanilla(attempt.playerFishingConditions.FinalFishingLevel, itemDrop);
            PlayerLoader.ModifyCaughtFish(player, item);
            info.stack = item.stack;

            info.filtered = CatchFilters.FitsFilters(item, attempt);
            info.fishingLineBreaks = ConfigContent.Sever.Common.Regulation.BreakFishingLine && !player.accTackleBox && Main.rand.NextBool(7);
            //if (info.filtered && player.sonarPotion) info.fishingLineBreaks = false;

            if (!info.fishingLineBreaks)
            {
                if (!info.filtered) info.autoOpened = DropItem_AutoOpen(player, itemDrop, info.stack);
                if (!info.autoOpened) info.autoSold = DropItem_AutoSell(player, item, ref info);
                if (!info.filtered && !info.autoOpened && !info.autoSold)
                {
                    AutoFisherUtils.TryGiveItemToPlayerElseDropItem(bobber, player, item, true);
                }
            }

            info.consumedBait = ConfigContent.Sever.Common.Regulation.ConsumeBait;
            if (info.filtered && !info.autoSold && player.sonarPotion) info.consumedBait = false;
            else bobber.ReduceRemainingChumsInPool();
            if (info.consumedBait) PickAndConsumeBait(bobber, out info.consumedBait);
            counterItem += info.consumedBait ? 1 : 0;
            counterItemAll++;
            CatchesInformation.ShowCatchesInfomation(info);

            return 0;
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
        private static int SpawnNPC(int npcSpawn, Projectile bobber)
        {
            //Main.NewText("npc: " + npcSpawn);

            if (ConfigContent.NotEnableMod) return npcSpawn;
            if (npcSpawn <= 0) return npcSpawn;
            if (!ConfigContent.CatchNPC) return 0;
            if (bobber == FishingCatchesCalculator.Calculater) return 0;

            Player player = Main.player[bobber.owner];

            CatchesInfo info = default;
            info.fishingLineBreaks = ConfigContent.Sever.Common.Regulation.BreakFishingLine && !player.accTackleBox && Main.rand.NextBool(7);

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
                PickAndConsumeBait(bobber, out info.consumedBait);
            }

            bobber.ReduceRemainingChumsInPool();

            info.npcSpawn = npcSpawn;
            Item bait = FindBait(player, true);
            info.bait = bait.type;// attempt.playerFishingConditions.BaitItemType;
            info.autoKilled = ConfigContent.KillNPC;
            counterNPC += info.consumedBait ? 1 : 0;
            counterNPCAll++;
            CatchesInformation.ShowCatchesInfomation(info);

            return 0;
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
