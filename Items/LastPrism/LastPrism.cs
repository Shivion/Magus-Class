using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.LastPrism
{
    /// <summary>
    /// Heavily adapted from example mod
    /// </summary>
    public class LastPrism : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LastPrism;
        public static Color OverrideColor = new(122, 173, 255);

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LastPrism);
            Item.mana = 200;
            Item.damage = 100;
            Item.useTime = 16;
            Item.useAnimation = 16;
            //Item.width = 16;
            //Item.height = 16;
            Item.autoReuse = false;
            Item.channel = false;
            //Item.shoot = ModContent.ProjectileType<LastPrismCannonSpawner>();
            Item.buffType = ModContent.BuffType<LastPrismBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LastPrism);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int spawnedProjectile = Projectile.NewProjectile(Item.GetSource_ReleaseEntity(), Main.MouseWorld.X, Main.MouseWorld.Y - 750f, 0f, 1f, ModContent.ProjectileType<LastPrismHoldout>(), damage, knockback, player.whoAmI);
            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);

            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }
        internal class LastPrismBuff : MagusSpellBuff
        {
            protected override int ManaCost => 200;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<LastPrismHoldout>() };
        }
    }
}