using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.RainbowGun
{
    public class RainbowGun : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.RainbowGun;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.RainbowGun);
            Item.mana = 25;
            Item.damage = 45;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 56;
            Item.height = 30;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<RainbowGunFront>();
            Item.buffType = ModContent.BuffType<RainbowGunBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RainbowGun);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Kill Existing Rainbow
            if (player.ownedProjectileCounts[ModContent.ProjectileType<RainbowGunBack>()] > 1)
            {
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && (Main.projectile[i].type == ModContent.ProjectileType<RainbowGunBack>() || Main.projectile[i].type == ModContent.ProjectileType<RainbowGunFront>()) && Main.projectile[i].ai[1] < 1)
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }
        internal class RainbowGunBuff : MagusSpellBuff
        {
            protected override int ManaCost => 25;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<RainbowGunFront>(), ModContent.ProjectileType<RainbowGunBack>() };
        }
    }
}