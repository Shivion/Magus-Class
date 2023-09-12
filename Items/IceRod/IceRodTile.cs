using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MagusClass.Items.IceRod
{
    internal class IceRodTile : ModTile
    {
        public override string Texture => "Terraria/Images/Tiles_" + TileID.MagicalIceBlock;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;

            DustType = DustID.IceRod;
            AddMapEntry(new Color(173, 216, 230));
            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.MagicalIceBlock, 0));
            IceRodTileEntity advancedEntity = ModContent.GetInstance<IceRodTileEntity>();
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);

            Main.NewText(TileObjectData.newTile.CoordinateFullWidth); //was 0
            Main.NewText(TileObjectData.newTile.CoordinateFullHeight);
            Main.NewText(TileObjectData.newTile.StyleMultiplier);
        }
    }
}