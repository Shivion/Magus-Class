﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.LastPrism
{
    internal class LastPrismHoldout : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.LastPrism;

        // The vanilla Last Prism is an animated item with 5 frames of animation. We copy that here.
        private const int NumAnimationFrames = 5;

        // This controls how many individual beams are fired by the Prism.
        public const int NumBeams = 6;

        // This value controls how many frames it takes for the Prism to reach "max charge". 60 frames = 1 second.
        public const float MaxCharge = 180f;

        // This value controls how many frames it takes for the beams to begin dealing damage. Before then they can't hit anything.
        public const float DamageStart = 30f;

        // This value controls how frequently the Prism emits sound once it's firing.
        private const int SoundInterval = 20;

        // This property encloses the internal AI variable Projectile.ai[0]. It makes the code easier to read.
        private float FrameCounter
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = NumAnimationFrames;

            // Signals to Terraria that this Projectile requires a unique identifier beyond its index in the Projectile array.
            // This prevents the issue with the vanilla Last Prism where the beams are invisible in multiplayer.
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;

            // Prevents jitter when steping up and down blocks and half blocks
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            // Use CloneDefaults to clone all basic Projectile statistics from the vanilla Last Prism.
            Projectile.CloneDefaults(ProjectileID.LastPrism);
            base.SetDefaults();
            buffID = ModContent.BuffType<LastPrism.LastPrismBuff>();
            projectileID = ModContent.ProjectileType<LastPrismHoldout>();
        }

        public override void AI()
        {
            KillExistingProjectiles();
            base.AI();

            Player player = Main.player[Projectile.owner];

            // Update the Prism's damage every frame so that it is dynamically affected by Mana Sickness.
            UpdateDamageForManaSickness(player);

            // Update the frame counter.
            FrameCounter += 1f;

            // Update Projectile visuals and sound.
            UpdateAnimation();
            PlaySounds();

            // Update the Prism's behavior: project beams on frame 1, consume mana, and despawn if out of mana.
            if (Projectile.owner == Main.myPlayer)
            {
                // Spawn in the Prism's lasers on the first frame if the player is capable of using the item.
                if (FrameCounter == 1f)
                {
                    FireBeams();
                }
            }
        }

        private void UpdateDamageForManaSickness(Player player)
        {
            Projectile.damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(player.HeldItem.damage);
        }

        private void UpdateAnimation()
        {
            Projectile.frameCounter++;

            // As the Prism charges up and focuses the beams, its animation plays faster.
            int framesPerAnimationUpdate = FrameCounter >= MaxCharge ? 2 : FrameCounter >= MaxCharge * 0.66f ? 3 : 4;

            // If necessary, change which specific frame of the animation is displayed.
            if (Projectile.frameCounter >= framesPerAnimationUpdate)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= NumAnimationFrames)
                {
                    Projectile.frame = 0;
                }
            }
        }

        private void PlaySounds()
        {
            // The Prism makes sound intermittently while in use, using the vanilla Projectile variable soundDelay.
            if (Projectile.soundDelay <= 0)
            {
                Projectile.soundDelay = SoundInterval;

                // On the very first frame, the sound playing is skipped. This way it doesn't overlap the starting hiss sound.
                if (FrameCounter > 1f)
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

            // This UUID will be the same between all players in multiplayer, ensuring that the beams are properly anchored on the Prism on everyone's screen.
            int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI);

            int damage = Projectile.damage;
            float knockback = Projectile.knockBack;
            for (int b = 0; b < NumBeams; ++b)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, beamVelocity, ModContent.ProjectileType<LastPrismLaser>(), damage, knockback, Projectile.owner, b, uuid);
            }

            // After creating the beams, mark the Prism as having an important network event. This will make Terraria sync its data to other players ASAP.
            Projectile.netUpdate = true;
        }

        // Because the Prism is a holdout Projectile and stays glued to its user, it needs custom drawcode.
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int spriteSheetOffset = frameHeight * Projectile.frame;
            Vector2 sheetInsertPosition = (Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();

            // The Prism is always at full brightness, regardless of the surrounding light. This is equivalent to it being its own glowmask.
            // It is drawn in a non-white color to distinguish it from the vanilla Last Prism.
            Color drawColor = Color.White;
            Main.EntitySpriteDraw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight)), drawColor, Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, effects, 0f);
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}