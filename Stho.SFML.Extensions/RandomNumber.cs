using System;
using SFML.Graphics;
using SFML.System;

namespace Stho.SFML.Extensions;

public static class RandomNumber
{
    private static readonly Random Random = new Random((int)DateTime.UtcNow.Ticks);
        
    public static Vector2f VectorAndReflect(int min, int max) => VectorAndReflect(min, max, min, max);
    public static Vector2f VectorAndReflect(int xMin, int xMax, int yMin, int yMax)
    {
            if (xMin <= 0 || xMax <= 0 || yMin <= 0 || yMax <= 0)
                throw new InvalidOperationException("Arguments must be greater that zero");

            var x = Random.Next(xMin, xMax) * (Random.Next(0, 2) == 1 ? -1 : 1);
            var y = Random.Next(yMin, yMax) * (Random.Next(0, 2) == 1 ? -1 : 1);
            
            return new Vector2f(x, y);
        }
        
    public static Vector2f Vector(int min, int max) => Vector(min, max, min, max);
        
    public static Vector2f Vector(int xMin, int xMax, int yMin, int yMax)
    {
            var x = Random.Next(xMin, xMax);
            var y = Random.Next(yMin, yMax);
            return new Vector2f(x, y);
        }

    public static int Get(Range range)
    {
            return Get(range.Start.Value, range.End.Value);
        }
        
    public static int Get(int min, int max)
    {
            return Random.Next(min, max);
        }

    public static float GetFloat()
    {
            return (float)Random.NextDouble();
        }

    public static float GetFloat(float min, float max)
    {
            var totalRange = (max - min);
            return (GetFloat() * totalRange) + min;
        }
}