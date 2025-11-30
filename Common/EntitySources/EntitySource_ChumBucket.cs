namespace AutoFisher.Common.EntitySources;

public class EntitySource_ChumBucket : IEntitySource_WithStatsFromItem, IEntitySource_AutoFisher
{
    public Player Player { get; set; }

    public Item Item { get; set; }

    public string Context { get; set; }

    public EntitySource_ChumBucket(Player player, Item item, string context = "AutoFisher")
    {
        Player = player;
        Item = item;
        Context = context;
    }
}
