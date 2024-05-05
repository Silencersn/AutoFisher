using AutoFisher.Configs.ClientConfigs;

namespace AutoFisher.Common.GlobalProjectiles
{
    public class FishingCatchesCalculator : GlobalProjectile
    {
        private static bool NeedToRecalculate = false;
        public static Projectile Calculater { get; set; }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (!projectile.bobber) return;
            if (projectile.owner != Main.myPlayer) return;
            if (ConfigContent.NotEnableMod) return;
            if (source is AEntitySource_AutoFisher) return;

            NeedToRecalculate = true;
            Calculater = CreateCalculater(projectile);
        }

        public override void AI(Projectile projectile)
        {
            if (!projectile.bobber) return;
            if (!projectile.wet) return;
            if (projectile.owner != Main.myPlayer) return;
            if (projectile.shimmerWet) return;
            if (Calculater is null) return;
            if (projectile != Calculater) return;
            if (!NeedToRecalculate) return;

            NeedToRecalculate = false;
            CatchesCalculator.RecalculateCatches();
        }

        private static Projectile CreateCalculater(Projectile bobber)
        {
            int calculaterIndex = Projectile.NewProjectile(new EntitySource_CalculateCatches(), bobber.Center, bobber.velocity, bobber.type, bobber.damage, bobber.knockBack, bobber.owner, bobber.ai[0], bobber.ai[1]);
            Projectile calculater = Main.projectile[calculaterIndex];
            return calculater;
        }

    }
}
