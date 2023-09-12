using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.CursedFlames
{
    internal class CursedFlamesSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CursedFlames;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 32;
            spawnedProjectileType = ProjectileID.CursedFlameFriendly;
            buffID = ModContent.BuffType<CursedFlames.CursedFlamesBuff>();
            projectileID = ModContent.ProjectileType<CursedFlamesSpawner>();
            coneRadius = 15;
            spawnInterval = 15f;
            sound = SoundID.Item20;
            doSpin = false;
            horizontalSprite = true;
        }
    }

    
}