using AutoFisher.IO;

namespace AutoFisher.Common.Players
{
    public class CatchesRecorder : ModPlayer
    {
        public Dictionary<ItemDefinition, int> TotalCatches = [];
        public Dictionary<ItemDefinition, int> CurrentOrLastCatches = [];
        public ItemDefinition LastCatchedItem = new();
        public long[] Coins = [0, 0, 0, 0];

        public override void SaveData(TagCompound tag)
        {
            TryCatch(() =>
            {
                using MemoryStream stream = new();
                using BinaryWriter writer = new(stream);
                writer.Write(TotalCatches);
                byte[] totalCatches = stream.ToArray();
                tag.Set(nameof(TotalCatches), totalCatches);
            }, nameof(SaveData) + nameof(TotalCatches));
            TryCatch(() =>
            {
                using MemoryStream stream = new();
                using BinaryWriter writer = new(stream);
                writer.Write(CurrentOrLastCatches);
                byte[] currentOrLastCatches = stream.ToArray();
                tag.Set(nameof(CurrentOrLastCatches), currentOrLastCatches);
            }, nameof(SaveData) + nameof(CurrentOrLastCatches));
            TryCatch(() =>
            {
                using MemoryStream stream = new();
                using BinaryWriter writer = new(stream);
                for (int i = 0; i < Coins.Length; i++)
                {
                    writer.Write(Coins[i]);
                }
                byte[] coins = stream.ToArray();
                tag.Set(nameof(Coins), coins);
            }, nameof(SaveData) + nameof(Coins));
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(TotalCatches)))
            {
                TryCatch(() =>
                {
                    byte[] totalCatches = tag.Get<byte[]>(nameof(TotalCatches));
                    using MemoryStream stream = new();
                    stream.Write(totalCatches);
                    stream.Seek(0, SeekOrigin.Begin);
                    using BinaryReader reader = new(stream);
                    TotalCatches = reader.ReadItemCounters();
                }, nameof(LoadData) + nameof(TotalCatches));
            }
            if (tag.ContainsKey(nameof(CurrentOrLastCatches)))
            {
                TryCatch(() =>
                {
                    byte[] currentOrLastCatches = tag.Get<byte[]>(nameof(CurrentOrLastCatches));
                    using MemoryStream stream = new();
                    stream.Write(currentOrLastCatches);
                    stream.Seek(0, SeekOrigin.Begin);
                    using BinaryReader reader = new(stream);
                    CurrentOrLastCatches = reader.ReadItemCounters();
                }, nameof(LoadData) + nameof(CurrentOrLastCatches));
            }
            if (tag.ContainsKey(nameof(Coins)))
            {
                TryCatch(() =>
                {
                    byte[] coins = tag.Get<byte[]>(nameof(Coins));
                    using MemoryStream stream = new();
                    stream.Write(coins);
                    stream.Seek(0, SeekOrigin.Begin);
                    using BinaryReader reader = new(stream);
                    for (int i = 0; i < Coins.Length; i++)
                    {
                        Coins[i] = reader.ReadInt64();
                    }
                }, nameof(LoadData) + nameof(Coins));
            }
        }

        public void AddCatch(int type, int stack)
        {
            ItemDefinition definition = new(type);
            LastCatchedItem = definition;
            if (!TotalCatches.ContainsKey(definition)) TotalCatches[definition] = 0;
            if (!CurrentOrLastCatches.ContainsKey(definition)) CurrentOrLastCatches[definition] = 0;
            TotalCatches[definition] += stack;
            CurrentOrLastCatches[definition] += stack;
        }
        public void AddCoins(int platinum, int gold, int silver, int copper)
        {
            Coins[0] += platinum;
            Coins[1] += gold;
            Coins[2] += silver;
            Coins[3] += copper;
            for (int i = 3; i > 0; i--)
            {
                while (Coins[i] >= 100)
                {
                    Coins[i - 1]++;
                    Coins[i] -= 100;
                }
            }
        }
        public void ClearData(bool totalCatches)
        {
            if (totalCatches)
            {
                TotalCatches.Clear();
                Coins = [0, 0, 0, 0];
            }
            CurrentOrLastCatches.Clear();
        }

        public static Dictionary<ItemDefinition, int> GetLocalPlayerTotalCatches()
        {
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out CatchesRecorder recorder))
            {
                return recorder.TotalCatches;
            }
            return [];
        }
        public static Dictionary<ItemDefinition, int> GetLocalPlayerCurrentOrLastCatches()
        {
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out CatchesRecorder recorder))
            {
                return recorder.CurrentOrLastCatches;
            }
            return [];
        }
        public static ItemDefinition GetLocalPlayerLastCatchedItem(out int count)
        {
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out CatchesRecorder recorder))
            {
                recorder.TotalCatches.TryGetValue(recorder.LastCatchedItem, out count);
                return recorder.LastCatchedItem;
            }
            count = 0;
            return new();
        }
        public static long[] GetLocalPlayerCoins()
        {
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out CatchesRecorder recorder))
            {
                return recorder.Coins;
            }
            return [0, 0, 0, 0];
        }
        public static void AddCatchToLocalPlayer(int type, int stack)
        {
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out CatchesRecorder recorder))
            {
                recorder.AddCatch(type, stack);
            }
        }
        public static void AddCoinsToLocalPlayer(int platinum, int gold, int silver, int copper)
        {
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out CatchesRecorder recorder))
            {
                recorder.AddCoins(platinum, gold, silver, copper);
            }
        }
        public static void AddCoinsToLocalPlayer(int[] coins)
        {
            AddCoinsToLocalPlayer(coins[3], coins[2], coins[1], coins[0]);
        }
        public static void ClearLocalPlayerData(bool totalCatches = true)
        {
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out CatchesRecorder recorder))
            {
                recorder.ClearData(totalCatches);
            }
        }
    }
}
