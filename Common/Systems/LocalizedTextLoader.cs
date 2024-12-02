namespace AutoFisher.Common.Systems
{
    public class LocalizedTextLoader : ModSystem
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        // Infomation
        public static LocalizedText CatchInfomationText { get; private set; }
        public static LocalizedText NotCatchInfomationText { get; private set; }
        public static LocalizedText FilteredText { get; private set; }
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
            const string KEY_INFOMATION = "Mods.AutoFisher.Infomation.";
            CatchInfomationText = Language.GetOrRegister(KEY_INFOMATION + nameof(CatchInfomationText));
            NotCatchInfomationText = Language.GetOrRegister(KEY_INFOMATION + nameof(NotCatchInfomationText));
            FilteredText = Language.GetOrRegister(KEY_INFOMATION + nameof(FilteredText));
            AutoOpenedText = Language.GetOrRegister(KEY_INFOMATION + nameof(AutoOpenedText));
            AutoSoldText = Language.GetOrRegister(KEY_INFOMATION + nameof(AutoSoldText));
            ConsumeBaitText = Language.GetOrRegister(KEY_INFOMATION + nameof(ConsumeBaitText));
            AutoKilledText = Language.GetOrRegister(KEY_INFOMATION + nameof(AutoKilledText));
            FishingLineBreaksText = Language.GetOrRegister(KEY_INFOMATION + nameof(FishingLineBreaksText));

            const string KEY_DEBUG = "Mods.AutoFisher.Debug.";
            PromptText = Language.GetOrRegister(KEY_DEBUG + nameof(PromptText));

            const string KEY_INFODISPLAY = "Mods.AutoFisher.InfoDisplays.";
            NumWatersText = Language.GetOrRegister(KEY_INFODISPLAY + nameof(NumWatersText));
            ChumCountText = Language.GetOrRegister(KEY_INFODISPLAY + nameof(ChumCountText));
            LavaText = Language.GetText("LegacyInterface.56");
            HoneyText = Language.GetText("LegacyInterface.58");
            WaterText = Language.GetText("LegacyInterface.53");

            const string KEY_CONFIG = "Mods.AutoFisher.Config.";
            UnknownText = Language.GetOrRegister(KEY_CONFIG + nameof(UnknownText));
            NoneText = Language.GetOrRegister(KEY_CONFIG + nameof(NoneText));
        }
    }
}
