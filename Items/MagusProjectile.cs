using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal abstract class MagusProjectile : ModProjectile
    {
        protected int buffID;
        protected int projectileID;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.velocity = Vector2.Zero;
        }

        public override void AI()
        {
            //duration timer, used to get the oldest projectile
            Projectile.ai[2]++;

            Player player = Main.player[Projectile.owner];
            //Kill all projectiles without the buff
            if (player.dead || !player.active)
            {
                player.ClearBuff(buffID);
            }
            if (!player.HasBuff(buffID))
            {
                Projectile.ai[1] = 1;
            }
            else
            {
                Projectile.timeLeft = 3600;
            }

            if (Projectile.ai[1] == 1)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
                return;
            }
        }

        public void KillExistingProjectiles()
        {
            //Kill the older projectile
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[projectileID] > 1)
            {
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type && Main.projectile[i].ai[1] < 1)
                    {
                        if (Main.projectile[i].ai[2] > Projectile.ai[2])
                        {
                            Main.projectile[i].ai[1] = 1;
                        }
                    }
                }
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}