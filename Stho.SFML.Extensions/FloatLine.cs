using System;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using SFML.System;

namespace Stho.SFML.Extensions
{
    public struct FloatLine
    {
        //**********************************************************
        //** ctors:
        //**********************************************************

        public FloatLine(float x1, float y1, float x2, float y2) : this(new Vector2f(x1, y1), new Vector2f(x2, y2)) {}
        public FloatLine(Vector2f p1, Vector2f p2)
        {
            P1 = p1;
            P2 = p2;
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public Vector2f P1 { get; set; }
        public Vector2f P2 { get; set; }
          
        //**********************************************************
        //** methods:
        //**********************************************************

        public Vector2f? Intersects(FloatLine line)
        {
            var x1 = line.P1.X;
            var y1 = line.P1.Y;
            var x2 = line.P2.X;
            var y2 = line.P2.Y;
            
            var x3 = P1.X;
            var y3 = P1.Y;
            var x4 = P2.X;
            var y4 = P2.Y;

            var den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (den == 0)
                return null;
            
            var t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
            var u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

            if (t > 0 && t < 1 && u > 0)
            {
                return new Vector2f(
                    x1 + (t * (x2 - x1)),
                    y1 + (t * (y2 - y1))
                );
            }

            return null;
        }
        
        public Vector2f[] Intersects(FloatCircle circle)
        {
            var circlePos = new Vector2f(circle.X, circle.Y);
            var r = circle.Radius;
            var p1 = P1 - circlePos; // translate p1 to circle local space
            var p2 = P2 - circlePos; // translate p2 to circle local space
            var d  = p2 - p1;

            var a = d.SqrLength();
            var b = 2 * ((d.X * p1.X) + (d.Y * p1.Y));
            var c = p1.SqrLength() - (r * r);
            
            var delta = b * b - (4 * a * c);
            
            if (delta < 0) 
                return new Vector2f[] {};
            
            if (delta == 1)
            {
                var u = -b / (2 * a);
                var intersection = P1 + (u * d);
                return new[] { intersection };
            }
            
            var sqrDelta = -Math.Sqrt(delta);
            var u1 = (float)(-b - sqrDelta) / (2 * a);
            var u2 = (float)(-b + sqrDelta) / (2 * a);

            var intersection1 = P1 + (u1 * d);
            var intersection2 = P1 + (u2 * d);
            
            return new[] { intersection1, intersection2 };
        }
    }
}