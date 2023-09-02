using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class Vilethorn : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Vilethorn);
            Item.color = Color.Purple;
            Item.damage = 5;
            Item.shoot = ModContent.ProjectileType<VilethornSpawner>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Vilethorn);
            recipe.AddTile(TileID.WorkBenches); 
            recipe.Register();
        }
    }

    internal class VilethornSpawner : ModProjectile
    {
        int spawnedProjectile;
        bool spawnPointSet;
        Vector2 spawnPoint;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.velocity = Vector2.Zero;
            Projectile.ai[0] = 0;
            spawnedProjectile = -1;
        }

        public override void AI()
        {
            if(!spawnPointSet)
            {
                spawnPoint = Projectile.position;
                spawnPointSet = true;
            }
            if (Projectile.ai[0] < 10f && spawnedProjectile < 0 || Main.projectile[spawnedProjectile].alpha >= 255)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), Projectile.position.X + Projectile.velocity.X + (float)(Projectile.width / 2), Projectile.position.Y + Projectile.velocity.Y + (float)(Projectile.height / 2), Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.VilethornBase, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[spawnedProjectile].damage = Projectile.damage;
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                }
                Projectile.ai[0]++;
            }
            Projectile.rotation += 0.4f * Projectile.direction;
            Projectile.position = spawnPoint;
            if (Projectile.ai[0] >= 10f)
            {
                Projectile.Kill();
            }
        }
    }
}