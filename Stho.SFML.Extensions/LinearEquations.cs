using System.Resources;
using SFML.System;

namespace Stho.SFML.Extensions
{
    /// <summary>A structure that represents a line by y = mx + b</summary>
    public struct SlopeInterceptForm
    {
        /// <summary>Slope of line</summary>
        public float M { get; set; }
        
        /// <summary>Line Y intercept</summary>
        public float B { get; set; }

        public Vector2f PointOnLineX(float x)
        {
            var y = M * x + B;
            return new Vector2f(x, y);
        }

        public Vector2f PointOnLineY(float y)
        {
            var x = -((B - y) / M);
            return new Vector2f(x, y);
        }
    }

    /// <summary>A structure that represents a line by (x - x1) = m(y - y1)</summary>
    public struct TwoPointForm
    {
        /// <summary>Slope of the line</summary>
        public float M { get; set; }
        
        /// <summary>X coordinate of a point on the line</summary>
        public float X1 { get; set; }
        
        /// <summary>Y coordinate of a point on the line</summary>
        public float Y1 { get; set; }

        public Vector2f PointOnLineX(float x)
        {
            var y = (M * (x - X1)) + Y1;
            return new Vector2f(x, y);
        }

        public Vector2f PointOnLineY(float y)
        {
            var x = ((y - Y1) / M) + X1;
            return new Vector2f(x, y);
        }
    }
   
    public static class LinearEquations
    {
        public static SlopeInterceptForm GetSlopeInterceptForm(FloatLine line)
        {
            var x1 = line.P1.X;
            var y1 = line.P1.Y;
            var x2 = line.P2.X;
            var y2 = line.P2.Y;
            var m = Slope(x1, y1, x2, y2);
            var b = -(m * x1) + y1;
            return new SlopeInterceptForm {M = m, B = b};
        }

        public static TwoPointForm GetTwoPointForm(FloatLine line)
        {
            var x1 = line.P1.X;
            var y1 = line.P1.Y;
            var x2 = line.P2.X;
            var y2 = line.P2.Y;
            var m = Slope(x1, y1, x2, y2);
            return new TwoPointForm { M = m, X1 = x1, Y1 = y1 };
        }
  
        public static float Slope(float x1, float y1, float x2, float y2)
        {
            return (y2 - y1) / (x2 - x1);
        }
    }
}