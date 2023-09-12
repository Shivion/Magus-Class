using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.BubbleGun
{
    internal class BubbleGunSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BubbleGun;

        public override void SetDefaults()
        {
            base.SetDefaults();
            spawnedProjectileType = ProjectileID.Bubble;
            Projectile.width = 70;
            Projectile.height = 31;
            buffID = ModContent.BuffType<BubbleGun.BubbleGunBuff>();
            projectileID = ModContent.ProjectileType<BubbleGunSpawner>();
            coneRadius = 15;
            spawnInterval = 10f;
            sound = SoundID.Item85;
            doSpin = false;
            horizontalSprite = true;
        }
    }
}