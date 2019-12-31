using System.Collections.Generic;
using SFML.System;

namespace Fireworks
{
    public class Rocket
    {
        //**********************************************************
        //** ctor:
        //**********************************************************

        public Rocket(float strength = 100f)
        {
            Strength = strength;
        }

        //**********************************************************
        //** props:
        //**********************************************************

        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; set; }
        public Vector2f Acceleration { get; set; }
        public Vector2f Direction { get; set; }
        public float Fuel { get; set; } = 100f;
        public bool HasFuel => Fuel > 0;
        public float Mass { get; set; }
        public float Strength { get; }
        public bool Done { get; set; }
        public float Age { get; set; }
        public float TotalLifetime { get; set; }
        public bool IsDead => Done || Age >= TotalLifetime;
        
        public List<Particle> Trail { get; } = new List<Particle>();
        public float TrailEmitTime { get; set; } = 20; // ms
        public float TrailTimeSinceLastEmit { get; set; } = 0;
    }
}