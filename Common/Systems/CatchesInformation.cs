using System.Text;

namespace AutoFisher.Common.Systems;

public struct CatchesInfo
{
    public bool fishingLineBreaks;
    public bool consumedBait;
    public int bait;
    /// <summary>
    /// 仅当【鱼线断裂】或【在有声纳药水Buff的情况过被过滤】时为 False
    /// </summary>
    public bool catchSuccessfully;

    public bool filtered;
    public bool autoOpened;
    public bool autoSold;

    public int itemDrop;
    public int stack;

    public long coins;

    public int npcSpawn;
    public bool autoKilled;
}

public static class CatchesInformation
{
    private static readonly StringBuilder _builder = new();
    private static readonly StringBuilder _coinBuilder = new();

    public static void Show(CatchesInfo info)
    {
        if (info.catchSuccessfully)
        {
            CatchesRecorder.AddCatchToLocalPlayer(info.itemDrop, info.stack);

            if (info.autoSold)
                CatchesRecorder.AddCoinsToLocalPlayer(info.coins);
        }

        var config = ConfigContent.Client.Common.CatchesInfomation;

        if (!config.Enable)
            return;

        if (info.filtered && !config.ShowFilteredCatchesInfomation)
            return;

        if (!info.catchSuccessfully && !config.ShowNotCaughtCatchesInfomation)
            return;

        _builder.Clear();
        if (config.ShowTimeInfomation)
        {
            AutoFisherUtils.GetDayTimeAs24HoursMinutesSeconds(out var hours, out var minutes, out var seconds);
            _builder.AppendFormat("[{0:D2}:{1:D2}:{2:D2}] ", hours, minutes, seconds);
        }

        var catchText = info.catchSuccessfully ? CatchInfomationText : NotCatchInfomationText;
        var catchObj = info.npcSpawn > 0 ? NPC.GetFullnameByID(info.npcSpawn) : AutoFisherUtils.GetItemIconString(info.itemDrop, info.stack);
        _builder.AppendFormat(catchText.Value, catchObj);

        if (info.catchSuccessfully)
        {
            if (info.filtered && config.ShowFilterInfomation)
                _builder.Append(FilteredText.Value);

            if (info.autoOpened && config.ShowAutoOpenInfomation)
                _builder.Append(AutoOpenedText.Value);

            if (info.autoSold && config.ShowAutoSellInfomation)
            {
                _coinBuilder.Clear();
                Span<int> coins = stackalloc int[4];
                AutoFisherUtils.SplitCoins(info.coins, coins);
                for (int i = 3; i >= 0; i--)
                {
                    if (coins[i] > 0)
                        _coinBuilder.AppendFormat("[i/s{0}:{1}]", coins[i], ItemID.CopperCoin + i);
                }
                _builder.AppendFormat(AutoSoldText.Value, _coinBuilder.ToString());
            }

            if (info.autoKilled)
                _builder.Append(AutoKilledText.Value);
        }
        else
        {
            if (info.fishingLineBreaks)
                _builder.Append(FishingLineBreaksText.Value);
            else
                _builder.Append(FilteredSonarText.Value);
        }

        if (info.consumedBait && config.ShowComsumedBaitInfomation)
            _builder.AppendFormat(ConsumeBaitText.Value, info.bait);

        Main.NewText(_builder.ToString());
    }
}
