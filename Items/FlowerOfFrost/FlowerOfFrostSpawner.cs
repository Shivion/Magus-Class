using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.FlowerOfFrost
{
    class FlowerOfFrostSpawner : FlowerOfSomethingSpawner
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            spawnedProjectileType = ProjectileID.BallofFrost;
            spawnInterval = 50;
            projectileID = ModContent.ProjectileType<FlowerOfFrostSpawner>();
            buffID = ModContent.BuffType<FlowerOfFrost.FlowerOfFrostBuff>();
        }
    }
}