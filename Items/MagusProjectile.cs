using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal abstract class MagusProjectile : ModProjectile
    {
        protected int buffID;
        protected int projectileID;

        protected Vector2 targetPosition;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            Projectile.localAI[2]++;

            if (Projectile.owner == Main.myPlayer)
            {
                Player player = Main.player[Projectile.owner];
                //Kill all projectiles without the buff
                if (player.dead || !player.active)
                {
                    player.ClearBuff(buffID);
                }
                if (!player.HasBuff(buffID))
                {
                    Projectile.ai[2] = 1;
                }
            }

            if (Projectile.ai[2] == 1)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha >= 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
                return;
            }
            else
            {
                Projectile.timeLeft = 3600;
            }

            //Capture mouse location on the first frame
            if (Projectile.owner == Main.myPlayer && Projectile.localAI[2] == 1)
            {
                targetPosition = Main.MouseWorld;
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
            }
        }

        public void KillExistingProjectiles()
        {
            //Kill the older projectile
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[projectileID] > 1)
            {
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type && Main.projectile[i].ai[2] < 1)
                    {
                        if (Main.projectile[i].localAI[2] > Projectile.localAI[2])
                        {
                            Main.projectile[i].ai[2] = 1;
                        }
                    }
                }
            }
        }

        public bool Thrown(float speedModifier = 1, bool doRotation = true)
        {
            bool reachedX = false;
            bool reachedY = false;

            //Move to mouse location and stop
            if (Projectile.velocity.X == 0f || (Projectile.velocity.X < 0f && Projectile.Center.X < targetPosition.X) || (Projectile.velocity.X > 0f && Projectile.Center.X > targetPosition.X))
            {
                reachedX = true;
            }
            else
            {
                Projectile.position.X += Projectile.velocity.X * speedModifier;
            }
            if (Projectile.velocity.Y == 0f || (Projectile.velocity.Y < 0f && Projectile.Center.Y < targetPosition.Y) || (Projectile.velocity.Y > 0f && Projectile.Center.Y > targetPosition.Y))
            {
                reachedY = true;
            }
            else
            {
                Projectile.position.Y += Projectile.velocity.Y * speedModifier;
            }

            if (reachedX && reachedY)
            {
                return true;
            }
            else if (doRotation)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WritePackedVector2(targetPosition);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetPosition = reader.ReadPackedVector2();
        }
    }
}