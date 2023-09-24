using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.IceRod
{
    public class IceRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceRod);
            Item.mana = 5;
            Item.damage = 28;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.width = 40;
            Item.height = 40;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IceRodSpawner>();
            Item.buffType = ModContent.BuffType<IceRodBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IceRod);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class IceRodBuff : MagusSpellBuff
        {
            protected override int ManaCost => 5;
            protected override bool MultipleSpellsAllowed => true;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<IceRodSpawner>() };
        }
    }
}