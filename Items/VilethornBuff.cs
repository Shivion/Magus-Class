using Terraria;
using Terraria.ModLoader;

namespace MagusClass.Items
{
    internal class VilethornBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VilethornSpawner>()] > 0)
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