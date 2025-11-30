using AutoFisher.IO;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace AutoFisher.Common.Players;

public class CatchesRecorder : ModPlayer
{
    public const int Version = 2;
    public Dictionary<ItemDefinition, int> TotalCatches = [];
    public Dictionary<ItemDefinition, int> CurrentOrLastCatches = [];
    public ItemDefinition? LastCatchedItem = null;
    public long TotalCoins = 0;

    public override void SaveData(TagCompound tag)
    {
        TryCatch(() =>
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            writer.Write(TotalCatches);
            byte[] totalCatches = stream.ToArray();
            tag.Set(nameof(TotalCatches), totalCatches);
        }, nameof(SaveData) + nameof(TotalCatches));
        TryCatch(() =>
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            writer.Write(CurrentOrLastCatches);
            byte[] currentOrLastCatches = stream.ToArray();
            tag.Set(nameof(CurrentOrLastCatches), currentOrLastCatches);
        }, nameof(SaveData) + nameof(CurrentOrLastCatches));
        TryCatch(() =>
        {
            var buffer = new byte[sizeof(long) * 4];
            using var stream = new MemoryStream(buffer);
            using var writer = new BinaryWriter(stream);
            Span<int> coins = stackalloc int[4];
            AutoFisherUtils.SplitCoins(TotalCoins, coins);
            for (int i = 3; i >= 0; i--)
            {
                writer.Write((long)coins[i]);
            }
            tag.Set("Coins", buffer);
        }, nameof(SaveData) + "Coins");
    }
    public override void LoadData(TagCompound tag)
    {
        if (!tag.TryGet(nameof(Version), out int version))
            version = 1;

        if (version is 1)
            LoadDataV1(tag);
    }

    private void LoadDataV1(TagCompound tag)
    {
        if (tag.ContainsKey(nameof(TotalCatches)))
        {
            TryCatch(() =>
            {
                var totalCatches = tag.Get<byte[]>(nameof(TotalCatches));
                using var stream = new MemoryStream(totalCatches);
                using var reader = new BinaryReader(stream);
                TotalCatches = reader.ReadItemCounters();
            }, nameof(LoadData) + nameof(TotalCatches));
        }
        if (tag.ContainsKey(nameof(CurrentOrLastCatches)))
        {
            TryCatch(() =>
            {
                var currentOrLastCatches = tag.Get<byte[]>(nameof(CurrentOrLastCatches));
                using var stream = new MemoryStream(currentOrLastCatches);
                using var reader = new BinaryReader(stream);
                CurrentOrLastCatches = reader.ReadItemCounters();
            }, nameof(LoadData) + nameof(CurrentOrLastCatches));
        }
        if (tag.ContainsKey("Coins"))
        {
            TryCatch(() =>
            {
                Span<long> coins = stackalloc long[4];
                var coinsBytes = tag.Get<byte[]>("Coins");
                using var stream = new MemoryStream(coinsBytes);
                using var reader = new BinaryReader(stream);
                for (int i = 0; i < coins.Length; i++)
                {
                    coins[i] = reader.ReadInt64();
                }
                TotalCoins = coins[0] * Item.platinum + coins[1] * Item.gold + coins[2] * Item.silver + coins[3] * Item.copper;
            }, nameof(LoadData) + "Coins");
        }
    }

    public void AddCatch(int type, int stack)
    {
        var definition = new ItemDefinition(type);
        LastCatchedItem = definition;
        TotalCatches.TryGetValue(definition, out var count);
        TotalCatches[definition] = count + stack;
        CurrentOrLastCatches.TryGetValue(definition, out count);
        CurrentOrLastCatches[definition] = count + stack;
    }
    public void ClearData(bool totalCatches)
    {
        if (totalCatches)
        {
            TotalCatches.Clear();
            TotalCoins = 0;
        }
        CurrentOrLastCatches.Clear();
    }

    public static Dictionary<ItemDefinition, int> GetLocalPlayerTotalCatches()
    {
        if (Main.LocalPlayer.TryGetModPlayer(out CatchesRecorder recorder))
            return recorder.TotalCatches;
        return [];
    }
    public static Dictionary<ItemDefinition, int> GetLocalPlayerCurrentOrLastCatches()
    {
        if (Main.LocalPlayer.TryGetModPlayer(out CatchesRecorder recorder))
            return recorder.CurrentOrLastCatches;
        return [];
    }

    public static bool TryGetLocalPlayerLastCatchedItem([NotNullWhen(true)] out ItemDefinition? item, out int count)
    {
        if (Main.LocalPlayer.TryGetModPlayer(out CatchesRecorder recorder) && recorder.LastCatchedItem is not null)
        {
            item = recorder.LastCatchedItem;
            count = recorder.TotalCatches[item];
            return true;
        }

        item = null;
        count = 0;
        return false;
    }
    public static long GetLocalPlayerTotalCoins()
    {
        if (Main.LocalPlayer.TryGetModPlayer(out CatchesRecorder recorder))
            return recorder.TotalCoins;
        return 0;
    }
    public static void AddCatchToLocalPlayer(int type, int stack)
    {
        if (Main.LocalPlayer.TryGetModPlayer(out CatchesRecorder recorder))
            recorder.AddCatch(type, stack);
    }
    public static void AddCoinsToLocalPlayer(long coins)
    {
        const long CoinsMaxValue = 9999_99_99_99;

        if (coins <= 0)
            return;

        if (Main.LocalPlayer.TryGetModPlayer(out CatchesRecorder recorder))
        {
            recorder.TotalCoins += coins;
            if (recorder.TotalCoins < 0 || recorder.TotalCoins > CoinsMaxValue)
                recorder.TotalCoins = CoinsMaxValue;
        }
    }
    public static void ClearLocalPlayerData(bool totalCatches)
    {
        if (Main.LocalPlayer.TryGetModPlayer(out CatchesRecorder recorder))
            recorder.ClearData(totalCatches);
    }
}
