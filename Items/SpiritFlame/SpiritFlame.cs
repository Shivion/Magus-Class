using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.SpiritFlame
{
    public class SpiritFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SpiritFlame);
            Item.mana = 100;
            Item.damage = 85;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 32;
            Item.height = 18;
            Item.glowMask = -1;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<SpiritFlameSpawner>();
            Item.buffType = ModContent.BuffType<SpiritFlameBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpiritFlame);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class SpiritFlameBuff : MagusSpellBuff
        {
            protected override int ManaCost => 100;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<SpiritFlameSpawner>() };
        }
    }
}