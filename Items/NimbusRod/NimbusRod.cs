using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.NimbusRod
{
    public class NimbusRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.NimbusRod);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 25;
            Item.damage = 30;
            Item.width = 46;
            Item.height = 46;
            Item.shoot = ModContent.ProjectileType<NimbusRodCloudSeed>();
            Item.buffType = ModContent.BuffType<NimbusRodBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.NimbusRod);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class NimbusRodBuff : MagusSpellBuff
        {
            protected override int ManaCost => 25;
            protected override bool MultipleSpellsAllowed => true;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<NimbusRodCloudSeed>(), ModContent.ProjectileType<NimbusRodCloud>() };
        }
    }
}
