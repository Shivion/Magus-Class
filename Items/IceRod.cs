using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MagusClass.Items
{
    public class IceRod : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.IceRod;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceRod);
            Item.mana = 5;
            Item.damage = 28;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.width = 30;
            Item.height = 30;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IceRodSpawner>();
            Item.buffType = ModContent.BuffType<IceRodBuff>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IceRod);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your spell alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);
            return true;
        }
    }

    internal class IceRodSpawner : MagusProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.IceBlock;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.IceBlock);
            Projectile.aiStyle = 0;
            Projectile.hide = false;
            Projectile.light = 0.5f;
            Projectile.coldDamage = true;
            buffID = ModContent.BuffType<IceRodBuff>();
            projectileID = ModContent.ProjectileType<IceRodSpawner>();
        }

        public override void AI()
        {
            base.AI();

            if (Thrown(1f, false))
            {
                Projectile.Kill();
            }

            //Collision Check
            int TileX = (int)Projectile.Center.X / 16;
            int TileY = (int)Projectile.Center.Y / 16;
            Tile tile = Main.tile[TileX, TileY];
            if (Projectile.lavaWet
                || (tile != null
                && tile.HasUnactuatedTile
                && tile.TileType != ModContent.TileType<IceRodTile>()
                && Main.tileSolid[tile.TileType]
                && !Main.tileSolidTop[tile.TileType]))
            {
                KillWithoutPlacing();
            }

            //Visuals
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceRod);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.3f;

            if (Projectile.velocity.X > 0f)
            {
                Projectile.rotation += 0.3f;
            }
            else
            {
                Projectile.rotation -= 0.3f;
            }
        }

        public void KillWithoutPlacing()
        {
            Kill(0);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(in SoundID.Item27, Projectile.position);
            for (int num613 = 0; num613 < 10; num613++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceRod);
            }

            if (Projectile.owner == Main.myPlayer && timeLeft > 0)
            {
                int blockPositionX = (int)targetPosition.X / 16;
                int blockPositionY = (int)targetPosition.Y / 16;
                if (Main.tile[blockPositionX, blockPositionY].HasTile && Main.tile[blockPositionX, blockPositionY].TileType == ModContent.TileType<IceRodTile>())
                {
                    WorldGen.KillTile(blockPositionX, blockPositionY);
                }
                else
                {
                    IceRodTileEntity.nextOwner = Projectile.owner;
                    WorldGen.PlaceTile(blockPositionX, blockPositionY, ModContent.TileType<IceRodTile>());
                    IceRodTileEntity advancedEntity = ModContent.GetInstance<IceRodTileEntity>();
                    advancedEntity.Hook_AfterPlacement(blockPositionX, blockPositionY, ModContent.TileEntityType<IceRodTileEntity>(), 0, 0, 0);
                }
            }
            Projectile.ai[2] = 1;
            Projectile.alpha = 255;
            base.Kill(Projectile.timeLeft);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }

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
            var newEntity = ModTileEntity.ByID[placedEntity];
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
            if (Main.tile[this.Position.X, this.Position.Y].TileType == ModContent.TileType<IceRodTile>())
            {
                WorldGen.KillTile(this.Position.X, this.Position.Y);
            }
            iceRodTileEntities.RemoveAll(iceRodTileEntity => iceRodTileEntity.Position.X == this.Position.X && iceRodTileEntity.Position.Y == this.Position.Y);
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
                player.ClearBuff(ModContent.BuffType<IceRodBuff>());
            }
            if (!player.HasBuff(ModContent.BuffType<IceRodBuff>()) || !IsTileValidForEntity(this.Position.X, this.Position.Y))
            {
                Kill(this.Position.X, this.Position.Y);
            }
        }
    }

    internal class IceRodBuff : MagusSpellBuff
    {
        protected override int ManaCost => 5;
        protected override bool MultipleSpellsAllowed => true;
        protected override int[] ProjectileTypes => new int[] { ModContent.ProjectileType<IceRodSpawner>() };
        protected override bool IsIceRod => true;
    }
}