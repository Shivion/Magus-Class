﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.MedusaHead
{
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
            buffID = ModContent.BuffType<MedusaHead.MedusaHeadBuff>();
            projectileID = ModContent.ProjectileType<MedusaHeadSpawner>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            //Adjust the position a bit
            if(TimeActive == 1)
            {
                Projectile.position -= new Vector2(Projectile.width, Projectile.height);
            }

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
            Projectile.soundDelay = flag2 ? 4 : 0;
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
                        num3 = num2 != 0 ? num3 / num2 : (float)0f;
                        for (int l = 0; l < 4; l++)
                        {
                            Vector2 randomDirection = Main.rand.NextBool(4) ? Vector2.UnitX.RotatedByRandom(Math.PI * 2) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f) : Vector2.UnitX.RotatedByRandom(Math.PI).RotatedBy(num3) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f);
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
}