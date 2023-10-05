using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.RainbowGun
{
    internal class RainbowGunFront : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RainbowFront;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RainbowFront);
            base.SetDefaults();
            Projectile.alpha = 255;
            buffID = ModContent.BuffType<RainbowGun.RainbowGunBuff>();
            projectileID = ModContent.ProjectileType<RainbowGunFront>();
        }

        public override void AI()
        {
            base.AI();
            KillExistingProjectiles();

            int maxTimeLeft = 5000;
            Point point2 = Projectile.Center.ToTileCoordinates();
            if (!WorldGen.InWorld(point2.X, point2.Y, 2) || Main.tile[point2.X, point2.Y] == null)
            {
                Projectile.Kill();
                return;
            }
            float YVelocityBuffer = 1f;
            if (Projectile.velocity.Y < 0f)
            {
                YVelocityBuffer -= Projectile.velocity.Y / 3f;
            }
            Projectile.ai[0] += YVelocityBuffer;
            if (Projectile.ai[0] > 30f)
            {
                Projectile.velocity.Y += 0.4f;
                if (Projectile.velocity.Y > 0f)
                {
                    Projectile.velocity.X *= 0.95f;
                }
                else
                {
                    Projectile.velocity.X *= 1.05f;
                }
            }

            float magnitude = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y);
            magnitude = 15.95f * Projectile.scale / magnitude;
            Projectile.velocity *= magnitude;

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.ToRadians(90);

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[0] += 1f;
                if (Projectile.localAI[0] > 4f)
                {
                    Projectile.localAI[0] = 3f;
                    Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0.001f, Projectile.velocity.Y * 0.001f, ModContent.ProjectileType<RainbowGunBack>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                if (Projectile.timeLeft > maxTimeLeft)
                {
                    Projectile.timeLeft = maxTimeLeft;
                }
            }
        }
    }
}