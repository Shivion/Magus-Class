using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.SpiritFlame
{
    internal class SpiritFlameSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SpiritFlame;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 32;
            Projectile.height = 18;
            buffID = ModContent.BuffType<SpiritFlame.SpiritFlameBuff>();
            projectileID = ModContent.ProjectileType<SpiritFlameSpawner>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            //Render Cool flame
            Vector2 vector = Projectile.position + new Vector2(30f, -1f);
            Vector2 vector2 = vector - Projectile.position;
            for (float i = 0f; i < 1f; i += 0.2f)
            {
                Vector2 vector3 = Vector2.Lerp(Projectile.oldPosition + vector2 + new Vector2(0f, Projectile.gfxOffY), vector, i);
                Dust obj = Main.dust[Dust.NewDust(vector - Vector2.One * 8f, 16, 16, DustID.Shadowflame, 0f, -2f)];
                obj.noGravity = true;
                obj.position = vector3;
                // -1 replaced -gravdir
                obj.velocity = new Vector2(0f, (0f - 1) * 2f);
                obj.scale = 1.2f;
                obj.alpha = 200;
            }

            //Shoot
            Projectile.rotation = 0f;
            if (Projectile.ai[2] == 0 && Projectile.ai[0] > 60f)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 attemptedPosition = Vector2.Zero;
                    int attempts = 0;
                    do
                    {
                        if (attempts > 10)
                        {
                            Main.NewText(attemptedPosition.ToString());
                            return;
                        }
                        attempts++;
                        float rotationOffset = Main.rand.NextFloat() * ((float)Math.PI * 2f);
                        Vector2 vectorOffset = new Vector2(1, 0).RotatedBy(rotationOffset, Vector2.Zero) * 100f;
                        attemptedPosition = Projectile.position + vectorOffset;
                    } while (!Collision.CanHit(Projectile.position, 0, 0, attemptedPosition, 0, 0));

                    Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), attemptedPosition, Vector2.Zero, ProjectileID.SpiritFlame, Projectile.damage, Projectile.knockBack, Projectile.owner, -2f);
                }
                SoundEngine.PlaySound(SoundID.Item117, Projectile.position);
                Projectile.ai[0] = Main.rand.Next(0, 10);
            }
            Projectile.ai[0]++;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }

}