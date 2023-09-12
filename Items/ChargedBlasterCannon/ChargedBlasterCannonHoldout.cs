using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.ChargedBlasterCannon
{
    internal class ChargedBlasterCannonHoldout : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ChargedBlasterCannon;

        // This value controls how frequently the Cannon emits sound once it's firing.
        private const int SoundInterval = 20;

        public override void SetStaticDefaults()
        {
            // Signals to Terraria that this Projectile requires a unique identifier beyond its index in the Projectile array.
            // This prevents the issue with the vanilla LastPrism where the beams are invisible in multiplayer.
            // I'm not sure this applies but it can't hurt, I hope
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;

            // Prevents jitter when steping up and down blocks and half blocks
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ChargedBlasterCannon);
            base.SetDefaults();
            Projectile.hide = false;
            buffID = ModContent.BuffType<ChargedBlasterCannon.ChargedBlasterCannonBuff>();
            projectileID = ModContent.ProjectileType<ChargedBlasterCannonHoldout>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            Player player = Main.player[Projectile.owner];

            // Update the Cannon's damage every frame so that it is dynamically affected by Mana Sickness.
            UpdateDamageForManaSickness(player);

            PlaySounds();

            // Update the Cannon's behavior: project beam on frame 1.
            if (Projectile.owner == Main.myPlayer)
            {
                // Spawn in the Cannon's laser on the first frame.
                if (Projectile.localAI[2] == 1f)
                {
                    FireBeams();
                }
            }
        }

        private void UpdateDamageForManaSickness(Player player)
        {
            Projectile.damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(player.HeldItem.damage);
        }

        private void PlaySounds()
        {
            //The Cannon makes sound intermittently while in use, using the vanilla Projectile variable soundDelay.
            if (Projectile.soundDelay <= 0)
            {
                Projectile.soundDelay = SoundInterval;

                // On the very first frame, the sound playing is skipped. This way it doesn't overlap the starting hiss sound.
                if (Projectile.localAI[0] > 1f)
                {
                    SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
                }
            }
        }
        private void FireBeams()
        {
            // If for some reason the beam velocity can't be correctly normalized, set it to a default value.
            Vector2 beamVelocity = Vector2.Normalize(Projectile.velocity);
            if (beamVelocity.HasNaNs())
            {
                beamVelocity = -Vector2.UnitY;
            }

            // This UUID will be the same between all players in multiplayer, ensuring that the beams are properly anchored on the Cannon on everyone's screen.
            int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI);

            int damage = Projectile.damage;
            float knockback = Projectile.knockBack;

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, beamVelocity, ModContent.ProjectileType<ChargedBlasterCannonLaser>(), damage, knockback, Projectile.owner, uuid);

            // After creating the beams, mark the Cannon as having an important network event. This will make Terraria sync its data to other players ASAP.
            Projectile.netUpdate = true;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}