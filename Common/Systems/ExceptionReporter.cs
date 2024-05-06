using Terraria.Localization;

namespace AutoFisher.Common.Systems
{
    public class ExceptionReporter : ModSystem
    {
        private static readonly Queue<Exception> exceptions = [];
        private static LocalizedText PromptText { get; set; }

        internal static void Add(Exception exception)
        {
            exceptions.Enqueue(exception);
        }

        public override void OnModLoad()
        {
            const string key = "Mods.AutoFisher.Debug.";
            PromptText = Language.GetOrRegister(key + nameof(PromptText));
        }

        public override void PostUpdateWorld()
        {
            if (exceptions.Count is 0) return;
            Main.NewText(PromptText, Color.LightBlue);
            while (exceptions.Count > 0)
            {
                var ex = exceptions.Dequeue();
                Main.NewText((ex.GetType().FullName ?? ex.GetType().Name) + ": " + ex.Message, Color.Red);
                Main.NewText(string.Join('\n', ex.StackTrace.Split('\n').Where(line => line.Contains("AutoFisher"))), Color.Red);
            }
        }
    }
}
