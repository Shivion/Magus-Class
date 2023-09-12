using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.MeteorStaff
{
    internal class MeteorStaffSpawner : CallDownSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MeteorStaff;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 44;
            Projectile.height = 42;
            buffID = ModContent.BuffType<MeteorStaff.MeteorStaffBuff>();
            projectileID = ModContent.ProjectileType<MeteorStaffSpawner>();
            possibleProjectiles = new int[] { ProjectileID.Meteor1, ProjectileID.Meteor2, ProjectileID.Meteor3 };
            sound = SoundID.Item88;
        }
    }
}