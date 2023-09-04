using Terraria;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class WaterBoltBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<WaterBoltSpawner>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                player.statManaMax2 -= 50;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}