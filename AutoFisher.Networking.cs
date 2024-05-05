

namespace AutoFisher
{
    public enum AFMessageType : byte
    {
        AutoSpawnNPC
    }

    public partial class AutoFisher
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            AFMessageType msgType = (AFMessageType)reader.ReadByte();

            switch (msgType)
            {
                case AFMessageType.AutoSpawnNPC:

                    int x = reader.ReadInt32();
                    int y = reader.ReadInt32();
                    int type = reader.ReadInt32();
                    bool kill = reader.ReadBoolean();
                    if (Main.netMode != NetmodeID.Server) return;
                    if (type is NPCID.TownSlimeRed)
                    {
                        if (NPC.unlockedSlimeRedSpawn)
                        {
                            return;
                        }
                        NPC.unlockedSlimeRedSpawn = true;
                        NetMessage.TrySendData(MessageID.WorldData);
                    }
                    x *= 16;
                    y *= 16;
                    NPC npc = new();
                    npc.SetDefaults(type);
                    int netID = npc.netID;
                    int index = NPC.NewNPC(new EntitySource_AutoSpawnNPC(Main.player[whoAmI], kill), x, y, type);
                    if (netID != npc.type)
                    {
                        Main.npc[index].SetDefaults(netID);
                        NetMessage.TrySendData(MessageID.SyncNPC, number: index);
                    }
                    if (type is NPCID.TownSlimeRed) WorldGen.CheckAchievement_RealEstateAndTownSlimes();
                    else if (kill) Main.npc[index].StrikeInstantKill();
                    break;
            }
        }
    }
}
