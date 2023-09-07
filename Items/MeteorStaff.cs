using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class MeteorStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MeteorStaff);
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 50;
            Item.damage = 50;
            Item.useTime = 16;
            Item.width = 44;
            Item.height = 43;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<MeteorStaffSpawner>();
            Item.buffType = ModContent.BuffType<MeteorStaffBuff>();
            Item.shootSpeed = 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteorStaff);
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

    internal class MeteorStaffSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MeteorStaff;

        Vector2 targetPosition;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.width = 44;
            Projectile.height = 42;
            buffID = ModContent.BuffType<MeteorStaffBuff>();
            projectileID = ModContent.ProjectileType<MeteorStaffSpawner>();
        }

        public override void AI()
        {
            base.AI();
            KillExistingProjectiles();

            if (Thrown())
            {
                if (Projectile.ai[1] == 0 && Projectile.ai[0] > 5f)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        //Setup
                        Vector2 startPoisition = Projectile.Center;
                        Vector2 targetPosition = Projectile.Center;
                        float speed = 5;

                        //Assign starting position
                        startPoisition.X = (startPoisition.X + Projectile.Center.X) / 2f + (float)Main.rand.Next(-400, 401);
                        startPoisition.Y -= 500;

                        //assign velocity
                        Vector2 velocity = targetPosition - startPoisition;
                        float magnitude = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);

                        //Skew the X velocity
                        velocity.X = Projectile.Center.X - startPoisition.X + (float)Main.rand.Next(-40, 41) * 0.03f;

                        //ensure the projectile is falling at a minimum speed
                        if (velocity.Y < 0f)
                        {
                            velocity.Y *= -1f;
                        }
                        if (velocity.Y < 20f)
                        {
                            velocity.Y = 20f;
                        }

                        //then add the magnitude?
                        magnitude = speed / magnitude;
                        velocity *= magnitude;

                        //Skew Y
                        velocity.Y += (float)Main.rand.Next(-40, 41) * 0.02f;

                        //launch projectile
                        int[] projectile = new int[] { ProjectileID.Meteor1, ProjectileID.Meteor2, ProjectileID.Meteor3 };
                        int newProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), startPoisition.X, startPoisition.Y, velocity.X * 0.75f, velocity.Y * 0.75f, Main.rand.NextFromList(projectile), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, (float)Main.rand.NextDouble() * 0.3f);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, newProjectile);
                        SoundEngine.PlaySound(SoundID.Item88, Projectile.position);
                    }
                    Projectile.ai[0] = Main.rand.Next(0, 2);
                }
                Projectile.ai[0]++;

                //try spinning, thats a neat trick
                Projectile.rotation += 0.1f * Projectile.direction;
                Projectile.spriteDirection = 180;
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }

    internal class MeteorStaffBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<MeteorStaffSpawner>() };
    }
}