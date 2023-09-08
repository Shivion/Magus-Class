using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class InfernoFork : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.InfernoFork);
            Item.mana = 50;
            Item.damage = 70;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 52;
            Item.height = 54;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<InfernoForkSpawner>();
            Item.buffType = ModContent.BuffType<InfernoForkBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.InfernoFork);
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

    internal class InfernoForkSpawner : MagusProjectile
    {
        bool isStuck;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.friendly = true; 
            DrawOffsetX = -52;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 17;
            Projectile.width = 4;
            Projectile.height = 5;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            buffID = ModContent.BuffType<InfernoForkBuff>();
            projectileID = ModContent.ProjectileType<InfernoForkSpawner>();
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.position += new Vector2(0, -25f);
            Projectile.velocity *= 2;
            base.OnSpawn(source);
        }

        public override void AI()
        {
            base.AI();

            if (!isStuck)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.position += Projectile.velocity;
                if (Projectile.ai[2] > 10f)
                {
                    Projectile.velocity += new Vector2(0, 0.1f);
                }
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }

                //isStuck = false;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;

                if (Projectile.ai[1] == 0 && Projectile.ai[0] == 0)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        float farX = Projectile.position.X + Projectile.width;
                        float centerY = Projectile.Center.Y;
                        Vector2 sprayVelocity = new Vector2(1, 0);
                        sprayVelocity = sprayVelocity.RotatedBy(Projectile.rotation);
                        int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), farX, centerY, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.InfernoFriendlyBlast, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                        SoundEngine.PlaySound(SoundID.Item73, Projectile.position);
                    }
                    Projectile.ai[0] = 160 + Main.rand.Next(0, 2);
                }
                Projectile.ai[0]--;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            isStuck = true;
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }


    internal class InfernoForkBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => true;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<InfernoForkSpawner>() };
    }
}