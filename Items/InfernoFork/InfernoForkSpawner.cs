using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.InfernoFork
{
    internal class InfernoForkSpawner : MagusProjectile
    {
        bool isStuck;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.friendly = true;
            DrawOffsetX = -52;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 17;
            Projectile.width = 4;
            Projectile.height = 5;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            buffID = ModContent.BuffType<InfernoFork.InfernoForkBuff>();
            projectileID = ModContent.ProjectileType<InfernoForkSpawner>();
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.position += new Vector2(0, -25f);
            Projectile.velocity *= 2;
            base.OnSpawn(source);
        }

        public override void AI()
        {
            base.AI();

            if (!isStuck)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.position += Projectile.velocity;
                if (Projectile.localAI[2] > 10f)
                {
                    Projectile.velocity += new Vector2(0, 0.1f);
                }
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }

                //isStuck = false;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;

                if (Projectile.ai[2] == 0 && Projectile.ai[0] == 0)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        float farX = Projectile.position.X + Projectile.width;
                        float centerY = Projectile.Center.Y;
                        Vector2 sprayVelocity = new Vector2(1, 0);
                        sprayVelocity = sprayVelocity.RotatedBy(Projectile.rotation);
                        int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), farX, centerY, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.InfernoFriendlyBlast, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                        SoundEngine.PlaySound(SoundID.Item73, Projectile.position);
                    }
                    Projectile.ai[0] = 160 + Main.rand.Next(0, 2);
                }
                Projectile.ai[0]--;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            isStuck = true;
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}