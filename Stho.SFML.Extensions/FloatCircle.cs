using SFML.System;

namespace Stho.SFML.Extensions
{
    public struct FloatCircle
    {
        //**********************************************************
        //** ctor:
        //**********************************************************

        public FloatCircle(Vector2f pos, float radius) : this(pos.X, pos.Y, radius) {}
        public FloatCircle(float x, float y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
          
        //**********************************************************
        //** methods:
        //**********************************************************

        public bool Intersects(FloatCircle circle)
        {
            var dx = circle.X - X;
            var dy = circle.Y - Y;
            var m = dx * dx + dy * dy;
            var r = (Radius + circle.Radius) * (Radius + circle.Radius);
            return m <= r;
        }

        public bool Contains(float x, float y)
        {
            return Intersects(new FloatCircle(x, y, 1));
        }
          
        //**********************************************************
        //** operator overloads:
        //**********************************************************

        public static implicit operator FloatCircle(Vector2f vec) => new FloatCircle
        {
            X = vec.X,
            Y = vec.Y,
            Radius = 1f
        };
        
        public static explicit operator Vector2f(FloatCircle circle) => new Vector2f
        {
            X = circle.X,
            Y = circle.Y
        };
    }
}