namespace AutoFisher.Common.GlobalProjectiles;

public class MultipleFishingLines : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return entity.bobber;
    }

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (projectile.owner != Main.myPlayer)
            return;

        CatchesRecorder.ClearLocalPlayerData(false);

        if (ConfigContent.NotEnableMod)
            return;

        if (!ConfigContent.MultipleFishingLines)
            return;

        if (source is AEntitySource_AutoFisher)
            return;

        Item? owner = null;
        if (source is EntitySource_ItemUse_WithAmmo itemUse && itemUse.Item.fishingPole > 0)
            owner = itemUse.Item;
        var config = ConfigContent.Client.Common.MultipleFishingLines;
        float range = MathHelper.ToRadians(MathF.Log(config.FishingLinesCount) * 8);
        int count = config.FishingLinesCount - 1;
        float unit = range / count;
        var spawnSource = new EntitySource_MultipleFishingLines(owner);
        for (int i = 0; i < count; i++)
        {
            float radians = (i % 2 is 0) ? (i / 2 + 1) * unit : -(i / 2 + 1) * unit;
            Projectile.NewProjectile(spawnSource, projectile.Center, projectile.velocity.RotatedBy(radians), projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1]);
        }
    }
}
