using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class CrimsonRodCloudSeed : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BloodCloudMoving;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BloodCloudMoving);
            Projectile.aiStyle = 0;
            AIType = ProjectileID.None;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = Main.mouseX + Main.screenPosition.X;
            Projectile.ai[1] = Main.mouseY + Main.screenPosition.Y;
            base.OnSpawn(source);
        }

        public override void AI()
        {
            float x = Projectile.ai[0];
            float y = Projectile.ai[1];

            bool reachedX = false;
            bool reachedY = false;
            if (Projectile.velocity.X == 0f || (Projectile.velocity.X < 0f && Projectile.Center.X < x) || (Projectile.velocity.X > 0f && Projectile.Center.X > x))
            {
                Projectile.velocity.X = 0f;
                reachedX = true;
            }
            if (Projectile.velocity.Y == 0f || (Projectile.velocity.Y < 0f && Projectile.Center.Y < y) || (Projectile.velocity.Y > 0f && Projectile.Center.Y > y))
            {
                Projectile.velocity.Y = 0f;
                reachedY = true;
            }
            if (Projectile.owner == Main.myPlayer && reachedX && reachedY)
            {
                Projectile.Kill();
            }

            Projectile.rotation += Projectile.velocity.X * 0.02f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<CrimsonRodCloud>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            return;
        }
    }
}
