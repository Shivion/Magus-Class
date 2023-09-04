using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class VilethornSpawner : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Vilethorn;

        int spawnedProjectile;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.velocity = Vector2.Zero;
            //Vilethorns cast count
            Projectile.ai[0] = 0;
            spawnedProjectile = -1;
        }

        public override void AI()
        {
            //duration timer, used to get the oldest projectile
            Projectile.ai[2]++;
            //Kill the older projectile
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VilethornSpawner>()] > 1)
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
                player.ClearBuff(ModContent.BuffType<VilethornBuff>());
            }
            if (!player.HasBuff(ModContent.BuffType<VilethornBuff>()))
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
            if (Projectile.ai[1] == 0 && Projectile.ai[0] < 10f && spawnedProjectile < 0 || Main.projectile[spawnedProjectile].alpha >= 255)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.VilethornBase, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                }
                Projectile.ai[0]++;
            }
            Projectile.rotation += 0.4f * Projectile.direction;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}