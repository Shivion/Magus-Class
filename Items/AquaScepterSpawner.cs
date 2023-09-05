﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class AquaScepterSpawner : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.AquaScepter;

        Vector2 targetPosition;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.velocity = Vector2.Zero;
            //AquaScepters cast count
            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
            //Kill the older projectile
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<AquaScepterSpawner>()] > 1)
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
                player.ClearBuff(ModContent.BuffType<AquaScepterBuff>());
            }
            if (!player.HasBuff(ModContent.BuffType<AquaScepterBuff>()))
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

            bool reachedX = false;
            bool reachedY = false;

            //Move to mouse location on the first frame
            if (Projectile.ai[2] == 0)
            {
                targetPosition = Main.MouseWorld;
            }
            else
            {
                if (Projectile.velocity.X == 0f || (Projectile.velocity.X < 0f && Projectile.Center.X < targetPosition.X) || (Projectile.velocity.X > 0f && Projectile.Center.X > targetPosition.X))
                {
                    Projectile.velocity.X = 0f;
                    reachedX = true;
                }
                if (Projectile.velocity.Y == 0f || (Projectile.velocity.Y < 0f && Projectile.Center.Y < targetPosition.Y) || (Projectile.velocity.Y > 0f && Projectile.Center.Y > targetPosition.Y))
                {
                    Projectile.velocity.Y = 0f;
                    reachedY = true;
                }

            }

            if (reachedX && reachedY)
            {
                if (Projectile.ai[1] == 0 && Projectile.ai[0] > 5f)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        float farX = Projectile.position.X + Projectile.width;
                        float centerY = Projectile.Center.Y;
                        Vector2 sprayVelocity = new Vector2(1, 0);
                        sprayVelocity = sprayVelocity.RotatedBy(Projectile.rotation);
                        sprayVelocity.Normalize();
                        sprayVelocity = sprayVelocity * 10;
                        int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), farX, centerY, sprayVelocity.X, sprayVelocity.Y, ProjectileID.WaterStream, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                        SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
                    }
                    Projectile.ai[0] = Main.rand.Next(0, 2);
                }
                Projectile.ai[0]++;

                Projectile.rotation += 0.1f * Projectile.direction;
                Projectile.spriteDirection = 180;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.position += Projectile.velocity;
            }

            //duration timer, used to get the oldest projectile
            Projectile.ai[2]++;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}