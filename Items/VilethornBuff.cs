using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class VilethornBuff : MagusSpellBuff
    {
        protected override int ManaCost => 30;
        protected override bool MultipleSpellsAllowed => false;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<VilethornSpawner>() };
    }
}