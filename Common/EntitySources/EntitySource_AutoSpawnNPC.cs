namespace AutoFisher.Common.EntitySources
{
    public class EntitySource_AutoSpawnNPC : EntitySource_FishedOut, IEntitySource_AutoFisher
    {
        public bool AutoKill;
        public EntitySource_AutoSpawnNPC(Player player, bool autoKill) : base(player)
        {
            AutoKill = autoKill;
        }
    }
}
