namespace AutoFisher.Common.Systems
{
    public class ExceptionReporter : ModSystem
    {
        private const string KEY_MSG = "AutoFisher_Message";
        private static readonly Queue<Exception> exceptions = [];
        private static Exception? lastException;
        private static int timer;

        internal static void TryCatch(Action action, string msg)
        {
            TryCatch<Exception>(action, msg);
        }
        internal static void TryCatch<E>(Action action, string msg) where E : Exception
        {
            try
            {
                action();
            }
            catch (E ex)
            {
                ex.Data[KEY_MSG] = msg;
                if (lastException is not null)
                {
                    if (timer > 0 && lastException.ToString() == ex.ToString()) return;
                }
                exceptions.Enqueue(ex);
                lastException = ex;
                timer = 60 * 60;
            }
        }

        public override void PostUpdateWorld()
        {
            if (timer > 0) timer--;
            if (exceptions.Count is 0) return;
            Main.NewText(PromptText, Color.LightBlue);
            while (exceptions.Count > 0)
            {
                var ex = exceptions.Dequeue();
                var msg = string.Empty;

                try
                {
                    if (ex.Data.Contains(KEY_MSG)) msg = ex.Data[KEY_MSG]?.ToString() ?? string.Empty;
                    Main.NewText($"[MSG] {msg}", Color.Red);
                    Main.NewText($"{ex.GetType().FullName ?? ex.GetType().Name}: {ex.Message}", Color.Red);
                    if (ex.StackTrace is null) continue;
                    Main.NewText(string.Join('\n', ex.StackTrace.Split('\n').Where(line => line.Contains("AutoFisher"))), Color.Yellow);
                }
                catch { }
            }
        }
    }
}
