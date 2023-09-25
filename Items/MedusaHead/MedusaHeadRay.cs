using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.MedusaHead
{
    internal class MedusaHeadRay : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MedusaHeadRay;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.MedusaHeadRay);
            //DrawOffsetX = -100;
            //DrawOriginOffsetX = 50;
            DrawOriginOffsetY = -90;
            Projectile.aiStyle = 0;
            Projectile.hide = false;
        }

        public override void AI()
        {
            float num887 = 20f;
            Projectile.localAI[0]++;
            Projectile.alpha = (int)MathHelper.Lerp(0f, 150f, Projectile.localAI[0] / num887);
            int parentProjectile = (int)Projectile.ai[0];
            if (Projectile.localAI[0] >= num887 || parentProjectile < 0 || parentProjectile > 1000 || !Main.projectile[parentProjectile].active)
            {
                Projectile.Kill();
                return;
            }
            Projectile.scale = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y) * 0.009f;
            Projectile.Center = Main.projectile[parentProjectile].Center + new Vector2(Main.projectile[parentProjectile].width, Main.projectile[parentProjectile].height);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = 90;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 63 - Projectile.alpha / 4);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}