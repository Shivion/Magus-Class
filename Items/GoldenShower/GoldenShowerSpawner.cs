using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.GoldenShower
{
    internal class GoldenShowerSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.GoldenShower;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 30;
            spawnedProjectileType = ProjectileID.GoldenShowerFriendly;
            buffID = ModContent.BuffType<GoldenShower.GoldenShowerBuff>();
            projectileID = ModContent.ProjectileType<GoldenShowerSpawner>();
            coneRadius = 5;
            spawnInterval = 18f;
            sound = SoundID.Item13;
            doSpin = false;
            horizontalSprite = true;
        }
    }
}