using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
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
            //Projectile.scale = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y) * 0.008f;
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

        public override bool PreDraw(ref Color lightColor)
        {
            // If the ray doesn't have a defined direction, don't draw anything.
            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            //Get the scale of the ray from the velocity
            Vector2 drawScale = new Vector2(1, (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y) * 0.0101f);
            Vector2 endPosition = Projectile.position + Projectile.velocity;

            DrawRay(Main.spriteBatch, texture, Projectile.position, endPosition, drawScale);

            // Returning false prevents Terraria from trying to draw the Projectile itself.
            return false;
        }

        private void DrawRay(SpriteBatch sb, Texture2D tex, Vector2 start, Vector2 end, Vector2 scale)
        {
            Vector2 vector2 = Vector2.Normalize(start - end);
            float rotation = vector2.ToRotation() - MathF.PI / 2f;

            if (vector2.HasNaNs())
            {
                return;
            }

            Color color = Color.White;

            sb.Draw(tex, end - Main.screenPosition, null, color, rotation, new Vector2(scale.X * 10, 0), scale, SpriteEffects.None, 1f);
        }
    }
}