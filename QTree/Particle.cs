using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace QTree
{
    public class Particle
    {
        public Vector2f OriginalPosition { get; set; }
        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; set; }
        public float Radius { get; set; } = 5f;
        public bool Colliding { get; set; }
        public bool InNeighborhood { get; set; }

        public FloatCircle ToShape()
        {
            FloatCircle circle = Position;
            circle.Radius = Radius;
            return circle;
        }

        public bool Intersects(Particle p2)
        {
            var c1 = new FloatCircle(Position, Radius);
            var c2 = new FloatCircle(p2.Position, p2.Radius);

            return c1.Intersects(c2);
        }
    }
}