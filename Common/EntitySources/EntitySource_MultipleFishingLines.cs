namespace AutoFisher.Common.EntitySources;

public class EntitySource_MultipleFishingLines : AEntitySource_AutoFisher
{
    public Item? Owner { get; }

    public EntitySource_MultipleFishingLines(Item? owner = null) : base("MultipleFishingLines")
    {
        Owner = owner;
    }
}
