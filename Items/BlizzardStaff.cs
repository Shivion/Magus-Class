using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class BlizzardStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BlizzardStaff);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 75;
            Item.damage = 58;
            Item.useTime = 16;
            Item.width = 62;
            Item.height = 62;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<BlizzardStaffSpawner>();
            Item.buffType = ModContent.BuffType<BlizzardStaffBuff>();
            Item.shootSpeed = 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BlizzardStaff);
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
    internal class BlizzardStaffSpawner : CallDownSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BlizzardStaff;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 58;
            Projectile.height = 58;
            buffID = ModContent.BuffType<BlizzardStaffBuff>();
            projectileID = ModContent.ProjectileType<BlizzardStaffSpawner>();
            possibleProjectiles = new int[] { ProjectileID.Blizzard };
            sound = SoundID.Item28;
        }
    }
    internal class BlizzardStaffBuff : MagusSpellBuff
    {
        protected override int ManaCost => 75;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<BlizzardStaffSpawner>() };
    }
}