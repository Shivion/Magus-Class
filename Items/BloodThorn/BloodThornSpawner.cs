using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagusClass.Items.BloodThorn
{
    internal class BloodThornSpawner : MagusProjectile
{
    public override string Texture => "Terraria/Images/Item_" + ItemID.SharpTears;

    public override void SetDefaults()
    {
        base.SetDefaults();
        Projectile.CloneDefaults(ProjectileID.SharpTears);
        Projectile.tileCollide = true;
        Projectile.aiStyle = 0;
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.hide = false;
        Projectile.alpha = 0;
        buffID = ModContent.BuffType<BloodThorn.BloodThornBuff>();
        projectileID = ModContent.ProjectileType<BloodThornSpawner>();
    }

    public override void AI()
    {
        KillExistingProjectiles();
        base.AI();


        if (Thrown(1, false))
        {
            if (Projectile.ai[2] == 0 && Projectile.ai[0] > 15f)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 pointPoisition2 = Projectile.Center;
                    Vector2 vector18 = pointPoisition2 + Main.rand.NextVector2Circular(8f, 8f);
                    Vector2 vector19 = FindSharpTearsSpot(vector18).ToWorldCoordinates(Main.rand.Next(17), Main.rand.Next(17));
                    Vector2 vector20 = (vector18 - vector19).SafeNormalize(-Vector2.UnitY) * 16f;
                    int spawnedProjectile = Projectile.NewProjectile(Projectile.GetSource_ReleaseEntity(), vector19.X, vector19.Y, vector20.X, vector20.Y, ProjectileID.SharpTears, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, Main.rand.NextFloat() * 0.5f + 0.6f);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, spawnedProjectile);
                }
                //SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
                Projectile.ai[0] = Main.rand.Next(0, 5);
            }
            Projectile.ai[0]++;
        }
    }

    private Point FindSharpTearsSpot(Vector2 targetSpot)
    {
        Point point = targetSpot.ToTileCoordinates();
        Vector2 center = Projectile.Center;
        Vector2 endPoint = targetSpot;
        int samplesToTake = 3;
        float samplingWidth = 4f;
        Collision.AimingLaserScan(center, endPoint, samplingWidth, samplesToTake, out var vectorTowardsTarget, out var samples);
        float num = float.PositiveInfinity;
        for (int i = 0; i < samples.Length; i++)
        {
            if (samples[i] < num)
            {
                num = samples[i];
            }
        }
        targetSpot = center + vectorTowardsTarget.SafeNormalize(Vector2.Zero) * num;
        point = targetSpot.ToTileCoordinates();
        Rectangle value = default;
        value = new Rectangle(point.X, point.Y, 1, 1);
        value.Inflate(6, 16);
        Rectangle value2 = default;
        value2 = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
        value2.Inflate(-40, -40);
        value = Rectangle.Intersect(value, value2);
        List<Point> list = new List<Point>();
        List<Point> list2 = new List<Point>();
        Vector2 value3 = default;
        for (int j = value.Left; j <= value.Right; j++)
        {
            for (int k = value.Top; k <= value.Bottom; k++)
            {
                if (!WorldGen.SolidTile2(j, k))
                {
                    continue;
                }
                value3 = new Vector2(j * 16 + 8, k * 16 + 8);
                if (!(Vector2.Distance(targetSpot, value3) > 200f))
                {
                    if (FindSharpTearsOpening(j, k, j > point.X, j < point.X, k > point.Y, k < point.Y))
                    {
                        list.Add(new Point(j, k));
                    }
                    else
                    {
                        list2.Add(new Point(j, k));
                    }
                }
            }
        }
        if (list.Count == 0 && list2.Count == 0)
        {
            list.Add((Projectile.Center.ToTileCoordinates().ToVector2() + Main.rand.NextVector2Square(-2f, 2f)).ToPoint());
        }
        List<Point> list3 = list;
        if (list3.Count == 0)
        {
            list3 = list2;
        }
        int index = Main.rand.Next(list3.Count);
        return list3[index];
    }

    private bool FindSharpTearsOpening(int x, int y, bool acceptLeft, bool acceptRight, bool acceptUp, bool acceptDown)
    {
        if (acceptLeft && !WorldGen.SolidTile(x - 1, y))
        {
            return true;
        }
        if (acceptRight && !WorldGen.SolidTile(x + 1, y))
        {
            return true;
        }
        if (acceptUp && !WorldGen.SolidTile(x, y - 1))
        {
            return true;
        }
        if (acceptDown && !WorldGen.SolidTile(x, y + 1))
        {
            return true;
        }
        return false;
    }

    public override bool ShouldUpdatePosition()
    {
        return false;
    }
}
}