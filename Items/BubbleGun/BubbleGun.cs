using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.BubbleGun
{
    public class BubbleGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BubbleGun);
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 75;
            Item.damage = 70;
            Item.width = 70;
            Item.height = 31;
            Item.shoot = ModContent.ProjectileType<BubbleGunSpawner>();
            Item.buffType = ModContent.BuffType<BubbleGunBuff>();

            Item.staff[ModContent.ItemType<BubbleGun>()] = false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-30f, -5f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BubbleGun);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class BubbleGunBuff : MagusSpellBuff
        {
            protected override int ManaCost => 75;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<BubbleGunSpawner>() };
        }
    }
}