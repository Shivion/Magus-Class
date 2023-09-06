using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace MagusClass.Items
{
    internal abstract class SimpleProjectileSpawner : MagusProjectile
    {
        protected int spawnedProjectileType;
        protected int coneRadius;
        protected SoundStyle? sound;
        protected float spawnInterval;
        protected float projectileXSpawnOffset = 0;
        protected bool doSpin;
        protected bool horizontalSprite;

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
            base.AI();
            KillExistingProjectiles();

            if (Projectile.ai[1] == 0 && Projectile.ai[0] > spawnInterval)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    float X = Projectile.Center.X;
                    X += (float)projectileXSpawnOffset;
                    float Y = Projectile.Center.Y;

                    int offset = Main.rand.Next(-coneRadius, coneRadius + 1);
                    //Y += offset;
                    Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(MathHelper.ToRadians(offset) * Projectile.direction);
                    int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), X, Y, perturbedSpeed.X, perturbedSpeed.Y, spawnedProjectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                    SoundEngine.PlaySound(sound, Projectile.position);
                }
                Projectile.ai[0] = 0;
            }
            Projectile.ai[0]++;

            if(doSpin)
            {
                Projectile.rotation += 0.4f * Projectile.direction;
            }
            else
            {
                if(horizontalSprite)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
                    Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
                }
                else
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); ;
                    Projectile.spriteDirection = Projectile.direction;
                }
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}