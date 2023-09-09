using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace MagusClass.Items
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
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GravitasProjectile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient( ItemID.Wood, 1 )
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            // Stops us from spawning more than 3 projectile.
            return player.ownedProjectileCounts[Item.shoot] < 3;
        }
    }


    public class GravitasProjectile : ModProjectile
    {
        private int maxRadius = 250;        // Maximum radius
        private int growthRate = 40;        // Rate of growth

        private bool spawned = false;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Main.myPlayer == Projectile.owner && !spawned)
            {
                Vector2 cursorPos = Main.MouseWorld;
                player.LimitPointToPlayerReachableArea(ref cursorPos); // Dont do what i thought it do

                Projectile.Center = cursorPos;

                spawned = true;
            }

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 90f)
            {
                Projectile.width += growthRate;
                Projectile.height += growthRate;

                Projectile.position.X -= (float)growthRate / 2;
                Projectile.position.Y -= (float)growthRate / 2;

                SoundEngine.PlaySound(SoundID.Item100, Projectile.position);

                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
            }

            // Create a dust circle
            for (int i = 0; i < 360; i += 10)
            {
                float rads = MathHelper.ToRadians(i);

                Vector2 offset = new Vector2(
                    (float)System.Math.Cos(rads),
                    (float)System.Math.Sin(rads)) * (Projectile.width / 2);

                Dust.NewDustPerfect(Projectile.Center + offset, DustID.Shadowflame, null, 0, default, 0.5f);
            }

            // Kill Projectile once we reach our max size.
            if (Projectile.width >= maxRadius) Projectile.Kill();
        }
    }
}
