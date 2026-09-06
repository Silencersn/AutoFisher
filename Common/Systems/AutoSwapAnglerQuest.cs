namespace AutoFisher.Common.Systems;

public class AutoSwapAnglerQuest : ModSystem
{
    public override void PostUpdateWorld()
    {
        if (ConfigContent.NotEnableMod) return;
        if (!ConfigContent.Server.Common.FishingQuests.ChangeAnglerQuestAfterThatIsFinished) return;

        if (Main.netMode is NetmodeID.Server)
        {
            var fishedCount = Main.anglerWhoFinishedToday.Count;
            foreach (var _ in Main.ActivePlayers)
                fishedCount--;
            if (fishedCount >= 0)
                Main.AnglerQuestSwap();
        }
        else // if (Main.netMode is NetmodeID.SinglePlayer)
        {
            if (Main.anglerQuestFinished)
                Main.AnglerQuestSwap();
        }
    }
}
