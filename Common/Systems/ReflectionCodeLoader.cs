using System.Reflection;

namespace AutoFisher.Common.Systems
{
    public class ReflectionCodeLoader : ModSystem
    {
        public static MethodInfo RF_Projectile_ReduceRemainingChumsInPool { get; private set; }
        public static MethodInfo RF_Projectile_GetFishingPondState { get; private set; }

        public override void OnModLoad()
        {
            Type type = typeof(Projectile);
            RF_Projectile_ReduceRemainingChumsInPool =
                type.GetMethod(nameof(RF_Projectile.ReduceRemainingChumsInPool), BindingFlags.Instance | BindingFlags.NonPublic);
            RF_Projectile_GetFishingPondState =
                type.GetMethod(nameof(RF_Projectile.GetFishingPondState), BindingFlags.Static | BindingFlags.NonPublic);
        }

    }

    public static class RF_Projectile
    {
        public static void ReduceRemainingChumsInPool(this Projectile self)
        {
            ReflectionCodeLoader.RF_Projectile_ReduceRemainingChumsInPool?.Invoke(self, null);
        }
        public static void GetFishingPondState(int x, int y, out bool lava, out bool honey, out int numWaters, out int chumCount)
        {
            object[] objects = { x, y, false, false, 0, 0 };
            ReflectionCodeLoader.RF_Projectile_GetFishingPondState?.Invoke(null, objects);
            lava = (bool)objects[2];
            honey = (bool)objects[3];
            numWaters = (int)objects[4];
            chumCount = (int)objects[5];
        }
    }
}
