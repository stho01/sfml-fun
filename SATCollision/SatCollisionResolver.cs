using System;
using SFML.System;
using Stho.SFML.Extensions;

namespace SATCollision
{
    public class SatCollisionResolver
    {
        public static bool Intersects(Vector2f[] p, Vector2f[] q)
        {
            bool PrimaryCollisionCheck(Vector2f[] pPoints, Vector2f[] qPoints)
            {
                for (var i = 0; i < pPoints.Length; i++)
                {
                    var p1 = pPoints[i];
                    var p2 = pPoints[(i + 1) % pPoints.Length];
                    var direction = (p2 - p1);
                    var normal = new Vector2f(-direction.Y, direction.X);

                    var (pMin, pMax) = GetMinMax(normal, pPoints);
                    var (qMin, qMax) = GetMinMax(normal, qPoints);

                    if (!(pMax > qMin || qMax < pMin)) 
                        return false;
                }

                return true;
            }
            
            (float min, float max) GetMinMax(Vector2f normal, Vector2f[] points)
            {
                var max = float.MinValue;
                var min = float.MaxValue;

                foreach (var point in points)
                {
                    max = Math.Max(point.Dot(normal), max);
                    min = Math.Min(point.Dot(normal), min);
                }

                return (min, max);
            }


            return PrimaryCollisionCheck(p, q) 
                && PrimaryCollisionCheck(q, p);
        }
    }
}