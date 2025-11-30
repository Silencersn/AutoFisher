using System.Reflection;

namespace AutoFisher.Common.Systems;

internal class ReflectionCodeLoader : ModSystem
{
    public static MethodInfo? RF_Main_DrawProj_FishingLine { get; private set; }

    public static MethodInfo? RF_Projectile_ReduceRemainingChumsInPool { get; private set; }
    public static MethodInfo? RF_Projectile_GetFishingPondState { get; private set; }
    public static MethodInfo? RF_Projectile_AI_061_FishingBobber_GiveItemToPlayer { get; private set; }

    public static MethodInfo? RF_Player_ItemCheck_CheckFishingBobber_PickAndConsumeBait { get; private set; }

    public override void OnModLoad()
    {
        Type type = typeof(Main);
        RF_Main_DrawProj_FishingLine =
            type.GetMethod(nameof(RF_Main.DrawProj_FishingLine), BindingFlags.Static | BindingFlags.NonPublic);

        type = typeof(Projectile);
        RF_Projectile_ReduceRemainingChumsInPool =
            type.GetMethod(nameof(RF_Projectile.ReduceRemainingChumsInPool), BindingFlags.Instance | BindingFlags.NonPublic);
        RF_Projectile_GetFishingPondState =
            type.GetMethod(nameof(RF_Projectile.GetFishingPondState), BindingFlags.Static | BindingFlags.NonPublic);
        RF_Projectile_AI_061_FishingBobber_GiveItemToPlayer =
            type.GetMethod(nameof(RF_Projectile.AI_061_FishingBobber_GiveItemToPlayer), BindingFlags.Instance | BindingFlags.NonPublic);

        type = typeof(Player);
        RF_Player_ItemCheck_CheckFishingBobber_PickAndConsumeBait =
            type.GetMethod(nameof(RF_Player.ItemCheck_CheckFishingBobber_PickAndConsumeBait), BindingFlags.Instance | BindingFlags.NonPublic);
    }

}

public static class RF_Main
{
    public static void DrawProj_FishingLine(Projectile proj, ref float polePosX, ref float polePosY, Vector2 mountedCenter)
    {
        object[] objs = [proj, polePosX, polePosY, mountedCenter];
        ReflectionCodeLoader.RF_Main_DrawProj_FishingLine?.Invoke(null, objs);
        polePosX = (float)objs[1];
        polePosY = (float)objs[2];
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
        object[] objs = [x, y, false, false, 0, 0];
        ReflectionCodeLoader.RF_Projectile_GetFishingPondState?.Invoke(null, objs);
        lava = (bool)objs[2];
        honey = (bool)objs[3];
        numWaters = (int)objs[4];
        chumCount = (int)objs[5];
    }
    public static void AI_061_FishingBobber_GiveItemToPlayer(this Projectile self, Player thePlayer, int itemType)
    {
        object[] objs = [thePlayer, itemType];
        ReflectionCodeLoader.RF_Projectile_AI_061_FishingBobber_GiveItemToPlayer?.Invoke(self, objs);
    }
}
public static class RF_Player
{
    public static void ItemCheck_CheckFishingBobber_PickAndConsumeBait(this Player self, Projectile bobber, out bool pullTheBobber, out int baitTypeUsed)
    {
        object[] objs = [bobber, false, 0];
        ReflectionCodeLoader.RF_Player_ItemCheck_CheckFishingBobber_PickAndConsumeBait?.Invoke(self, objs);
        pullTheBobber = (bool)objs[1];
        baitTypeUsed = (int)objs[2];
    }
}
