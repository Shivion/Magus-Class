using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class CrimsonRodCloud : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BloodCloudRaining;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BloodCloudRaining);
            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            //duration timer, used to get the oldest projectile
            Projectile.ai[2]++;
            //Kill the older projectile
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CrimsonRodCloud>()] > 1)
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

            bool notColliding = true;
            int centerX = (int)Projectile.Center.X;
            int BottomY = (int)(Projectile.position.Y + (float)Projectile.height);

            //Fade projectile to kill if ai[1] is 1
            if (Projectile.ai[1] == 1)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
            }

            //check for collision
            if (Collision.SolidTiles(new Vector2((float)centerX, (float)BottomY), 2, 20))
            {
                notColliding = false;
            }
            //animate cloud
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if ((!notColliding && Projectile.frame > 2) || Projectile.frame > 5)
                {
                    Projectile.frame = 0;
                }
            }
            //if its not colliding summon rain
            else if (notColliding)
            {
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] > 10f)
                {
                    Projectile.ai[0] = 0f;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        centerX += Main.rand.Next(-14, 15);
                        Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), centerX, BottomY, 0f, 5f, 245, Projectile.damage, 0f, Projectile.owner);
                    }
                }
            }
            Projectile.localAI[0] += 1f;
            if (!(Projectile.localAI[0] >= 10f))
            {
                return;
            }
            Projectile.localAI[0] = 0f;


            //kill all projectiles without the buff
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<CrimsonRodBuff>());
            }
            if (!player.HasBuff(ModContent.BuffType<CrimsonRodBuff>()))
            {
                Projectile.ai[1] = 1;
            }
        }
    }
}
