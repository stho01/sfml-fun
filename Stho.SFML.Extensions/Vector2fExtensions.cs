using System;
using SFML.System;

namespace Stho.SFML.Extensions
{
    public static class Vector2fExtensions
    {
        public static readonly Vector2f Zero  = new Vector2f(0, 0);
        
        public static PolarVector2f ToPolarCoordinates(this Vector2f vector)
        {
            if (vector == Zero)
                return new PolarVector2f(0, 0);
            
            var quadrant = vector.Quadrant();
            var quadrantAddition = quadrant switch {
                2 => 180,
                3 => 180,
                4 => 360,
                _ => 0
            };
            
            var degrees = (float)(Math.Atan(vector.Y/vector.X) * (180 / Math.PI));
            var polar = new PolarVector2f(
                vector.Length(),
                degrees + quadrantAddition + 90
            );
            
            return polar;
        }
        
        public static int Quadrant(this Vector2f vector)
        {
            return vector.X >= 0 && vector.Y >= 0 ? 1
                 : vector.X <  0 && vector.Y >= 0 ? 2
                 : vector.X <  0 && vector.Y <  0 ? 3
                 : vector.X >= 0 && vector.Y <  0 ? 4
                 : 0;
        }
        
        public static float Dot(this Vector2f a, Vector2f b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        public static Vector2f Normalize(this Vector2f vector)
        {
            var length = vector.Length();
            if (length == 0)
                return new Vector2f();

            return new Vector2f(
                vector.X / length,
                vector.Y / length
            );
        }

        public static float Length(this Vector2f vector)
        {
            return (float)Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y));
        }

        public static float SqrLength(this Vector2f vector)
        {
            return vector.X * vector.X + vector.Y * vector.Y;
        }

        public static Vector2f LookAt(this Vector2f vec, Vector2f point)
        {
            return (point - vec).Normalize();
        }

       
    }
}