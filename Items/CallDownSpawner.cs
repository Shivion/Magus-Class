using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal abstract class CallDownSpawner : MagusProjectile
    {
        protected int[] possibleProjectiles;
        protected int horizontalSpawnSpread = 400;
        protected int horizontalAimSpread = 40;
        protected int verticalAimSpread = 40;
        protected SoundStyle? sound;
        protected float spawnFrequency = 10f;

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1; 
        }

        public override void AI()
        {
            base.AI();
            KillExistingProjectiles();

            if (Thrown())
            {
                if (Projectile.ai[1] == 0 && Projectile.ai[0] > spawnFrequency)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        //Setup
                        Vector2 startPoisition = Projectile.Center;
                        Vector2 targetPosition = Projectile.Center;
                        float speed = 5;

                        //Assign starting position
                        startPoisition.X = (startPoisition.X + Projectile.Center.X) / 2f + (float)Main.rand.Next(-horizontalSpawnSpread, horizontalSpawnSpread+1);
                        startPoisition.Y -= 500;

                        //assign velocity
                        Vector2 velocity = targetPosition - startPoisition;
                        float magnitude = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);

                        //Skew the X velocity
                        velocity.X = Projectile.Center.X - startPoisition.X + (float)Main.rand.Next(-horizontalAimSpread, horizontalAimSpread + 1) * 0.03f;

                        //ensure the projectile is falling at a minimum speed
                        if (velocity.Y < 0f)
                        {
                            velocity.Y *= -1f;
                        }
                        if (velocity.Y < 20f)
                        {
                            velocity.Y = 20f;
                        }

                        //then add the magnitude?
                        magnitude = speed / magnitude;
                        velocity *= magnitude;

                        //Skew Y
                        velocity.Y += (float)Main.rand.Next(-verticalAimSpread, verticalAimSpread + 1) * 0.02f;

                        //launch projectile
                        int newProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), startPoisition.X, startPoisition.Y, velocity.X * 0.75f, velocity.Y * 0.75f, Main.rand.NextFromList(possibleProjectiles), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, (float)Main.rand.NextDouble() * 0.3f);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, newProjectile);
                        SoundEngine.PlaySound(sound, Projectile.position);
                    }
                    Projectile.ai[0] = Main.rand.Next(0, 2);
                }
                Projectile.ai[0]++;

                //try spinning, thats a neat trick
                Projectile.rotation += 0.1f * Projectile.direction;
                Projectile.spriteDirection = 180;
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}