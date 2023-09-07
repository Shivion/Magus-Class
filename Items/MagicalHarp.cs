using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class MagicalHarp : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MagicalHarp;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MagicalHarp);
            Item.width = 28;
            Item.height = 38;
            Item.autoReuse = false;
            Item.mana = 50;
            Item.damage = 42;
            Item.shoot = ModContent.ProjectileType<MagicalHarpSpawner>();
            Item.buffType = ModContent.BuffType<MagicalHarpBuff>();

            Item.staff[ModContent.ItemType<MagicalHarp>()] = false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 0f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MagicalHarp);
            recipe.AddIngredient(ItemID.FragmentNebula);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }
    }

    internal class MagicalHarpSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MagicalHarp;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 30;
            spawnedProjectileType = ProjectileID.EighthNote;
            buffID = ModContent.BuffType<MagicalHarpBuff>();
            projectileID = ModContent.ProjectileType<MagicalHarpSpawner>();
            coneRadius = 15;
            spawnInterval = 6f;
            sound = SoundID.Item26;
            doSpin = false;
            horizontalSprite = true;
        }
    }

    internal class MagicalHarpBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<MagicalHarpSpawner>() };
    }
}