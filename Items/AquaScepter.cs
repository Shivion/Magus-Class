using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class AquaScepter : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AquaScepter);
            Item.mana = 50;
            Item.damage = 15;
            Item.useTime = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<AquaScepterSpawner>();
            Item.buffType = ModContent.BuffType<AquaScepterBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AquaScepter);
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

    internal class AquaScepterSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.AquaScepter;

        Vector2 targetPosition;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.velocity = Vector2.Zero;
            //AquaScepters cast count
            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
            base.AI();
            KillExistingProjectiles();

            bool reachedX = false;
            bool reachedY = false;

            //Move to mouse location on the first frame
            if (Projectile.ai[2] == 1)
            {
                targetPosition = Main.MouseWorld;
            }
            else
            {
                if (Projectile.velocity.X == 0f || (Projectile.velocity.X < 0f && Projectile.Center.X < targetPosition.X) || (Projectile.velocity.X > 0f && Projectile.Center.X > targetPosition.X))
                {
                    Projectile.velocity.X = 0f;
                    reachedX = true;
                }
                if (Projectile.velocity.Y == 0f || (Projectile.velocity.Y < 0f && Projectile.Center.Y < targetPosition.Y) || (Projectile.velocity.Y > 0f && Projectile.Center.Y > targetPosition.Y))
                {
                    Projectile.velocity.Y = 0f;
                    reachedY = true;
                }

            }

            if (reachedX && reachedY)
            {
                if (Projectile.ai[1] == 0 && Projectile.ai[0] > 5f)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        float farX = Projectile.position.X + Projectile.width;
                        float centerY = Projectile.Center.Y;
                        Vector2 sprayVelocity = new Vector2(1, 0);
                        sprayVelocity = sprayVelocity.RotatedBy(Projectile.rotation);
                        sprayVelocity.Normalize();
                        sprayVelocity = sprayVelocity * 10;
                        int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), farX, centerY, sprayVelocity.X, sprayVelocity.Y, ProjectileID.WaterStream, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                        SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
                    }
                    Projectile.ai[0] = Main.rand.Next(0, 2);
                }
                Projectile.ai[0]++;

                Projectile.rotation += 0.1f * Projectile.direction;
                Projectile.spriteDirection = 180;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.position += Projectile.velocity;
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }

    internal class AquaScepterBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<AquaScepterSpawner>() };
    }
}