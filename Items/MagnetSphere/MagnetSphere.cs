using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.MagnetSphere
{
    public class MagnetSphere : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MagnetSphere);
            Item.mana = 50;
            Item.damage = 48;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 44;
            Item.height = 42;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<MagnetSphereSpawner>();
            Item.buffType = ModContent.BuffType<MagnetSphereBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MagnetSphere);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class MagnetSphereBuff : MagusSpellBuff
        {
            protected override int ManaCost => 50;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<MagnetSphereSpawner>() };
        }
    }
}