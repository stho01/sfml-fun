using SFML.Graphics;
using SFML.System;

namespace FireWorks
{
    public class Particle
    {
        public Color Color { get; set; }
        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; set; }
        public float Opacity { get; set; }
        public float Mass { get; set; }
    }
}