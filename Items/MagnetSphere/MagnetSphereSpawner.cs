using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.MagnetSphere
{
    internal class MagnetSphereSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MagnetSphereBall;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.MagnetSphereBall);
            Projectile.aiStyle = 0;
            Projectile.hide = false;
            buffID = ModContent.BuffType<MagnetSphere.MagnetSphereBuff>();
            projectileID = ModContent.ProjectileType<MagnetSphereSpawner>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 4)
                {
                    Projectile.frame = 0;
                }
            }

            //slow down near target position
            if (Projectile.velocity.Length() > ((targetPosition - Projectile.Center) / 100).Length())
            {
                Projectile.velocity = (targetPosition - Projectile.Center) / 100;
            }

            //Get target
            if (Projectile.owner == Main.myPlayer)
            {
                int[] targets = new int[20];
                int targetIndex = 0;
                float num3 = 300f;
                bool hasTarget = false;
                for (int i = 0; i < 200; i++)
                {
                    if (!Main.npc[i].CanBeChasedBy(Projectile))
                    {
                        continue;
                    }
                    float num6 = Main.npc[i].position.X + Main.npc[i].width / 2;
                    float num7 = Main.npc[i].position.Y + Main.npc[i].height / 2;
                    if (Math.Abs(Projectile.position.X + Projectile.width / 2 - num6) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num7) < num3 && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                    {
                        if (targetIndex < 20)
                        {
                            targets[targetIndex] = i;
                            targetIndex++;
                        }
                        hasTarget = true;
                    }
                }
                if (Projectile.timeLeft < 30)
                {
                    hasTarget = false;
                }
                if (hasTarget)
                {
                    //Shoot
                    int randomTargetIndex = Main.rand.Next(targetIndex);
                    randomTargetIndex = targets[randomTargetIndex];
                    float targetX = Main.npc[randomTargetIndex].position.X + Main.npc[randomTargetIndex].width / 2;
                    float targetY = Main.npc[randomTargetIndex].position.Y + Main.npc[randomTargetIndex].height / 2;
                    Projectile.localAI[0] += 1f;
                    if (Projectile.localAI[0] > 8f)
                    {
                        Projectile.localAI[0] = 0f;
                        float X = targetX - Projectile.Center.X;
                        float Y = targetY - Projectile.Center.Y;
                        float magnitude = (float)Math.Sqrt(X * X + Y * Y);
                        magnitude = 6f / magnitude;
                        X *= magnitude;
                        Y *= magnitude;
                        Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), Projectile.Center.X, Projectile.Center.Y, X, Y, 255, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
        }

    }
}