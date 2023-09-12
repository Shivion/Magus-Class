using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace MagusClass.Items.ClingerStaff
{
    internal class ClingerStaffSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ClingerStaff;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.ClingerStaff);
            Projectile.hide = true;
            Projectile.alpha = 0;
            buffID = ModContent.BuffType<ClingerStaff.ClingerStaffBuff>();
            projectileID = ModContent.ProjectileType<ClingerStaffSpawner>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            Projectile.position.Y = Projectile.ai[1];
            Projectile.height = (int)Projectile.ai[0];
            if (Projectile.Center.X > Main.player[Projectile.owner].Center.X)
            {
                Projectile.direction = 1;
            }
            else
            {
                Projectile.direction = -1;
            }

            //Spawn Dust
            float num805 = Projectile.width * Projectile.height * 0.0045f;
            for (int i = 0; i < num805; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 100);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
                Main.dust[dust].velocity.Y -= 0.5f;
                Main.dust[dust].scale = 1.4f;
                Main.dust[dust].position.X += 6f;
                Main.dust[dust].position.Y -= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(39, 420);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(39, 420, quiet: false);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}