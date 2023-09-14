using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.RainbowGun
{
    internal class RainbowGunBack : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RainbowBack;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RainbowBack);
            base.SetDefaults();
            Projectile.alpha = 255;
            Projectile.light = 0;
            buffID = ModContent.BuffType<RainbowGun.RainbowGunBuff>();
            projectileID = ModContent.ProjectileType<RainbowGunBack>();
        }

        public override void AI()
        {
            base.AI();

            float num = Main.DiscoR / 255f;
            float num2 = Main.DiscoG / 255f;
            float num3 = Main.DiscoB / 255f;
            num = (num + 1f) / 2f;
            num2 = (num2 + 1f) / 2f;
            num3 = (num3 + 1f) / 2f;
            num *= 0.5f;
            num2 *= 0.5f;
            num3 *= 0.5f;

            Lighting.AddLight(Projectile.Center, num, num2, num3);

            if (Projectile.localAI[0] == 0f)
            {
                if (Projectile.velocity.X > 0f)
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.ToRadians(90);//1.57f;

                }
                else
                {
                    Projectile.spriteDirection = 1;
                    Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.ToRadians(90);//1.57f;
                }
                Projectile.localAI[0] = 1f;
            }

            if (Projectile.ai[2] != 1 && Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            //return new Color((255 - Projectile.alpha) * 2, (255 - Projectile.alpha) * 2, (255 - Projectile.alpha) * 2, 255 - Projectile.alpha);
            int r = 255 - Projectile.alpha;
            int g = 255 - Projectile.alpha;
            int b = 255 - Projectile.alpha;
            return new Color(r, g, b, 0);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }




}