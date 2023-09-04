using Terraria;
using Microsoft.Xna.Framework;

namespace MagusClass
{
    internal static class ShivUtilities
    {
        public static Vector2 FindRestingSpot(Vector2 pointPoisition)
        {
            int X = (int)pointPoisition.X / 16;
            int Y = (int)pointPoisition.Y / 16;
            int ScanDirection = 1;

            if (!CheckForSentryCollison(X, Y))
            {
                ScanDirection = -1;
            }

            //scanning down and not colliding
            bool colliding;
            do
            {
                if (Y < 10 || Y > Main.maxTilesY - 10)
                {
                    return Vector2.Zero;
                }

                Y += ScanDirection;
                colliding = CheckForSentryCollison(X, Y);
            }
            while ((colliding && ScanDirection > 0) || (!colliding && ScanDirection < 0));

            int worldY = Y * 16;

            return new Vector2(pointPoisition.X, worldY);
        }

        public static bool CheckForSentryCollison(int X, int Y)
        {
            return !WorldGen.SolidTile2(X, Y) && !WorldGen.SolidTile2(X - 1, Y) && !WorldGen.SolidTile2(X + 1, Y);
        }
    }
}
