using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class CrimsonRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CrimsonRod);
            Item.mana = 30;
            Item.damage = 12;
            Item.shoot = ModContent.ProjectileType<CrimsonRodCloudSeed>();
            Item.buffType = ModContent.BuffType<CrimsonRodBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimsonRod);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }
    }
}
