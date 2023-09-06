using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class BubbleGun : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BubbleGun;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BubbleGun);
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 75;
            Item.damage = 70;
            Item.shoot = ModContent.ProjectileType<BubbleGunSpawner>();
            Item.buffType = ModContent.BuffType<BubbleGunBuff>();

            Item.staff[ModContent.ItemType<BubbleGun>()] = false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-30f, -5f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BubbleGun);
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

    internal class BubbleGunSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BubbleGun;

        public override void SetDefaults()
        {
            base.SetDefaults();
            spawnedProjectileType = ProjectileID.Bubble;
            buffID = ModContent.BuffType<BubbleGunBuff>();
            projectileID = ModContent.ProjectileType<BubbleGunSpawner>();
            coneRadius = 15;
            spawnInterval = 10f;
            sound = SoundID.Item85;
            doSpin = false;
            horizontalSprite = true;
        }
    }

    internal class BubbleGunBuff : MagusSpellBuff
    {
        protected override int ManaCost => 75;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<BubbleGunSpawner>() };
    }
}