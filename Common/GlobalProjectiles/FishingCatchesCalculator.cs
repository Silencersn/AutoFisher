using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFisher.Common.GlobalProjectiles;

public class FishingCatchesCalculator : GlobalProjectile
{
    private static bool NeedToRecalculate = false;
    public static readonly ConcurrentDictionary<int, int> Catches = [];

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
        RecalculateCatches(calculater);
    }

    private static Projectile OnSpawn_CreateCalculater(Projectile bobber)
    {
        int calculaterIndex = Projectile.NewProjectile(new EntitySource_CalculateCatches(), bobber.Center, bobber.velocity, bobber.type, bobber.damage, bobber.knockBack, bobber.owner);
        Projectile calculater = Main.projectile[calculaterIndex];
        return calculater;
    }

    public static void RecalculateCatches(Projectile calculater)
    {
        var attempts = ConfigContent.Client.ItemIDFilter.Attempts;
        Catches.Clear();

        TryCatch(() =>
        {
            for (int i = 0; i < attempts; i++)
                calculater.FishingCheck();
        }, nameof(RecalculateCatches));

        RefreshConfig();
        calculater.Kill();
    }

    public static void RefreshConfig()
    {
        var config = ConfigContent.Client.ItemIDFilter;
        var totalCount = Catches.Select(pair => pair.Value).Sum();
        config.CatchesInTheLakeWhereCurrentOrLastFishing =
            Catches
            .Where(pair => pair.Key is not ItemID.None)
            .OrderByDescending(pair => pair.Value)
            .Select(pair => new CatchItem(pair.Key, pair.Value, totalCount))
            .ToList();
    }
}
