using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.WaterBolt
{
    public class WaterBolt : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterBolt);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 30;
            Item.height = 32;
            Item.mana = 50;
            Item.damage = 10;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<WaterBoltSpawner>();
            Item.buffType = ModContent.BuffType<WaterBoltBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WaterBolt);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class WaterBoltBuff : MagusSpellBuff
        {
            protected override int ManaCost => 50;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<WaterBoltSpawner>() };
        }
    }
}