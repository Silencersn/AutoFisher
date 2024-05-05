using AutoFisher.Common.Players;
using Terraria.Localization;

namespace AutoFisher.Common.Systems
{
    public struct CatchesInfo
    {
        public bool fishingLineBreaks;
        public bool consumedBait;
        public int bait;

        public bool filtered;
        public bool autoOpened;
        public bool autoSold;
        
        public int itemDrop;
        public int stack;
        
        public int[] coins;

        public int npcSpawn;
        public bool autoKilled;
    }

    public static class CatchesInformation
    {
        public static LocalizedText CatchInfomationText { get; set; }
        public static LocalizedText NotCatchInfomationText { get; set; }
        public static LocalizedText FilteredText { get; set; }
        public static LocalizedText AutoOpenedText { get; set; }
        public static LocalizedText AutoSoldText { get; set; }
        public static LocalizedText ConsumeBaitText { get; set; }
        public static LocalizedText AutoKilledText { get; set; }

        public static void ShowCatchesInfomation(CatchesInfo info)
        {
            if (!info.fishingLineBreaks && !info.filtered)
            {
                CatchesRecorder.AddCatchToLocalPlayer(info.itemDrop, info.stack);
            }

            var config = ConfigContent.Client.Common.CatchesInfomation;

            if (!config.Enable) return;
            if (info.filtered && !config.ShowFilteredCatchesInfomation) return;
            if (info.fishingLineBreaks && !config.ShowNotCaughtCatchesInfomation) return;

            float hours = Utils.GetDayTimeAs24FloatStartingFromMidnight();
            float minutes = (hours - (int)hours) * 60;
            float seconds = (minutes - (int)minutes) * 60;

            static string format(float value)
            {
                return ((int)value).ToString().PadLeft(2, '0');
            }

            LocalizedText text = info.fishingLineBreaks ? NotCatchInfomationText : CatchInfomationText;
            string itemDrop = $"[i/s{info.stack}:{info.itemDrop}]";
            string infoText = config.ShowTimeInfomation ? string.Format("[{0}:{1}:{2}] ", format(hours), format(minutes), format(seconds)) : "";
            
            if (info.npcSpawn > 0) infoText += text.Format(NPC.GetFullnameByID(info.npcSpawn));
            else infoText += text.Format(itemDrop);

            if (!info.fishingLineBreaks)
            {
                if (info.filtered && config.ShowFilterInfomation) infoText += FilteredText;
                if (info.autoOpened && config.ShowAutoOpenInfomation) infoText += AutoOpenedText;
                if (info.autoSold && config.ShowAutoSellInfomation)
                {
                    string temp = string.Empty;
                    for (int i = 3; i >= 0; i--)
                    {
                        if (info.coins[i] > 0)
                        {
                            temp += $"[i/s{info.coins[i]}:{ItemID.CopperCoin + i}]";
                        }
                    }
                    infoText += AutoSoldText.Format(temp);
                }
                if (info.autoKilled) infoText += AutoKilledText;
            }
            if (info.consumedBait && config.ShowComsumedBaitInfomation) infoText += ConsumeBaitText.Format(info.bait);
            Main.NewText(infoText);
        }
    }

    public class LocalizedTextLoader : ModSystem
    {
        public override void OnModLoad()
        {
            string key = "Mods.AutoFisher.Infomation.";
            CatchesInformation.CatchInfomationText = Language.GetOrRegister(key + nameof(CatchesInformation.CatchInfomationText));
            CatchesInformation.NotCatchInfomationText = Language.GetOrRegister(key + nameof(CatchesInformation.NotCatchInfomationText));
            CatchesInformation.FilteredText = Language.GetOrRegister(key + nameof(CatchesInformation.FilteredText));
            CatchesInformation.AutoOpenedText = Language.GetOrRegister(key + nameof(CatchesInformation.AutoOpenedText));
            CatchesInformation.AutoSoldText = Language.GetOrRegister(key + nameof(CatchesInformation.AutoSoldText));
            CatchesInformation.ConsumeBaitText = Language.GetOrRegister(key + nameof(CatchesInformation.ConsumeBaitText));
            CatchesInformation.AutoKilledText = Language.GetOrRegister(key + nameof(CatchesInformation.AutoKilledText));
        }
    }
}
