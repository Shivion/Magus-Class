using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class MagnetSphere : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MagnetSphere;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MagnetSphere);
            Item.mana = 50;
            Item.damage = 48;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 28;
            Item.height = 30;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<MagnetSphereSpawner>();
            Item.buffType = ModContent.BuffType<MagnetSphereBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MagnetSphere);
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

    internal class MagnetSphereSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MagnetSphereBall;

        private static List<Tuple<int, float>> _MagnetSphereTargetList = new List<Tuple<int, float>>();

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.MagnetSphereBall);
            Projectile.aiStyle = 0;
            Projectile.hide = false;
            buffID = ModContent.BuffType<MagnetSphereBuff>();
            projectileID = ModContent.ProjectileType<MagnetSphereSpawner>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 4)
                {
                    Projectile.frame = 0;
                }
            }

            //slow down near target position
            if (Projectile.velocity.Length() > ((targetPosition - Projectile.Center) / 100).Length())
            {
                Projectile.velocity = ((targetPosition - Projectile.Center) / 100);
            }

            //Get target
            if(Projectile.owner == Main.myPlayer)
            {
                int[] targets = new int[20];
                int targetIndex = 0;
                float num3 = 300f;
                bool hasTarget = false;
                for (int i = 0; i < 200; i++)
                {
                    if (!Main.npc[i].CanBeChasedBy(Projectile))
                    {
                        continue;
                    }
                    float num6 = Main.npc[i].position.X + (Main.npc[i].width / 2);
                    float num7 = Main.npc[i].position.Y + (Main.npc[i].height / 2);
                    if (Math.Abs(Projectile.position.X + (Projectile.width / 2) - num6) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num7) < num3 && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                    {
                        if (targetIndex < 20)
                        {
                            targets[targetIndex] = i;
                            targetIndex++;
                        }
                        hasTarget = true;
                    }
                }
                if (Projectile.timeLeft < 30)
                {
                    hasTarget = false;
                }
                if (hasTarget)
                {
                    //Shoot
                    int randomTargetIndex = Main.rand.Next(targetIndex);
                    randomTargetIndex = targets[randomTargetIndex];
                    float targetX = Main.npc[randomTargetIndex].position.X + (float)(Main.npc[randomTargetIndex].width / 2);
                    float targetY = Main.npc[randomTargetIndex].position.Y + (float)(Main.npc[randomTargetIndex].height / 2);
                    Projectile.localAI[0] += 1f;
                    if (Projectile.localAI[0] > 8f)
                    {
                        Projectile.localAI[0] = 0f;
                        float X = targetX - Projectile.Center.X;
                        float Y = targetY - Projectile.Center.Y;
                        float magnitude = (float)Math.Sqrt(X * X + Y * Y);
                        magnitude = 6f / magnitude;
                        X *= magnitude;
                        Y *= magnitude;
                        Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), Projectile.Center.X, Projectile.Center.Y, X, Y, 255, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
        }
					
    }

    internal class MagnetSphereBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<MagnetSphereSpawner>() };
    }
}