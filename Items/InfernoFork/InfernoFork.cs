using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.InfernoFork
{
    public class InfernoFork : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.InfernoFork);
            Item.mana = 25;
            Item.damage = 70;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 52;
            Item.height = 54;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<InfernoForkSpawner>();
            Item.buffType = ModContent.BuffType<InfernoForkBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.InfernoFork);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }


        internal class InfernoForkBuff : MagusSpellBuff
        {
            protected override int ManaCost => 25;
            protected override bool MultipleSpellsAllowed => true;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<InfernoForkSpawner>() };
        }
    }
}