using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class WaterBoltSpawner : ModProjectile
    {
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
            //duration timer, used to get the oldest projectile
            Projectile.ai[2]++;
            //Kill the older projectile
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<WaterBoltSpawner>()] > 1)
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
                player.ClearBuff(ModContent.BuffType<WaterBoltBuff>());
            }
            if (!player.HasBuff(ModContent.BuffType<WaterBoltBuff>()))
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
                    centerY += offset;
                    Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(MathHelper.ToRadians(offset));
                    int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), farX, centerY, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.WaterBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                }
                Projectile.ai[0] = 0;
            }
            Projectile.ai[0]++;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); ; 
            Projectile.spriteDirection = Projectile.direction;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}