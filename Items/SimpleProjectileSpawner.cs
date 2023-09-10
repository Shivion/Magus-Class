using Microsoft.Xna.Framework;
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
        protected bool doSpin;
        protected bool horizontalSprite;
        protected bool thrown;

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
            if (!thrown || Thrown())
            {
                if (Projectile.ai[2] == 0 && Projectile.ai[0] > spawnInterval)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        float X = Projectile.Center.X;
                        float Y = Projectile.Center.Y;

                        float offset = Main.rand.NextFloat(-coneRadius, coneRadius + 1);
                        Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(MathHelper.ToRadians(offset) * Projectile.direction);
                        int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), X, Y, perturbedSpeed.X, perturbedSpeed.Y, spawnedProjectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                    }
                    SoundEngine.PlaySound(sound, Projectile.position);
                    Projectile.ai[0] = 0;
                }
                Projectile.ai[0]++;

                if (doSpin)
                {
                    Projectile.rotation += 0.4f * Projectile.direction;
                }
                else
                {
                    if (horizontalSprite)
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
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}