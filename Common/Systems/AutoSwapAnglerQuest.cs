namespace AutoFisher.Common.Systems;

public class AutoSwapAnglerQuest : ModSystem
{
    public override void PostUpdateWorld()
    {
        if (ConfigContent.NotEnableMod) return;
        if (!ConfigContent.Server.Common.FishingQuests.ChangeAnglerQuestAfterThatIsFinished) return;
        if (!Main.anglerQuestFinished) return;

        if (Main.netMode is NetmodeID.SinglePlayer)
        {
            Main.AnglerQuestSwap();
        }
        else if (Main.netMode is NetmodeID.MultiplayerClient)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)AFMessageType.SwapAnglerQuest);
            packet.Send();
        }
    }
}
