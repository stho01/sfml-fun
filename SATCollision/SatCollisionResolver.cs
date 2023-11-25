using System;
using System.Collections.Generic;
using SFML.System;
using Stho.SFML.Extensions;

namespace SATCollision;

public class SatCollisionResolver
{
    public static bool Intersects(Vector2f[] p, Vector2f[] q)
    {
            return PrimaryCollisionCheck(p, q) 
                && PrimaryCollisionCheck(q, p);
        }
        
    /// <summary>Checking collisions for primary</summary>
    /// <param name="primary"></param>
    /// <param name="secondary"></param>
    /// <returns>True if primary points check evaluates true</returns>
    private static bool PrimaryCollisionCheck(Vector2f[] primary, Vector2f[] secondary)
    {
            for (var i = 0; i < primary.Length; i++)
            {
                var p1 = primary[i];
                var p2 = primary[(i + 1) % primary.Length];
                var edge = (p2 - p1);
                var normal = new Vector2f(-edge.Y, edge.X);

                var (pMin, pMax) = GetMinMax(normal, primary);
                var (qMin, qMax) = GetMinMax(normal, secondary);

                if (!(pMax < qMin || qMax > pMin)) 
                    return false;
            }

            return true;
        }

    private static (float min, float max) GetMinMax(Vector2f normal, IEnumerable<Vector2f> points)
    {
            var max = float.MinValue;
            var min = float.MaxValue;

            foreach (var point in points)
            {
                var dot = point.Dot(normal);
                max = Math.Max(dot, max);
                min = Math.Min(dot, min);
            }

            return (min, max);
        }
}