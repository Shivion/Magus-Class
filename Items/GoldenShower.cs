using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class GoldenShower : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.GoldenShower;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenShower);
            Item.autoReuse = false;
            Item.mana = 75;
            Item.damage = 15;
            Item.shoot = ModContent.ProjectileType<GoldenShowerSpawner>();
            Item.buffType = ModContent.BuffType<GoldenShowerBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldenShower);
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

    internal class GoldenShowerSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.GoldenShower;

        public override void SetDefaults()
        {
            base.SetDefaults();
            spawnedProjectileType = ProjectileID.GoldenShowerFriendly;
            buffID = ModContent.BuffType<GoldenShowerBuff>();
            projectileID = ModContent.ProjectileType<GoldenShowerSpawner>();
            coneRadius = 5;
            spawnInterval = 6f;
            sound = SoundID.Item13;
            doSpin = false;
            horizontalSprite = true;
        }
    }

    internal class GoldenShowerBuff : MagusSpellBuff
    {
        protected override int ManaCost => 75;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<GoldenShowerSpawner>() };
    }
}