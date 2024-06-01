namespace AutoFisher.Common.GlobalNPCs
{
    public class SwapAnglerQuestGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type is NPCID.Angler;
        }

        public override void AI(NPC npc)
        {
            if (ConfigContent.NotEnableMod) return;
            if (!ConfigContent.Sever.Common.FishingQuests.ChangeAnglerQuestAfterThatIsFinished) return;
            if (Main.anglerQuestFinished) Main.AnglerQuestSwap();
        }
    }
}
