namespace AutoFisher.Common.EntitySources
{
    public class EntitySource_GetCatchesOrCoins : EntitySource_OverfullInventory, IEntitySource_AutoFisher
    {
        public EntitySource_GetCatchesOrCoins(Player player) : base(player, "AutoFisher")
        {
        }
    }
}
