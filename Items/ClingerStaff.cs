using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class ClingerStaff : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.ClingerStaff;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ClingerStaff);
            Item.mana = 50;
            Item.damage = 43;
            Item.useTime = 16;
            Item.useAnimation = 16;
            //Item.width = 48;
            //Item.height = 18;
            Item.autoReuse = false;
            //Item.shoot = ModContent.ProjectileType<ClingerStaffSpawner>();
            Item.buffType = ModContent.BuffType<ClingerStaffBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ClingerStaff);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 originPosition = player.RotatedRelativePoint(player.MountedCenter);
            Vector2 pointerPoisition = Main.MouseWorld;
            while (Collision.CanHitLine(player.position, player.width, player.height, originPosition, 1, 1))
            {
                originPosition.X += velocity.X;
                originPosition.Y += velocity.Y;
                if ((originPosition - pointerPoisition).Length() < 20f + Math.Abs(velocity.X) + Math.Abs(velocity.Y))
                {
                    originPosition = pointerPoisition;
                    break;
                }
            }
            bool notHitTile = false;
            int TileX = (int)originPosition.X / 16;
            int TileY = (int)originPosition.Y / 16;
            int i;
            for (i = TileY; TileY < Main.maxTilesY - 10 && TileY - i < 30 && !WorldGen.SolidTile(TileX, TileY) && !TileID.Sets.Platforms[Main.tile[TileX, TileY].TileType]; TileY++)
            {
            }
            if (!WorldGen.SolidTile(TileX, TileY) && !TileID.Sets.Platforms[Main.tile[TileX, TileY].TileType])
            {
                notHitTile = true;
            }
            float Y = TileY * 16;
            TileY = i;
            while (TileY > 10 && i - TileY < 30 && !WorldGen.SolidTile(TileX, TileY))
            {
                TileY--;
            }
            float YAdjusted = TileY * 16 + 16;
            float Y2 = Y - YAdjusted;
            if (Y2 > (16 * 15))
            {
                Y2 = 16 * 15;
            }
            YAdjusted = Y - Y2;
            if (!notHitTile)
            {
                //Main.NewText(originPosition.X + ", " + YAdjusted);
                int spawnedProjectile = Projectile.NewProjectile(Item.GetSource_ReleaseEntity(), originPosition.X, YAdjusted, 0f, 0f, ModContent.ProjectileType<ClingerStaffSpawner>(), damage, knockback, player.whoAmI, Y2, YAdjusted);
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
            }

            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }
    }

    internal class ClingerStaffSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ClingerStaff;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.ClingerStaff);
            Projectile.hide = true;
            Projectile.alpha = 0;
            buffID = ModContent.BuffType<ClingerStaffBuff>();
            projectileID = ModContent.ProjectileType<ClingerStaffSpawner>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            Projectile.position.Y = Projectile.ai[1];
            Projectile.height = (int)Projectile.ai[0];
            if (Projectile.Center.X > Main.player[Projectile.owner].Center.X)
            {
                Projectile.direction = 1;
            }
            else
            {
                Projectile.direction = -1;
            }

            //Spawn Dust
            float num805 = (float)(Projectile.width * Projectile.height) * 0.0045f;
            for (int i = 0; (float)i < num805; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 100);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
                Main.dust[dust].velocity.Y -= 0.5f;
                Main.dust[dust].scale = 1.4f;
                Main.dust[dust].position.X += 6f;
                Main.dust[dust].position.Y -= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(39, 420);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(39, 420, quiet: false);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }


    internal class ClingerStaffBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<ClingerStaffSpawner>() };
    }
}