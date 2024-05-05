namespace AutoFisher.Common.GlobalNPCs
{
    public class SwapAnglerQuestGlobalNPC : GlobalNPC
    {
        public override void AI(NPC npc)
        {
            if (ConfigContent.NotEnableMod) return;

            if (!ConfigContent.Sever.Common.FishingQuests.ChangeAnglerQuestAfterThatIsFinished) return;

            if (npc.type == NPCID.Angler)
            {
                if (Main.anglerQuestFinished)
                {
                    Main.AnglerQuestSwap();
                }
            }
        }
    }
}
