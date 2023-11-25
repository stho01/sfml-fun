using System;
using SFML.Graphics;
using SFML.System;

namespace Stho.SFML.Extensions;

public static class Vector2Extensions
{
    public static readonly Vector2f Zero  = new Vector2f(0, 0);
    public static readonly Vector2f Right = new Vector2f(1, 0);
    public static readonly Vector2f Left = new Vector2f(-1, 0);
    public static readonly Vector2f Up = new Vector2f(1, -1);
    public static readonly Vector2f Down = new Vector2f(1, 1);

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


    public static Vector2f Multiply(this Vector2f vec, Vector2f scalar)
    {
            return new Vector2f(
                vec.X * scalar.X,
                vec.Y * scalar.Y
            );
        }

    public static Vector2f Multiply(this Vector2f vec, float x, float y)
    {
            return new Vector2f(
                vec.X * x,
                vec.Y * y
            );
        }

        
    public static Vector2f Multiply(this Vector2f vec, Vector2i scalar)
    {
            return new Vector2f(
                vec.X * scalar.X,
                vec.Y * scalar.Y
            );
        }

    public static Vector2i Multiply(this Vector2i vec, Vector2i scalar)
    {
            return new Vector2i(
                vec.X * scalar.X,
                vec.Y * scalar.Y
            );
        } 
        
    public static Vector2i Multiply(this Vector2i vec, Vector2f scalar)
    {
            return new Vector2i(
                (int)(vec.X * scalar.X),
                (int)(vec.Y * scalar.Y)
            );
        } 
        
    public static Vector2f Divide(this Vector2f vec, Vector2f divisor)
    {
            var x = divisor.X == 0 ? 0 : vec.X / divisor.X;
            var y = divisor.Y == 0 ? 0 : vec.Y / divisor.Y;
            return new Vector2f(x, y);
        }

    public static Vector2f Divide(this Vector2f vec, Vector2i divisor)
    {
            var x = divisor.X == 0 ? 0 : vec.X / divisor.X;
            var y = divisor.Y == 0 ? 0 : vec.Y / divisor.Y;
            return new Vector2f(x, y);
        }

    public static Vector2i Divide(this Vector2i vec, Vector2i divisor)
    {
            var x = divisor.X == 0 ? 0 : vec.X / divisor.X;
            var y = divisor.Y == 0 ? 0 : vec.Y / divisor.Y;
            return new Vector2i(x, y);
        } 
        
    public static Vector2i Divide(this Vector2i vec, Vector2f divisor)
    {
            var x = divisor.X == 0 ? 0 : (int)(vec.X / divisor.X);
            var y = divisor.Y == 0 ? 0 : (int)(vec.Y / divisor.Y);
            return new Vector2i(x, y);
        }

    public static (Vector2f, Vector2f) Normal(Vector2f p1, Vector2f p2)
    {
            var d = p2 - p1;
            var n1 = new Vector2f(-d.Y, d.X);
            var n2 = new Vector2f(d.Y, -d.X);
            return (n1, n2);
        }

    public static Vector2f NormalDirection(Vector2f p1, Vector2f p2)
    {
            var n = Normal(p1, p2);

            return (n.Item2 - n.Item1).Normalize();
        }

    public static FloatLine Normal(this FloatLine line)
    {
            var normal = Normal(line.P1, line.P2);
            
            return new FloatLine(normal.Item1, normal.Item2); 
        }
        
    public static Vector2f NormalDirection(this FloatLine line)
    {
            return NormalDirection(line.P1, line.P2);
        }

        
        
    public static Vector2f Reflect(this Vector2f vec, FloatLine mirror)
    {
            return vec.Reflect(mirror.NormalDirection());
        }
        
    public static Vector2f Reflect(this Vector2f vec, Vector2f normal)
    {
            var d = vec.Normalize();
            return d - 2 * d.Dot(normal) * normal;
        }
}