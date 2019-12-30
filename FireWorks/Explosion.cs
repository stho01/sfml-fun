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
            InitializeParticles();   
        }

        //**********************************************************
        //** props:
        //**********************************************************
        
        public float Strength { get; set; }
        public Vector2f Position { get; set; }
        public Particle[] Particles { get; }
        public bool Done => Particles.All(p => p.Opacity <= 0);

        //**********************************************************
        //** methods:
        //**********************************************************
        
        private void InitializeParticles()
        {
            for (var i = 0; i < Particles.Length; i++)
            {
                var angleOfVelocity = i / Math.PI * 2;
                var direction = new Vector2f(
                    (float)Math.Cos(angleOfVelocity),    
                    (float)Math.Sin(angleOfVelocity)    
                );
                var velocity = direction * Strength;
                
                Particles[i] = new Particle 
                {
                    Color = Color.White,
                    Position = Position,
                    Velocity = velocity,
                    Mass = RandomNumber.GetFloat(.1f, .5f)
                };
            }
        }
    }
}