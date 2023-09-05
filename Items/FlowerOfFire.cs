using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class FlowerOfFire : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FlowerofFire);
            Item.mana = 50;
            Item.damage = 10;
            Item.shoot = ModContent.ProjectileType<FlowerOfFireSpawner>();
            Item.buffType = ModContent.BuffType<FlowerOfFireBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FlowerofFire);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }
        internal class FlowerOfFireSpawner : ModProjectile
        {
            private float fallSpeed = 0;
            private float fallspeed;

            public override void SetStaticDefaults()
            {

            }

            public override void SetDefaults()
            {
                Projectile.penetrate = -1;
                Projectile.aiStyle = 0;
                Projectile.velocity = Vector2.Zero;
            }

            public override void OnSpawn(IEntitySource source)
            {
                Player player = Main.player[Projectile.owner];
                Projectile.position = ShivUtilities.FindRestingSpot(player.position) - new Vector2(0, Projectile.height + 10);
                base.OnSpawn(source);
            }

            public override void AI()
            {
                //duration timer, used to get the oldest projectile
                Projectile.ai[2]++;
                //Kill the older projectile
                Player player = Main.player[Projectile.owner];
                if (player.ownedProjectileCounts[ModContent.ProjectileType<FlowerOfFireSpawner>()] > 1)
                {
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type && Main.projectile[i].ai[1] < 1)
                        {
                            if (Main.projectile[i].ai[2] > Projectile.ai[2])
                            {
                                Main.projectile[i].ai[1] = 1;
                            }
                        }
                    }
                }

                //Kill all projectiles without the buff
                if (player.dead || !player.active)
                {
                    player.ClearBuff(ModContent.BuffType<FlowerOfFireBuff>());
                }
                if (!player.HasBuff(ModContent.BuffType<FlowerOfFireBuff>()))
                {
                    Projectile.ai[1] = 1;
                }

                if (Projectile.ai[1] == 1)
                {
                    Projectile.alpha += 5;
                    if (Projectile.alpha > 255)
                    {
                        Projectile.alpha = 255;
                        Projectile.Kill();
                    }
                }

                if (Projectile.ai[1] == 0 && Projectile.ai[0] > 60f)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        float farX = Projectile.Center.X;
                        float centerY = Projectile.Center.Y;

                        int offset = Main.rand.Next(-15, 16);
                        Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(MathHelper.ToRadians(offset));
                        int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), farX, centerY, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.BallofFire, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                        SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                    }
                    Projectile.ai[0] = 0;
                }
                Projectile.ai[0]++;

                //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); ; 
                Projectile.spriteDirection = Projectile.direction;
            }

            public override bool ShouldUpdatePosition()
            {
                return false;
            }
        }

        internal class FlowerOfFireBuff : MagusSpellBuff
        {
            protected override int ManaCost => 50;
            protected override bool MultipleSpellsAllowed => false;
            protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<FlowerOfFireSpawner>() };
        }
    }
}