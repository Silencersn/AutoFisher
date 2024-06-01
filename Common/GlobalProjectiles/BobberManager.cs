using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFisher.Common.GlobalProjectiles
{
    public class BobberManager : GlobalProjectile
    {
        public static bool IsFishing => WetBobbers.Count > 0;

        public static List<Projectile> MainBobbers { get; private set; } = [];
        public static List<Projectile> AutoFisherBobbers { get; private set; } = [];

        public static List<Projectile> ActiveBobbers { get; private set; } = [];
        public static List<Projectile> WetBobbers { get; private set; } = [];

        public static Projectile? Calculater { get; private set; } = null;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.bobber;
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.owner != Main.myPlayer) return;

            ActiveBobbers.Add(projectile);
            if (source is AEntitySource_AutoFisher)
            {
                AutoFisherBobbers.Add(projectile);
                if (source is EntitySource_CalculateCatches) Calculater = projectile;
            }
            else
            {
                MainBobbers.Add(projectile);
            }
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (projectile.owner != Main.myPlayer) return;

            MainBobbers.Remove(projectile);
            AutoFisherBobbers.Remove(projectile);
            ActiveBobbers.Remove(projectile);
            WetBobbers.Remove(projectile);
            if (projectile == Calculater) Calculater = null;
        }

        public override void PostAI(Projectile projectile)
        {
            if (projectile.owner != Main.myPlayer) return;

            if (!projectile.wet && WetBobbers.Contains(projectile) || !projectile.active) WetBobbers.Remove(projectile);
            else if(projectile.wet && !WetBobbers.Contains(projectile)) WetBobbers.Add(projectile);
        }
    }
}
