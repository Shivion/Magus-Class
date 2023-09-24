using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.IceRod
{
    internal class IceRodSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.IceBlock;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.IceBlock);
            Projectile.aiStyle = 0;
            Projectile.hide = false;
            Projectile.light = 0.5f;
            Projectile.coldDamage = true;
            buffID = ModContent.BuffType<IceRod.IceRodBuff>();
            projectileID = ModContent.ProjectileType<IceRodSpawner>();
        }

        public override void AI()
        {
            base.AI();

            if (Projectile.ai[0] == 0 && Thrown(2f, false, false))
            {
                PlaceBlock();
            }

            //Collision Check
            int TileX = (int)Projectile.Center.X / 16;
            int TileY = (int)Projectile.Center.Y / 16;
            Tile tile = Main.tile[TileX, TileY];
            if (Projectile.lavaWet
                || tile != null
                && tile.HasUnactuatedTile
                && tile.TileType != ModContent.TileType<IceRodTile>()
                && Main.tileSolid[tile.TileType]
                && !Main.tileSolidTop[tile.TileType])
            {
                Kill(0);
            }

            //Visuals
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceRod);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.3f;

            if (Projectile.velocity.X > 0f)
            {
                Projectile.rotation += 0.3f;
            }
            else
            {
                Projectile.rotation -= 0.3f;
            }
        }

        public override void Kill(int timeLeft)
        {
            Projectile.ai[2] = 1;
            Projectile.alpha = 255;

            int blockPositionX = (int)targetPosition.X / 16;
            int blockPositionY = (int)targetPosition.Y / 16;
            if (Main.tile[blockPositionX, blockPositionY].HasTile && Main.tile[blockPositionX, blockPositionY].TileType == ModContent.TileType<IceRodTile>())
            {
                WorldGen.KillTile(blockPositionX, blockPositionY);
            }

            base.Kill(Projectile.timeLeft);
        }

        private void PlaceBlock()
        {
            SoundEngine.PlaySound(in SoundID.Item27, Projectile.position);
            for (int num613 = 0; num613 < 10; num613++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceRod);
            }

            int blockPositionX = (int)targetPosition.X / 16;
            int blockPositionY = (int)targetPosition.Y / 16;
            if (Projectile.owner == Main.myPlayer && !Main.tile[blockPositionX, blockPositionY].HasTile && Vector2.Distance(Projectile.position, targetPosition) < 32f)
            {
                WorldGen.PlaceTile(blockPositionX, blockPositionY, ModContent.TileType<IceRodTile>());
                Projectile.position = new Vector2(blockPositionX, blockPositionY) * 16;
                Projectile.ai[0] = 1;
                Projectile.alpha = 255;
            }
            else
            {
                Kill(0);
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}