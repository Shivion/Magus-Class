using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class FlowerOfSomethingSpawner : ModProjectile
    {
        protected int spawnedProjectileType;
        protected int buffID;
        protected int projectileID;

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.velocity = Vector2.Zero;
            spawnedProjectileType = ProjectileID.BallofFire;
            projectileID = ModContent.ProjectileType<FlowerOfFireSpawner>();
            buffID = ModContent.BuffType<FlowerOfFireBuff>();
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.position = ShivUtilities.FindRestingSpot(player.position) - new Vector2(0, Projectile.height + 10);
            base.OnSpawn(source);
        }

        public override void AI()
        {
            //duration timer, used to get the oldest projectile
            Projectile.ai[2]++;
            //Kill the older projectile
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[spawnedProjectileType] > 1)
            {
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type && Main.projectile[i].ai[1] < 1)
                    {
                        if (Main.projectile[i].ai[2] > Projectile.ai[2])
                        {
                            Main.projectile[i].ai[1] = 1;
                        }
                    }
                }
            }

            //Kill all projectiles without the buff
            if (player.dead || !player.active)
            {
                player.ClearBuff(buffID);
            }
            if (!player.HasBuff(buffID))
            {
                Projectile.ai[1] = 1;
            }

            if (Projectile.ai[1] == 1)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
            }

            if (Projectile.ai[1] == 0 && Projectile.ai[0] > 60f)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    float farX = Projectile.Center.X;
                    float centerY = Projectile.Center.Y;

                    int offset = Main.rand.Next(-15, 16);
                    Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(MathHelper.ToRadians(offset));
                    int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), farX, centerY, perturbedSpeed.X, perturbedSpeed.Y, projectileID, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                    SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                }
                Projectile.ai[0] = 0;
            }
            Projectile.ai[0]++;

            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); ; 
            Projectile.spriteDirection = Projectile.direction;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}