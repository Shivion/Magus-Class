using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public partial class FlowerOfFrost : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FlowerofFrost);
            Item.width = 31;
            Item.height = 31;
            Item.mana = 50;
            Item.damage = 10;
            Item.shoot = ModContent.ProjectileType<FlowerOfFrostSpawner>();
            Item.buffType = ModContent.BuffType<FlowerOfFrostBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FlowerofFrost);
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

    class FlowerOfFrostSpawner : FlowerOfSomethingSpawner
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            spawnedProjectileType = ProjectileID.BallofFrost;
            projectileID = ModContent.ProjectileType<FlowerOfFrostSpawner>();
            buffID = ModContent.BuffType<FlowerOfFrostBuff>();
        }
    }

    class FlowerOfFrostBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<FlowerOfFrostSpawner>() };
    }
}