using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFisher.Common.Players
{
    public class SwapAnglerQuestPlayer : ModPlayer
    {
        public override void PostUpdate()
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
}
