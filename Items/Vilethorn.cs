using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class Vilethorn : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Vilethorn);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 32;
            Item.height = 32;
            Item.mana = 25;
            Item.damage = 5;
            Item.shoot = ModContent.ProjectileType<VilethornSpawner>();
            Item.buffType = ModContent.BuffType<VilethornBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Vilethorn);
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

    internal class VilethornSpawner : VilethornishSpawner
    {
    }

    internal class VilethornBuff : MagusSpellBuff
    {
        protected override int ManaCost => 25;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<VilethornSpawner>() };
    }
}