using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class NimbusRod : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.NimbusRod;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.NimbusRod);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 25;
            Item.damage = 30;
            Item.width = 46;
            Item.height = 46;
            Item.shoot = ModContent.ProjectileType<NimbusRodCloudSeed>();
            Item.buffType = ModContent.BuffType<NimbusRodBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.NimbusRod);
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


    internal class NimbusRodCloudSeed : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RainCloudMoving;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RainCloudMoving);
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = 0;
            AIType = ProjectileID.None;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = Main.mouseX + Main.screenPosition.X;
            Projectile.ai[1] = Main.mouseY + Main.screenPosition.Y;
            base.OnSpawn(source);
        }

        public override void AI()
        {
            float x = Projectile.ai[0];
            float y = Projectile.ai[1];

            bool reachedX = false;
            bool reachedY = false;
            if (Projectile.velocity.X == 0f || (Projectile.velocity.X < 0f && Projectile.Center.X < x) || (Projectile.velocity.X > 0f && Projectile.Center.X > x))
            {
                Projectile.velocity.X = 0f;
                reachedX = true;
            }
            if (Projectile.velocity.Y == 0f || (Projectile.velocity.Y < 0f && Projectile.Center.Y < y) || (Projectile.velocity.Y > 0f && Projectile.Center.Y > y))
            {
                Projectile.velocity.Y = 0f;
                reachedY = true;
            }
            if (Projectile.owner == Main.myPlayer && reachedX && reachedY)
            {
                Projectile.Kill();
            }

            Projectile.rotation += Projectile.velocity.X * 0.02f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                int newProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<NimbusRodCloud>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, newProjectile);
            }
        }
    }

    internal class NimbusRodCloud : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RainCloudRaining;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RainCloudRaining);
            Projectile.aiStyle = 0;
            Projectile.width = 54;
            Projectile.height = 24;
            buffID = ModContent.BuffType<NimbusRodBuff>();
            projectileID = ModContent.ProjectileType<NimbusRodCloud>();
        }

        public override void AI()
        {
            base.AI();

            bool notColliding = true;
            int centerX = (int)Projectile.Center.X;
            int BottomY = (int)(Projectile.position.Y + (float)Projectile.height);

            //check for collision
            if (Collision.SolidTiles(new Vector2((float)centerX, (float)BottomY), 2, 20))
            {
                notColliding = false;
            }

            //animate cloud
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if ((!notColliding && Projectile.frame > 2) || Projectile.frame > 5)
                {
                    Projectile.frame = 0;
                }
            }

            //if its not colliding summon rain
            else if (notColliding)
            {
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] > 10f)
                {
                    Projectile.ai[0] = 0f;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        centerX += Main.rand.Next(-14, 15);
                        Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), centerX, BottomY, 0f, 5f, ProjectileID.RainFriendly, Projectile.damage, 0f, Projectile.owner);
                    }
                }
            }
            Projectile.localAI[0] += 1f;
            if (!(Projectile.localAI[0] >= 10f))
            {
                return;
            }
            Projectile.localAI[0] = 0f;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }

    internal class NimbusRodBuff : MagusSpellBuff
    {
        protected override int ManaCost => 25;
        protected override bool MultipleSpellsAllowed => true;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<NimbusRodCloudSeed>(), ModContent.ProjectileType<NimbusRodCloud>() };
    }
}
