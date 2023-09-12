using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.LunarFlare
{
    internal class LunarFlareSpawner : CallDownSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LunarFlareBook;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 30;
            buffID = ModContent.BuffType<LunarFlare.LunarFlareBuff>();
            projectileID = ModContent.ProjectileType<LunarFlareSpawner>();
            possibleProjectiles = new int[] { ProjectileID.LunarFlare };
            sound = SoundID.Item88;
        }
    }

}