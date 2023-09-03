using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
            Item.mana = 25;
            Item.damage = 5;
            Item.shoot = ModContent.ProjectileType<VilethornSpawner>();
            Item.buffType = ModContent.BuffType<VilethornBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Vilethorn);
            recipe.AddTile(TileID.WorkBenches); 
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
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
            Player player = Main.player[Projectile.owner];

            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<VilethornBuff>());
            }
            if (!player.HasBuff(ModContent.BuffType<VilethornBuff>()))
            {
                Projectile.Kill();
            }

            if (!spawnPointSet)
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
        }
    }

    internal class VilethornBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VilethornSpawner>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                player.statManaMax2 -= player.ownedProjectileCounts[ModContent.ProjectileType<VilethornSpawner>()] * 25;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}