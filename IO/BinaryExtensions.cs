namespace AutoFisher.IO;

public static class BinaryExtensions
{
    public static void Write(this BinaryWriter writer, ItemDefinition definition, int count)
    {
        writer.Write(definition.Mod);
        writer.Write(definition.Name);
        writer.Write(count);
    }
    public static void Write(this BinaryWriter writer, Dictionary<ItemDefinition, int> catchs)
    {
        writer.Write(catchs.Count);
        foreach (var pair in catchs)
        {
            writer.Write(pair.Key, pair.Value);
        }
    }

    public static void ReadItemCounter(this BinaryReader reader, out ItemDefinition definition, out int count)
    {
        string mod = reader.ReadString();
        string name = reader.ReadString();
        count = reader.ReadInt32();
        definition = new ItemDefinition(mod, name);
    }
    public static Dictionary<ItemDefinition, int> ReadItemCounters(this BinaryReader reader)
    {
        var result = new Dictionary<ItemDefinition, int>();
        int count = reader.ReadInt32();
        for (int i = count - 1; i >= 0; i--)
        {
            reader.ReadItemCounter(out var definition, out count);
            result[definition] = count;
        }
        return result;
    }
}
