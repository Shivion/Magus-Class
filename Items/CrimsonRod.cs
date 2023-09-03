using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace MagusClass.Items
{
    public class CrimsonRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CrimsonRod);
            Item.mana = 25;
            Item.damage = 5;
            Item.shoot = ModContent.ProjectileType<CrimsonRodCloudSeed>();
            Item.buffType = ModContent.BuffType<CrimsonRodBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimsonRod);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);

            Main.projectile[type].ai[0] = (float)Main.mouseX + Main.screenPosition.X;
            Main.projectile[type].ai[1] = (float)Main.mouseY + Main.screenPosition.Y;

            return true;
        }
    }
    internal class CrimsonRodCloudSeed : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {

        }

        public override void AI()
        {
            float x = Projectile.ai[0];
            float y = Projectile.ai[1];
            if (x != 0f && y != 0f)
            {
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
                    //Spawn the projectile here?
                    Projectile.Kill();
                }
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
    }

    internal class CrimsonRodCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {

        }

        public override void AI()
        {
            //duration timer, used to get the oldest projectile
            Projectile.ai[2]++;
            //Kill the older projectile
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VilethornSpawner>()] > 1)
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

            bool colliding = true;
            int centerX = (int)Projectile.Center.X;
            int Y = (int)(Projectile.position.Y + (float)Projectile.height);

            //Fade projectile to kill if ai[1] is 1
            if (Projectile.ai[1] == 1)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
            }

            //check for collision
            if (Collision.SolidTiles(new Vector2((float)centerX, (float)Y), 2, 20))
            {
                colliding = false;
            }
            //animate cloud
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if ((!colliding && Projectile.frame > 2) || Projectile.frame > 5)
                {
                    Projectile.frame = 0;
                }
            }
            //if its colliding summon rain?
            else if (colliding)
            {
                Projectile.ai[0] += 1f;
                if (Projectile.type == 244)
                {
                    if (Projectile.ai[0] > 10f)
                    {
                        Projectile.ai[0] = 0f;
                        if (Projectile.owner == Main.myPlayer)
                        {
                            centerX += Main.rand.Next(-14, 15);
                            Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), centerX, Y, 0f, 5f, 245, Projectile.damage, 0f, Projectile.owner);
                        }
                    }
                }
                else if (Projectile.ai[0] > 8f)
                {
                    Projectile.ai[0] = 0f;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        centerX += Main.rand.Next(-14, 15);
                        Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), centerX, Y, 0f, 5f, 239, Projectile.damage, 0f, Projectile.owner);
                    }
                }
            }
            Projectile.localAI[0] += 1f;
            if (!(Projectile.localAI[0] >= 10f))
            {
                return;
            }
            Projectile.localAI[0] = 0f;


            //kill all projectiles without the buff
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<CrimsonRodBuff>());
            }
            if (!player.HasBuff(ModContent.BuffType<CrimsonRodBuff>()))
            {
                Projectile.ai[1] = 1;
            }
        }
    }

    internal class CrimsonRodBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CrimsonRodCloud>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<CrimsonRodCloudSeed>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                player.statManaMax2 -= player.ownedProjectileCounts[ModContent.ProjectileType<CrimsonRodCloud>()] * 25;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
