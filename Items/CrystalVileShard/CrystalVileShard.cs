using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.CrystalVileShard
{
    public class CrystalVileShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CrystalVileShard);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 50;
            Item.damage = 20;
            Item.width = 35;
            Item.height = 35;
            Item.shoot = ModContent.ProjectileType<CrystalVileShardSpawner>();
            Item.buffType = ModContent.BuffType<CrystalVileShardBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrystalVileShard);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class CrystalVileShardBuff : MagusSpellBuff
        {
            protected override int ManaCost => 50;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<CrystalVileShardSpawner>() };
        }
    }
}