using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.NimbusRod
{
    internal class NimbusRodCloud : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RainCloudRaining;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RainCloudRaining);
            Projectile.aiStyle = 0;
            Projectile.width = 54;
            Projectile.height = 24;
            buffID = ModContent.BuffType<NimbusRod.NimbusRodBuff>();
            projectileID = ModContent.ProjectileType<NimbusRodCloud>();
        }

        public override void AI()
        {
            base.AI();

            bool notColliding = true;
            int centerX = (int)Projectile.Center.X;
            int BottomY = (int)(Projectile.position.Y + Projectile.height);

            //check for collision
            if (Collision.SolidTiles(new Vector2(centerX, BottomY), 2, 20))
            {
                notColliding = false;
            }

            //animate cloud
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (!notColliding && Projectile.frame > 2 || Projectile.frame > 5)
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
                        Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), centerX, BottomY, 0f, 5f, ProjectileID.RainFriendly, Projectile.damage, 0f, Projectile.owner);
                    }
                }
            }
            Projectile.localAI[0] += 1f;
            if (!(Projectile.localAI[0] >= 10f))
            {
                return;
            }
            Projectile.localAI[0] = 0f;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }

}
