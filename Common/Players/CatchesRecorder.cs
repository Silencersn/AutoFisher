using AutoFisher.IO;

namespace AutoFisher.Common.Players
{
    public class CatchesRecorder : ModPlayer
    {
        public Dictionary<ItemDefinition, int> TotalCatches = new();
        public Dictionary<ItemDefinition, int> CurrentOrLastCatches = new();

        public override void SaveData(TagCompound tag)
        {
            try
            {
                using MemoryStream stream = new();
                using BinaryWriter writer = new(stream);
                writer.Write(TotalCatches);
                byte[] totalCatches = stream.ToArray();
                tag.Set(nameof(TotalCatches), totalCatches);
            }
            catch { }
            try
            {
                using MemoryStream stream = new();
                using BinaryWriter writer = new(stream);
                writer.Write(CurrentOrLastCatches);
                byte[] currentOrLastCatches = stream.ToArray();
                tag.Set(nameof(CurrentOrLastCatches), currentOrLastCatches);
            }
            catch { }
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(TotalCatches)))
            {
                try
                {
                    using MemoryStream stream = new();
                    byte[] totalCatches = tag.Get<byte[]>(nameof(TotalCatches));
                    stream.Write(totalCatches);
                    stream.Seek(0, SeekOrigin.Begin);
                    using BinaryReader reader = new(stream);
                    TotalCatches = reader.ReadItemCounters();
                }
                catch { }
            }
            if (tag.ContainsKey(nameof(CurrentOrLastCatches)))
            {
                try
                {
                    using MemoryStream stream = new();
                    byte[] currentOrLastCatches = tag.Get<byte[]>(nameof(CurrentOrLastCatches));
                    stream.Write(currentOrLastCatches);
                    stream.Seek(0, SeekOrigin.Begin);
                    using BinaryReader reader = new(stream);
                    CurrentOrLastCatches = reader.ReadItemCounters();
                }
                catch { }
            }
        }

        public void AddCatch(int type, int stack)
        {
            ItemDefinition definition = new(type);
            if (!TotalCatches.ContainsKey(definition)) TotalCatches[definition] = 0;
            if (!CurrentOrLastCatches.ContainsKey(definition)) CurrentOrLastCatches[definition] = 0;
            TotalCatches[definition] += stack;
            CurrentOrLastCatches[definition] += stack;
        }
        public void ClearData(bool totalCatches)
        {
            if (totalCatches) TotalCatches.Clear();
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
        public static void AddCatchToLocalPlayer(int type, int stack)
        {
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out CatchesRecorder recorder))
            {
                recorder.AddCatch(type, stack);
            }
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
