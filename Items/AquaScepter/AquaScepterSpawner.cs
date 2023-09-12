using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.AquaScepter
{
    internal class AquaScepterSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.AquaScepter;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 18;
            Projectile.height = 48;
            buffID = ModContent.BuffType<AquaScepter.AquaScepterBuff>();
            projectileID = ModContent.ProjectileType<AquaScepterSpawner>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            if (Thrown())
            {
                if (Projectile.ai[2] == 0 && Projectile.ai[0] > 5f)
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
                    }
                    SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
                    Projectile.ai[0] = Main.rand.Next(0, 2);
                }
                Projectile.ai[0]++;

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