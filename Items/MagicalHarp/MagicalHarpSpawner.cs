using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.MagicalHarp
{
    internal class MagicalHarpSpawner : SimpleProjectileSpawner
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MagicalHarp;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 30;
            spawnedProjectileType = ProjectileID.EighthNote;
            buffID = ModContent.BuffType<MagicalHarp.MagicalHarpBuff>();
            projectileID = ModContent.ProjectileType<MagicalHarpSpawner>();
            coneRadius = 15;
            spawnInterval = 12f;
            sound = SoundID.Item26;
            doSpin = false;
            horizontalSprite = true;
        }

        internal override bool SoundUpdateCallback(ProjectileAudioTracker tracker, ActiveSound soundInstance)
        {
            soundInstance.Pitch = Projectile.ai[1];
            return tracker.IsActiveAndInGame();
        }
    }
}