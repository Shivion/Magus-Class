using Terraria;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal abstract class MagusSpellBuff : ModBuff
    {
        protected abstract int ManaCost { get; }
        protected abstract bool MultipleSpellsAllowed { get; }
        protected abstract int[] ProjectileTypes { get; }

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int activeSpells = 0;
            for (int i = 0; i < ProjectileTypes.Length; i++)
            {
                activeSpells += player.ownedProjectileCounts[ProjectileTypes[i]];
            }
            if (activeSpells > 0)
            {
                player.buffTime[buffIndex] = 18000;
                player.statManaMax2 -= MultipleSpellsAllowed ? ManaCost * activeSpells : ManaCost;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}