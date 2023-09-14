using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.MedusaHead
{
    public class MedusaHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MedusaHead);
            Item.mana = 50;
            Item.damage = 40;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 38;
            Item.height = 34;
            Item.autoReuse = false;
            Item.UseSound = SoundID.ScaryScream;
            Item.shoot = ModContent.ProjectileType<MedusaHeadSpawner>();
            Item.buffType = ModContent.BuffType<MedusaHeadBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MedusaHead);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class MedusaHeadBuff : MagusSpellBuff
        {
            protected override int ManaCost => 50;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<MedusaHeadSpawner>() };
        }
    }
}