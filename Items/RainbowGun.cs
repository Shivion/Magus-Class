using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
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
            Item.mana = 50;
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
    }

    internal class RainbowGunFront : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RainbowFront;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RainbowFront);
            base.SetDefaults();
            Projectile.alpha = 255;
            buffID = ModContent.BuffType<RainbowGunBuff>();
            projectileID = ModContent.ProjectileType<RainbowGunFront>();
        }

        public override void AI()
        {
            base.AI();
            KillExistingProjectiles();

            int maxTimeLeft = 5000;
            Point point2 = Projectile.Center.ToTileCoordinates();
            if (!WorldGen.InWorld(point2.X, point2.Y, 2) || Main.tile[point2.X, point2.Y] == null)
            {
                Projectile.Kill();
                return;
            }
            float YVelocityBuffer = 1f;
            if (Projectile.velocity.Y < 0f)
            {
                YVelocityBuffer -= Projectile.velocity.Y / 3f;
            }
            Projectile.ai[0] += YVelocityBuffer;
            if (Projectile.ai[0] > 30f)
            {
                Projectile.velocity.Y += 0.4f;
                if (Projectile.velocity.Y > 0f)
                {
                    Projectile.velocity.X *= 0.95f;
                }
                else
                {
                    Projectile.velocity.X *= 1.05f;
                }
            }

            float magnitude = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y);
            magnitude = 15.95f * Projectile.scale / magnitude;
            Projectile.velocity *= magnitude;

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.ToRadians(90);

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[0] += 1f;
                if (Projectile.localAI[0] > 4f)
                {
                    Projectile.localAI[0] = 3f;
                    Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0.001f, Projectile.velocity.Y * 0.001f, ModContent.ProjectileType<RainbowGunBack>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                if (Projectile.timeLeft > maxTimeLeft)
                {
                    Projectile.timeLeft = maxTimeLeft;
                }
            }
        }
    }

    internal class RainbowGunBack : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RainbowBack;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RainbowBack);
            base.SetDefaults();
            Projectile.alpha = 255;
            Projectile.light = 0;
            buffID = ModContent.BuffType<RainbowGunBuff>();
            projectileID = ModContent.ProjectileType<RainbowGunBack>();
        }

        public override void AI()
        {
            base.AI();

            float num = (float)Main.DiscoR / 255f;
            float num2 = (float)Main.DiscoG / 255f;
            float num3 = (float)Main.DiscoB / 255f;
            num = (num + 1f) / 2f;
            num2 = (num2 + 1f) / 2f;
            num3 = (num3 + 1f) / 2f;
            num *= 0.5f;
            num2 *= 0.5f;
            num3 *= 0.5f;

            Lighting.AddLight(Projectile.Center, num, num2, num3);

            if (Projectile.localAI[0] == 0f)
            {
                if (Projectile.velocity.X > 0f)
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.ToRadians(90);//1.57f;

                }
                else
                {
                    Projectile.spriteDirection = 1;
                    Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.ToRadians(90);//1.57f;
                }
                Projectile.localAI[0] = 1f;
            }

            if (Projectile.ai[1] != 1 && Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            //return new Color((255 - Projectile.alpha) * 2, (255 - Projectile.alpha) * 2, (255 - Projectile.alpha) * 2, 255 - Projectile.alpha);
            int r = 255 - Projectile.alpha;
            int g = 255 - Projectile.alpha;
            int b = 255 - Projectile.alpha;
            return new Color(r, g, b, 0);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }




    internal class RainbowGunBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<RainbowGunFront>(), ModContent.ProjectileType<RainbowGunBack>() };
    }
}