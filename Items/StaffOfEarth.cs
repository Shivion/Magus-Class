using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class StaffOfEarth : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StaffofEarth);
            Item.width = 49;
            Item.height = 49;
            Item.autoReuse = false;
            Item.mana = 100;
            Item.damage = 100;
            Item.shoot = ModContent.ProjectileType<StaffOfEarthSpawner>();
            Item.buffType = ModContent.BuffType<StaffOfEarthBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.StaffofEarth);
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

    internal class StaffOfEarthSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.StaffofEarth;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 42;
            Projectile.height = 42;
            spawnedProjectileType = ProjectileID.BoulderStaffOfEarth;
            buffID = ModContent.BuffType<StaffOfEarthBuff>();
            projectileID = ModContent.ProjectileType<StaffOfEarthSpawner>();
            coneRadius = 15;
            spawnInterval = 60f;
            sound = SoundID.Item69;
            doSpin = true;
        }
    }

    internal class StaffOfEarthBuff : MagusSpellBuff
    {
        protected override int ManaCost => 100;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<StaffOfEarthSpawner>() };
    }
}