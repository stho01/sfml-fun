using System;
using SFML.System;

namespace Stho.SFML.Extensions
{
    public static class RandomVector
    {
        private static readonly Random Random = new Random((int)DateTime.UtcNow.Ticks);
        
        public static Vector2f GetAndReflect(int min, int max) => GetAndReflect(min, max, min, max);
        public static Vector2f GetAndReflect(int xMin, int xMax, int yMin, int yMax)
        {
            if (xMin <= 0 || xMax <= 0 || yMin <= 0 || yMax <= 0)
                throw new InvalidOperationException("Arguments must be greater that zero");

            var x = Random.Next(xMin, xMax) * (Random.Next(0, 2) == 1 ? -1 : 1);
            var y = Random.Next(yMin, yMax) * (Random.Next(0, 2) == 1 ? -1 : 1);
            
            return new Vector2f(x, y);
        }
        
        public static Vector2f Get(int min, int max) => Get(min, max, min, max);
        
        public static Vector2f Get(int xMin, int xMax, int yMin, int yMax)
        {
            var x = Random.Next(xMin, xMax);
            var y = Random.Next(yMin, yMax);
            return new Vector2f(x, y);
        }
    }
}