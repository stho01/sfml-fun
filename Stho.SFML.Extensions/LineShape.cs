using System;
using SFML.Graphics;
using SFML.System;

namespace Stho.SFML.Extensions
{
    public class LineShape : Shape
    {
        public LineShape(float x1, float y1, float x2, float y2) 
        {
            P1 = new Vector2f(x1, y1);
            P2 = new Vector2f(x2, y2);
            FillColor = Color.White;
        }
        public LineShape(Vector2f p1, Vector2f p2)
        {
            P1 = p1;
            P2 = p2;
            FillColor = Color.White;
        }

        public Vector2f P1 { get; set; }
        public Vector2f P2 { get; set; }
        
        public override uint GetPointCount() => 2;

        public override Vector2f GetPoint(uint index)
        {
            if (index > 1)
                throw new ArgumentOutOfRangeException();
            
            return index == 0 ? P1 : P2;
        }

       
    }
}