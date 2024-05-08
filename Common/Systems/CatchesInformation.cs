using AutoFisher.Common.Players;
using Terraria.Localization;

namespace AutoFisher.Common.Systems
{
    public struct CatchesInfo
    {
        public bool fishingLineBreaks;
        public bool consumedBait;
        public int bait;
        public bool catchSuccessfully;

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
        public static LocalizedText FishingLineBreaksText { get; set; }

        public static void Show(CatchesInfo info)
        {
            if (info.catchSuccessfully && !info.filtered)
            {
                CatchesRecorder.AddCatchToLocalPlayer(info.itemDrop, info.stack);
            }

            var config = ConfigContent.Client.Common.CatchesInfomation;

            if (!config.Enable) return;
            if (info.filtered && !config.ShowFilteredCatchesInfomation) return;
            if (!info.catchSuccessfully && !config.ShowNotCaughtCatchesInfomation) return;

            string infoText = config.ShowTimeInfomation ? AutoFisherUtils.GetTimeString(" ") : string.Empty;

            LocalizedText text = info.catchSuccessfully ? CatchInfomationText : NotCatchInfomationText;
            if (info.npcSpawn > 0) text = text.WithFormatArgs(NPC.GetFullnameByID(info.npcSpawn));
            else text = text.WithFormatArgs(AutoFisherUtils.GetItemIconString(info.itemDrop, info.stack));
            infoText += text;

            if (info.catchSuccessfully)
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
                            temp += AutoFisherUtils.GetItemIconString(ItemID.CopperCoin + i, info.coins[i]);
                        }
                    }
                    infoText += AutoSoldText.Format(temp);
                }
                if (info.autoKilled) infoText += AutoKilledText;
            }
            else
            {
                if (info.fishingLineBreaks) infoText += FishingLineBreaksText;
                else infoText += FilteredText;
            }

            if (info.consumedBait && config.ShowComsumedBaitInfomation) infoText += ConsumeBaitText.Format(info.bait);
            Main.NewText(infoText);
        }
    }

    public class LocalizedTextLoader : ModSystem
    {
        public override void OnModLoad()
        {
            const string key = "Mods.AutoFisher.Infomation.";
            CatchesInformation.CatchInfomationText = Language.GetOrRegister(key + nameof(CatchesInformation.CatchInfomationText));
            CatchesInformation.NotCatchInfomationText = Language.GetOrRegister(key + nameof(CatchesInformation.NotCatchInfomationText));
            CatchesInformation.FilteredText = Language.GetOrRegister(key + nameof(CatchesInformation.FilteredText));
            CatchesInformation.AutoOpenedText = Language.GetOrRegister(key + nameof(CatchesInformation.AutoOpenedText));
            CatchesInformation.AutoSoldText = Language.GetOrRegister(key + nameof(CatchesInformation.AutoSoldText));
            CatchesInformation.ConsumeBaitText = Language.GetOrRegister(key + nameof(CatchesInformation.ConsumeBaitText));
            CatchesInformation.AutoKilledText = Language.GetOrRegister(key + nameof(CatchesInformation.AutoKilledText));
            CatchesInformation.FishingLineBreaksText = Language.GetOrRegister(key + nameof(CatchesInformation.FishingLineBreaksText));
        }
    }
}
