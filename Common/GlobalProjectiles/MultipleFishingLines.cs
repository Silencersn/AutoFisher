namespace AutoFisher.Common.GlobalProjectiles
{
    public class MultipleFishingLines : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.owner != Main.myPlayer) return;

            CatchesRecorder.ClearLocalPlayerData(false);

            if (ConfigContent.NotEnableMod) return;
            if (source is AEntitySource_AutoFisher) return;
            if (!ConfigContent.MultipleFishingLines) return;

            Item? owner = null;
            if (source is EntitySource_ItemUse_WithAmmo itemUse && itemUse.Item.fishingPole > 0)
            {
                owner = itemUse.Item;
            }
            var config = ConfigContent.Client.Common.MultipleFishingLines;
            float range = MathHelper.ToRadians(MathF.Log(config.FishingLinesCount) * 8);
            int count = config.FishingLinesCount - 1;
            float unit = range / count;
            for (int i = 0; i < count; i++)
            {
                float radians = (i % 2 is 0) ? (i / 2 + 1) * unit : -(i / 2 + 1) * unit;
                OnSpawn_CreateNewBobber(projectile, radians, owner);
            }
        }

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.bobber;
        }

        private static Projectile OnSpawn_CreateNewBobber(Projectile bobber, double radians, Item? owner)
        {
            int newBobberIndex = Projectile.NewProjectile(new EntitySource_MultipleFishingLines(owner), bobber.Center, bobber.velocity.RotatedBy(radians), bobber.type, bobber.damage, bobber.knockBack, bobber.owner, bobber.ai[0], bobber.ai[1]);
            Projectile newBobber = Main.projectile[newBobberIndex];
            return newBobber;
        }

    }
}
