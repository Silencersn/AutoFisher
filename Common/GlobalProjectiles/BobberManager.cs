using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFisher.Common.GlobalProjectiles
{
    public class BobberManager : GlobalProjectile
    {
        private class Cleaner : ModSystem
        {
            public override void OnWorldUnload()
            {
                MainBobbers.Clear();
                AutoFisherBobbers.Clear();
                ActiveBobbers.Clear();
                WetBobbers.Clear();
            }
        }

        public static bool IsFishing => WetBobbers.Count > 0;

        public static Projectile? Bobber => MainBobbers.Intersect(WetBobbers).FirstOrDefault();

        public static HashSet<Projectile> MainBobbers { get; private set; } = [];
        public static HashSet<Projectile> AutoFisherBobbers { get; private set; } = [];

        public static HashSet<Projectile> ActiveBobbers { get; private set; } = [];
        public static HashSet<Projectile> WetBobbers { get; private set; } = [];

        public static Dictionary<Projectile, Item> OwnerFishingRodOfBobbers { get; private set; } = [];

        public static Projectile? Calculater { get; private set; } = null;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.bobber;
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.owner != Main.myPlayer) return;

            if (source is EntitySource_ItemUse_WithAmmo itemUse && itemUse.Item.fishingPole > 0)
            {
                OwnerFishingRodOfBobbers[projectile] = itemUse.Item;
            }

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
            OwnerFishingRodOfBobbers.Remove(projectile);
            if (projectile == Calculater) Calculater = null;
        }

        public override void PostAI(Projectile projectile)
        {
            if (projectile.owner != Main.myPlayer) return;

            if (!projectile.wet || !projectile.active) WetBobbers.Remove(projectile);
            else if(projectile.wet && projectile.active) WetBobbers.Add(projectile);
        }
    }
}
