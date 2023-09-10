using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class MeteorStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MeteorStaff);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 50;
            Item.damage = 50;
            Item.useTime = 16;
            Item.width = 44;
            Item.height = 43;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<MeteorStaffSpawner>();
            Item.buffType = ModContent.BuffType<MeteorStaffBuff>();
            Item.shootSpeed = 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteorStaff);
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
    internal class MeteorStaffSpawner : CallDownSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MeteorStaff;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 44;
            Projectile.height = 42;
            buffID = ModContent.BuffType<MeteorStaffBuff>();
            projectileID = ModContent.ProjectileType<MeteorStaffSpawner>();
            possibleProjectiles = new int[] { ProjectileID.Meteor1, ProjectileID.Meteor2, ProjectileID.Meteor3 };
            sound = SoundID.Item88;
        }
    }
    internal class MeteorStaffBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<MeteorStaffSpawner>() };
    }
}