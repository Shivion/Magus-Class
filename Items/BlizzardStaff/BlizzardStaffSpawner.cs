using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.BlizzardStaff
{
    internal class BlizzardStaffSpawner : CallDownSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BlizzardStaff;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 58;
            Projectile.height = 58;
            buffID = ModContent.BuffType<BlizzardStaff.BlizzardStaffBuff>();
            projectileID = ModContent.ProjectileType<BlizzardStaffSpawner>();
            possibleProjectiles = new int[] { ProjectileID.Blizzard };
            sound = SoundID.Item28;
        }
    }
}