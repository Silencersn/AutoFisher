using AutoFisher.Common.Players;

namespace AutoFisher.Common.GlobalProjectiles
{
    public class MultipleFishingLines : GlobalProjectile
    {
        public static readonly List<Projectile> Bobbers = new List<Projectile>();

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (!projectile.bobber) return;
            if (projectile.owner != Main.myPlayer) return;

            if (source is not EntitySource_CalculateCatches) Bobbers.Add(projectile);
            CatchesRecorder.ClearLocalPlayerData(false);

            if (ConfigContent.NotEnableMod) return;
            if (source is AEntitySource_AutoFisher) return;



            if (!ConfigContent.MultipleFishingLines) return;
            float range = MathHelper.ToRadians(MathF.Log(ConfigContent.Client.Common.MultipleFishingLines.FishingLinesCount) * 8);
            float unit = range / (ConfigContent.Client.Common.MultipleFishingLines.FishingLinesCount - 1);
            for (int i = 0; i < ConfigContent.Client.Common.MultipleFishingLines.FishingLinesCount - 1; i++)
            {
                float radians = i % 2 == 0 ? (i / 2 + 1) * unit : -(i / 2 + 1) * unit;
                OnSpawn_CreateNewBobber(projectile, radians);
            }
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (projectile.bobber)
            {
                if (Bobbers.Contains(projectile))
                {
                    Bobbers.Remove(projectile);
                }
            }
        }

        public static void RemoveNotActiveProjectiles()
        {
            Bobbers.RemoveAll(bobber => !bobber.active);
        }

        private static Projectile OnSpawn_CreateNewBobber(Projectile bobber, double radians)
        {
            int newBobberIndex = Projectile.NewProjectile(new EntitySource_MultipleFishingLines(), bobber.Center, bobber.velocity.RotatedBy(radians), bobber.type, bobber.damage, bobber.knockBack, bobber.owner, bobber.ai[0], bobber.ai[1]);
            Projectile newBobber = Main.projectile[newBobberIndex];
            return newBobber;
        }

    }
}
