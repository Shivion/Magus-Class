using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace MagusClass.Items.GravitasStaff
{
    public class GravitasStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.mana = 25;
            Item.knockBack = 10f;

            Item.width = 48;
            Item.height = 48;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GravitasProjectile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofFright, 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            // Stops us from spawning more than 3 projectile.
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}
