using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.CrimsonRod
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
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 25;
            Item.damage = 12;
            Item.width = 38;
            Item.height = 40;
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

        internal class CrimsonRodBuff : MagusSpellBuff
        {
            protected override int ManaCost => 25;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<CrimsonRodCloudSeed>(), ModContent.ProjectileType<CrimsonRodCloud>() };
        }
    }
}
