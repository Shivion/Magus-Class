using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.ChargedBlasterCannon
{
    public class ChargedBlasterCannon : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.ChargedBlasterCannon;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ChargedBlasterCannon);
            Item.mana = 50;
            Item.damage = 100;
            Item.useTime = 16;
            Item.useAnimation = 16;
            //Item.width = 16;
            //Item.height = 16;
            Item.autoReuse = false;
            Item.channel = false;
            //Item.shoot = ModContent.ProjectileType<ChargedBlasterCannonSpawner>();
            Item.buffType = ModContent.BuffType<ChargedBlasterCannonBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ChargedBlasterCannon);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int spawnedProjectile = Projectile.NewProjectile(Item.GetSource_ReleaseEntity(), Main.MouseWorld.X, Main.MouseWorld.Y - 750f, 0f, 1, ModContent.ProjectileType<ChargedBlasterCannonHoldout>(), damage, knockback, player.whoAmI);
            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);

            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }

        internal class ChargedBlasterCannonBuff : MagusSpellBuff
        {
            protected override int ManaCost => 50;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<ChargedBlasterCannonHoldout>() };
        }
    }
}