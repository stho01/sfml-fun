using System.Linq;
using SFML.System;

namespace Fireworks
{
    public class Explosion
    {
        //**********************************************************
        //** ctor:
        //**********************************************************
        
        public Explosion(int density)
        {
            Particles = new Particle[density];
        }

        //**********************************************************
        //** props:
        //**********************************************************

        public float Strength { get; set; } = 1f;
        public Vector2f Position { get; set; }
        public Particle[] Particles { get; }
        public bool Done => Particles.All(p => p.IsDead);

        //**********************************************************
        //** methods:
        //**********************************************************
    }
}