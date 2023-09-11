using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class RazorbladeTyphoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.RazorbladeTyphoon);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 31;
            Item.height = 30;
            Item.autoReuse = false;
            Item.mana = 200;
            Item.damage = 85;
            Item.shoot = ModContent.ProjectileType<RazorbladeTyphoonSpawner>();
            Item.buffType = ModContent.BuffType<RazorbladeTyphoonBuff>();
            Item.shootSpeed = 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RazorbladeTyphoon);
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

    internal class RazorbladeTyphoonSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.RazorbladeTyphoon;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 30;
            spawnedProjectileType = ProjectileID.Typhoon;
            buffID = ModContent.BuffType<RazorbladeTyphoonBuff>();
            projectileID = ModContent.ProjectileType<RazorbladeTyphoonSpawner>();
            coneRadius = 180;
            spawnInterval = 6f;
            sound = SoundID.Item84;
            doSpin = true;
            thrown = true;
        }
    }

    internal class RazorbladeTyphoonBuff : MagusSpellBuff
    {
        protected override int ManaCost => 200;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<RazorbladeTyphoonSpawner>() };
    }
}