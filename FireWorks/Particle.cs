using SFML.Graphics;
using SFML.System;

namespace FireWorks
{
    public class Particle
    {
        public byte R { get; set; } = 255;
        public byte G { get; set; } = 255;
        public byte B { get; set; } = 255;
        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; set; }
        public float Age { get; set; }
        public float TotalLifetime { get; set; }
        public float Mass { get; set; }
        public bool IsDead => Age >= TotalLifetime;
    }
}