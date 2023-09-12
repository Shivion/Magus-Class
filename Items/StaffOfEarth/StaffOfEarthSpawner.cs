using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.StaffOfEarth
{
    internal class StaffOfEarthSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.StaffofEarth;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 42;
            Projectile.height = 42;
            spawnedProjectileType = ProjectileID.BoulderStaffOfEarth;
            buffID = ModContent.BuffType<StaffOfEarth.StaffOfEarthBuff>();
            projectileID = ModContent.ProjectileType<StaffOfEarthSpawner>();
            coneRadius = 15;
            spawnInterval = 60f;
            sound = SoundID.Item69;
            doSpin = true;
        }
    }
}