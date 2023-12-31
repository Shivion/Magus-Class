﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.LunarFlare
{
    public class LunarFlare : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LunarFlareBook;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LunarFlareBook);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 50;
            Item.damage = 100;
            Item.useTime = 16;
            Item.width = 38;
            Item.height = 38;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<LunarFlareSpawner>();
            Item.buffType = ModContent.BuffType<LunarFlareBuff>();
            Item.shootSpeed = 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarFlareBook);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class LunarFlareBuff : MagusSpellBuff
        {
            protected override int ManaCost => 50;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<LunarFlareSpawner>() };
        }
    }
}