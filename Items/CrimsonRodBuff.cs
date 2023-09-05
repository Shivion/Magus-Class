using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class CrimsonRodBuff : MagusSpellBuff
    {
        protected override int ManaCost => 30;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<CrimsonRodCloudSeed>(), ModContent.ProjectileType<CrimsonRodCloud>() };
    }
}
