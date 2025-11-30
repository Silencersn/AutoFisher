namespace AutoFisher.Common.Systems;

public class LocalizedTextLoader : ModSystem
{
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    // Infomation
    public static LocalizedText CatchInfomationText { get; private set; }
    public static LocalizedText NotCatchInfomationText { get; private set; }
    public static LocalizedText FilteredText { get; private set; }
    public static LocalizedText FilteredSonarText { get; private set; }
    public static LocalizedText AutoOpenedText { get; private set; }
    public static LocalizedText AutoSoldText { get; private set; }
    public static LocalizedText ConsumeBaitText { get; private set; }
    public static LocalizedText AutoKilledText { get; private set; }
    public static LocalizedText FishingLineBreaksText { get; private set; }
    // Debug
    public static LocalizedText PromptText { get; private set; }
    // InfoDisplays
    public static LocalizedText NumWatersText { get; private set; }
    public static LocalizedText ChumCountText { get; private set; }
    public static LocalizedText LavaText { get; private set; }
    public static LocalizedText HoneyText { get; private set; }
    public static LocalizedText WaterText { get; private set; }
    // Config
    public static LocalizedText UnknownText { get; private set; }
    public static LocalizedText NoneText { get; private set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    public override void OnModLoad()
    {
        const string KeyInfomation = "Mods.AutoFisher.Infomation.";
        CatchInfomationText = Language.GetOrRegister(KeyInfomation + nameof(CatchInfomationText));
        NotCatchInfomationText = Language.GetOrRegister(KeyInfomation + nameof(NotCatchInfomationText));
        FilteredText = Language.GetOrRegister(KeyInfomation + nameof(FilteredText));
        FilteredSonarText = Language.GetOrRegister(KeyInfomation + nameof(FilteredSonarText));
        AutoOpenedText = Language.GetOrRegister(KeyInfomation + nameof(AutoOpenedText));
        AutoSoldText = Language.GetOrRegister(KeyInfomation + nameof(AutoSoldText));
        ConsumeBaitText = Language.GetOrRegister(KeyInfomation + nameof(ConsumeBaitText));
        AutoKilledText = Language.GetOrRegister(KeyInfomation + nameof(AutoKilledText));
        FishingLineBreaksText = Language.GetOrRegister(KeyInfomation + nameof(FishingLineBreaksText));

        const string KeyDebug = "Mods.AutoFisher.Debug.";
        PromptText = Language.GetOrRegister(KeyDebug + nameof(PromptText));

        const string KeyInfoDisplay = "Mods.AutoFisher.InfoDisplays.";
        NumWatersText = Language.GetOrRegister(KeyInfoDisplay + nameof(NumWatersText));
        ChumCountText = Language.GetOrRegister(KeyInfoDisplay + nameof(ChumCountText));
        LavaText = Language.GetText("LegacyInterface.56");
        HoneyText = Language.GetText("LegacyInterface.58");
        WaterText = Language.GetText("LegacyInterface.53");

        const string KeyConfig = "Mods.AutoFisher.Config.";
        UnknownText = Language.GetOrRegister(KeyConfig + nameof(UnknownText));
        NoneText = Language.GetOrRegister(KeyConfig + nameof(NoneText));
    }
}
