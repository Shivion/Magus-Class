using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.NettleBurst
{
    internal class NettleBurstSpawner : VilethornishSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.NettleBurst;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 46;
            Projectile.height = 42;
            spawnedProjectileType = ProjectileID.NettleBurstRight;
            buffID = ModContent.BuffType<NettleBurst.NettleBurstBuff>();
            projectileID = ModContent.ProjectileType<NettleBurstSpawner>();
        }
    }
}