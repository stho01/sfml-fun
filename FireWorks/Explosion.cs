using System;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace FireWorks
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