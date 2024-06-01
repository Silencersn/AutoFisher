namespace AutoFisher.Common.Systems
{
    public class LocalizedTextLoader : ModSystem
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        // Infomation
        public static LocalizedText CatchInfomationText { get; set; }
        public static LocalizedText NotCatchInfomationText { get; set; }
        public static LocalizedText FilteredText { get; set; }
        public static LocalizedText AutoOpenedText { get; set; }
        public static LocalizedText AutoSoldText { get; set; }
        public static LocalizedText ConsumeBaitText { get; set; }
        public static LocalizedText AutoKilledText { get; set; }
        public static LocalizedText FishingLineBreaksText { get; set; }
        // Debug
        public static LocalizedText PromptText { get; set; }
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

        }
    }
}
