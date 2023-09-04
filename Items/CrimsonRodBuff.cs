using Terraria.ModLoader;
using Terraria;

namespace MagusClass.Items
{
    internal class CrimsonRodBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CrimsonRodCloud>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<CrimsonRodCloudSeed>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                player.statManaMax2 -= 30;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
