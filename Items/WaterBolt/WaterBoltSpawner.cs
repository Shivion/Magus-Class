using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.WaterBolt
{
    internal class WaterBoltSpawner : SimpleProjectileSpawner
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 30;
            Projectile.height = 16;
            spawnedProjectileType = ProjectileID.WaterBolt;
            buffID = ModContent.BuffType<WaterBolt.WaterBoltBuff>();
            projectileID = ModContent.ProjectileType<WaterBoltSpawner>();
            sound = SoundID.Item21;
            coneRadius = 15;
            spawnInterval = 60f;
            doSpin = false;
        }
    }
}