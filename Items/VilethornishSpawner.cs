using MagusClass.Items.Vilethorn;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal abstract class VilethornishSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Vilethorn;

        int spawnedProjectile;
        protected int spawnedProjectileType;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.velocity = Vector2.Zero;
            spawnedProjectile = -1;
            spawnedProjectileType = ProjectileID.VilethornBase;
            buffID = ModContent.BuffType<VilethornBuff>();
            projectileID = ModContent.ProjectileType<VilethornSpawner>();
        }

        public override void AI()
        {
            base.AI();
            KillExistingProjectiles();

            if (Projectile.ai[2] == 0 && Projectile.ai[0] > 10f && (spawnedProjectile < 0 || Main.projectile[spawnedProjectile].alpha >= 255))
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, spawnedProjectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                }
                Projectile.ai[0] = 0;
            }
            Projectile.ai[0]++;
            Projectile.rotation += 0.4f * Projectile.direction;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}