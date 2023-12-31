﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.CursedFlames
{
    public class CursedFlames : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CursedFlames);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.autoReuse = false;
            Item.mana = 50;
            Item.damage = 55;
            Item.width = 37;
            Item.height = 33;
            Item.shoot = ModContent.ProjectileType<CursedFlamesSpawner>();
            Item.buffType = ModContent.BuffType<CursedFlamesBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CursedFlames);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class CursedFlamesBuff : MagusSpellBuff
        {
            protected override int ManaCost => 50;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<CursedFlamesSpawner>() };
        }
    }
}