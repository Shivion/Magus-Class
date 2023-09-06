using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Chat.Commands;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal abstract class FlowerOfSomethingSpawner : MagusProjectile
    {
        protected int spawnedProjectileType;

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.velocity = Vector2.Zero;
            Projectile.height = 16;
            Projectile.width = 16;
            spawnedProjectileType = ProjectileID.BallofFire;
            projectileID = ModContent.ProjectileType<FlowerOfFireSpawner>();
            buffID = ModContent.BuffType<FlowerOfFireBuff>();
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.position = ShivUtilities.FindRestingSpot(player.position) - new Vector2(0,Projectile.height + 10);
            base.OnSpawn(source);
        }

        public override void AI()
        {
            base.AI();
            KillExistingProjectiles();

            if (Projectile.ai[1] == 0 && Projectile.ai[0] > 60f)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    float farX = Projectile.Center.X;
                    float centerY = Projectile.Center.Y;

                    int offset = Main.rand.Next(-15, 16);
                    Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(MathHelper.ToRadians(offset));
                    int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), farX, centerY, perturbedSpeed.X, perturbedSpeed.Y, spawnedProjectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
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