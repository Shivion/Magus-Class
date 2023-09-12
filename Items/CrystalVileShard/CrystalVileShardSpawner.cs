using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.CrystalVileShard
{
    internal class CrystalVileShardSpawner : VilethornishSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CrystalVileShard;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 32;
            Projectile.height = 32;
            spawnedProjectileType = ProjectileID.CrystalVileShardShaft;
            buffID = ModContent.BuffType<CrystalVileShard.CrystalVileShardBuff>();
            projectileID = ModContent.ProjectileType<CrystalVileShardSpawner>();
        }
    }
}