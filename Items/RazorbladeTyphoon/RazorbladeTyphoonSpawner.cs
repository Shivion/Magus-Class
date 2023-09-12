using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.RazorbladeTyphoon
{
    internal class RazorbladeTyphoonSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.RazorbladeTyphoon;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 30;
            spawnedProjectileType = ProjectileID.Typhoon;
            buffID = ModContent.BuffType<RazorbladeTyphoon.RazorbladeTyphoonBuff>();
            projectileID = ModContent.ProjectileType<RazorbladeTyphoonSpawner>();
            coneRadius = 180;
            spawnInterval = 6f;
            sound = SoundID.Item84;
            doSpin = true;
            thrown = true;
        }
    }

}