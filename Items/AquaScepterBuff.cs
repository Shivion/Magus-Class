using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class AquaScepterBuff : MagusSpellBuff
    {
        protected override int ManaCost => 50;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<AquaScepterSpawner>() };
    }
}