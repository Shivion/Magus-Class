using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.GoldenShower
{
    public class GoldenShower : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenShower);
            Item.autoReuse = false;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 28;
            Item.height = 32;
            Item.mana = 75;
            Item.damage = 30;
            Item.shoot = ModContent.ProjectileType<GoldenShowerSpawner>();
            Item.buffType = ModContent.BuffType<GoldenShowerBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldenShower);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class GoldenShowerBuff : MagusSpellBuff
        {
            protected override int ManaCost => 75;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<GoldenShowerSpawner>() };
        }
    }
}