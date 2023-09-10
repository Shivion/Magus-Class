using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    public class MedusaHead : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MedusaHead;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MedusaHead);
            Item.mana = 50;
            Item.damage = 40;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.width = 32;
            Item.height = 18;
            Item.color = Color.Red;
            Item.autoReuse = false;
            Item.UseSound = SoundID.ScaryScream;
            Item.shoot = ModContent.ProjectileType<MedusaHeadSpawner>();
            Item.buffType = ModContent.BuffType<MedusaHeadBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MedusaHead);
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

    internal class MedusaHeadSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MedusaHead;

        private static List<Tuple<int, float>> _medusaHeadTargetList = new List<Tuple<int, float>>();

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.MedusaHead);
            Projectile.aiStyle = 0;
            Projectile.hide = false;
            buffID = ModContent.BuffType<MedusaHeadBuff>();
            projectileID = ModContent.ProjectileType<MedusaHeadSpawner>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            bool flag2 = Projectile.ai[0] > 0f;

            if (Projectile.ai[0] > 0f)
            {
                Projectile.ai[0] -= 1f;

                if (Projectile.frame < 8)
                {
                    Projectile.frame = 8;
                }
                if (Projectile.frame >= 12)
                {
                    Projectile.frame = 8;
                }
                Projectile.frameCounter++;
                if (++Projectile.frameCounter >= 5)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= 12)
                    {
                        Projectile.frame = 8;
                    }
                }
            }
            else if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 8)
                {
                    Projectile.frame = 0;
                }
            }

            if (flag2 && Projectile.soundDelay == 0)
            {
                //SoundEngine.PlaySound(4, Projectile.position, 17);
                SoundEngine.PlaySound(SoundID.NPCDeath17, Projectile.position);
            }
            Projectile.soundDelay = (flag2 ? 4 : 0);
            if (Main.myPlayer == Projectile.owner)
            {
                Vector2 position = Projectile.Center;
                if (!flag2 || Projectile.ai[0] % 15f == 0f)
                {
                    bool flag3 = false;
                    for (int i = 0; i < 200; i++)
                    {
                        NPC nPC = Main.npc[i];
                        if (nPC.active && Projectile.Distance(nPC.Center) < 320f && nPC.CanBeChasedBy(Projectile) && Collision.CanHitLine(nPC.position, nPC.width, nPC.height, position, 0, 0))
                        {
                            flag3 = true;
                            break;
                        }
                    }
                    if (flag3)
                    {
                        int num = Projectile.damage;
                        position = Projectile.Center;
                        int num2 = 0;
                        float num3 = 0f;
                        _medusaHeadTargetList.Clear();
                        for (int j = 0; j < 200; j++)
                        {
                            NPC nPC2 = Main.npc[j];
                            float num4 = Projectile.Distance(nPC2.Center);
                            if (nPC2.active && num4 < 320f && nPC2.CanBeChasedBy(Projectile) && Collision.CanHitLine(nPC2.position, nPC2.width, nPC2.height, position, 0, 0))
                            {
                                _medusaHeadTargetList.Add(Tuple.Create(j, num4));
                            }
                        }
                        //private static NPCDistanceByIndexComparator _medusaTargetComparer = new NPCDistanceByIndexComparator();
                        //_medusaHeadTargetList.Sort(_medusaTargetComparer);

                        for (int k = 0; k < _medusaHeadTargetList.Count && k < 3; k++)
                        {
                            Tuple<int, float> tuple = _medusaHeadTargetList[k];
                            NPC nPC3 = Main.npc[tuple.Item1];
                            Vector2 v = nPC3.Center - position;
                            num3 += v.ToRotation();
                            num2++;
                            int num5 = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), position.X, position.Y, v.X, v.Y, ModContent.ProjectileType<MedusaHeadRay>(), 0, 0f, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[num5].Center = nPC3.Center;
                            Main.projectile[num5].damage = num;
                            Main.projectile[num5].Damage();
                            Main.projectile[num5].damage = 0;
                            Main.projectile[num5].Center = position;
                            Projectile.ai[0] = 180f;
                        }
                        num3 = (num2 != 0) ? (num3 / num2) : ((float)0f);
                        for (int l = 0; l < 4; l++)
                        {
                            Vector2 randomDirection = ((Main.rand.NextBool(4)) ? (Vector2.UnitX.RotatedByRandom(Math.PI*2) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f)) : (Vector2.UnitX.RotatedByRandom(Math.PI).RotatedBy(num3) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f)));
                            Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), position.X, position.Y, randomDirection.X, randomDirection.Y, ModContent.ProjectileType<MedusaHeadRay>(), 0, 0f, Projectile.owner, Projectile.whoAmI);
                        }
                        Projectile.ai[0] = 60f;
                    }
                }
            }
            Lighting.AddLight(Projectile.Center, 0.9f, 0.75f, 0.1f);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
    internal class MedusaHeadRay : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MedusaHeadRay;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.MedusaHeadRay);
            //DrawOffsetX = -100;
            //DrawOriginOffsetX = 50;
            DrawOriginOffsetY = -90;
            Projectile.aiStyle = 0;
            Projectile.hide = false;
        }

        public override void AI()
        {
            float num887 = 20f;
            Projectile.localAI[0]++;
            Projectile.alpha = (int)MathHelper.Lerp(0f, 255f, Projectile.localAI[0] / num887);
            int parentProjectile = (int)Projectile.ai[0];
            if (Projectile.localAI[0] >= num887 || parentProjectile < 0 || parentProjectile > 1000 || !Main.projectile[parentProjectile].active)
            {
                Projectile.Kill();
                return;
            }
            Projectile.scale = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y) * 0.009f;
            Projectile.Center = Main.projectile[parentProjectile].Center + new Vector2(Main.projectile[parentProjectile].width, Main.projectile[parentProjectile].height);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = 90;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 63 - Projectile.alpha / 4);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }

    internal class MedusaHeadBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<MedusaHeadSpawner>() };
    }
}