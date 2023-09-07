using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class NettleBurst : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.NettleBurst);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 50;
            Item.height = 46;
            Item.mana = 75;
            Item.damage = 30;
            Item.shoot = ModContent.ProjectileType<NettleBurstSpawner>();
            Item.buffType = ModContent.BuffType<NettleBurstBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.NettleBurst);
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

    internal class NettleBurstSpawner : VilethornishSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.NettleBurst;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 46;
            Projectile.height = 42;
            spawnedProjectileType = ProjectileID.NettleBurstRight;
            buffID = ModContent.BuffType<NettleBurstBuff>();
            projectileID = ModContent.ProjectileType<NettleBurstSpawner>();
        }
    }

    internal class NettleBurstBuff : MagusSpellBuff
    {
        protected override int ManaCost => 75;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<NettleBurstSpawner>() };
    }
}