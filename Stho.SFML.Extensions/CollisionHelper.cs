using SFML.System;

namespace Stho.SFML.Extensions;

public class CollisionHelper
{
    public static Vector2f CalculateElasticCollision(float m1, Vector2f v1, float m2, Vector2f v2)
    {
            var sumM = m1 + m2;
            var a = (m1 - m2) / sumM * v1;
            var b = (2 * m2 / sumM) * v2;
            return a + b;
        }
}