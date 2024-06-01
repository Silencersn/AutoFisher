using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFisher.Common.GlobalProjectiles
{
    public class FishingCatchesCalculator : GlobalProjectile
    {
        private static bool NeedToRecalculate = false;
        private static CancellationTokenSource? cts = null;

        public static readonly Dictionary<int, int> Catches = [];

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.owner != Main.myPlayer) return;
            if (ConfigContent.NotEnableMod) return;
            if (source is AEntitySource_AutoFisher) return;

            NeedToRecalculate = true;
        }

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.bobber;
        }

        public override void AI(Projectile projectile)
        {
            if (!NeedToRecalculate) return;
            if (!BobberManager.WetBobbers.Contains(projectile)) return;
            NeedToRecalculate = false;

            var calculater = OnSpawn_CreateCalculater(projectile);

            TryCatch(() =>
            {
                if (ConfigContent.Client.ItemIDFilter.CalculateImmediately)
                {
                    RecalculateCatches(calculater);
                }
                else
                {
                    RecalculateCatchesAsync(calculater);
                }
            }, nameof(RecalculateCatches));
        }

        private static Projectile OnSpawn_CreateCalculater(Projectile bobber)
        {
            int calculaterIndex = Projectile.NewProjectile(new EntitySource_CalculateCatches(), bobber.Center, bobber.velocity, bobber.type, bobber.damage, bobber.knockBack, bobber.owner);
            Projectile calculater = Main.projectile[calculaterIndex];
            return calculater;
        }

        public static void RecalculateCatches(Projectile calculater)
        {
            var config = ConfigContent.Client.ItemIDFilter;
            Catches.Clear();

            for (int i = config.Attempts; i > 0 && calculater.active && calculater.wet; i--)
            {
                TryCatch(calculater.FishingCheck, nameof(calculater.FishingCheck));
            }

            RefreshConfig();
            calculater.Kill();
        }

        public static async void RecalculateCatchesAsync(Projectile calculater)
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            await Task.Run(() =>
            {
                var config = ConfigContent.Client.ItemIDFilter;
                Catches.Clear();

                for (int i = config.Attempts; i > 0 && !token.IsCancellationRequested && calculater.active && calculater.wet; i--)
                {
                    TryCatch(calculater.FishingCheck, nameof(calculater));
                    if (i % 500 is 1) RefreshConfig();
                }

                RefreshConfig();
                calculater.Kill();
            }, token);
        }

        public static void RefreshConfig()
        {
            var config = ConfigContent.Client.ItemIDFilter;
            config.CatchesInTheLakeWhereCurrentOrLastFishing =
                Catches.OrderByDescending(pair => pair.Key)
                .Select(pair => new CatchItem(pair.Key))
                .ToList();
        }
    }
}
