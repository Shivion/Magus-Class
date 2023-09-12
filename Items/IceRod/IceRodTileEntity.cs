using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.IceRod
{
    internal class IceRodTileEntity : ModTileEntity
    {
        internal static int nextOwner;
        public static List<IceRodTileEntity> iceRodTileEntities = new List<IceRodTileEntity>();
        public int owner;

        public override bool IsTileValidForEntity(int x, int y)
        {
            return Main.tile[x, y].HasTile && Main.tile[x, y].TileType == ModContent.TileType<IceRodTile>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 1, 1);
                NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
                return -1;
            }

            int placedEntity = Place(i, j);
            var newEntity = ByID[placedEntity];
            ((IceRodTileEntity)newEntity).owner = nextOwner;
            iceRodTileEntities.Add((IceRodTileEntity)newEntity);
            return placedEntity;
        }

        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }
        }

        public override void OnKill()
        {
            if (Main.tile[Position.X, Position.Y].TileType == ModContent.TileType<IceRodTile>())
            {
                WorldGen.KillTile(Position.X, Position.Y);
            }
            iceRodTileEntities.RemoveAll(iceRodTileEntity => iceRodTileEntity.Position.X == Position.X && iceRodTileEntity.Position.Y == Position.Y);
            base.OnKill();
        }

        internal static List<IceRodTileEntity> GetIceRodTileEntitiesByOwner(int owner)
        {
            return iceRodTileEntities.FindAll(iceRodTileEntity => iceRodTileEntity.owner == owner);
        }

        public override void Update()
        {
            Player player = Main.player[owner];
            //Kill all projectiles without the buff
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<IceRod.IceRodBuff>());
            }
            if (!player.HasBuff(ModContent.BuffType<IceRod.IceRodBuff>()) || !IsTileValidForEntity(Position.X, Position.Y))
            {
                Kill(Position.X, Position.Y);
            }
        }
    }
}