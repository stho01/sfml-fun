using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Fireworks
{
    public class ExplosionSpawner
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private readonly Game _game;
        private readonly List<Color> _colorRange = new List<Color>();

        //**********************************************************
        //** ctor:
        //**********************************************************

        public ExplosionSpawner(Game game)
        {
            _game = game;
            _colorRange.Add(Color.Red);
            _colorRange.Add(Color.Blue);
            _colorRange.Add(Color.Green);
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public Range DensityRange { get; set; } = 150..250;
        public Range LifetimeRange { get; set; } = 1000..1500;
        public Range StrengthRange { get; set; } = 40..80;
        
        //**********************************************************
        //** methods:
        //**********************************************************

        public void Spawn(Vector2f pos)
        {
            var explosion = new Explosion(RandomNumber.Get(DensityRange))
            {
                Strength = RandomNumber.Get(StrengthRange),
                Position = pos
            };
            
            InitializeParticles(explosion);
            
            _game.AddExplosion(explosion);
        }
        
        private void InitializeParticles(Explosion explosion)
        {
            var lifeTime = RandomNumber.Get(LifetimeRange);
            for (var i = 0; i < explosion.Particles.Length; i++)
            {
                var angleOfVelocity = i / Math.PI * 2;
                var direction = new Vector2f(
                    (float)Math.Cos(angleOfVelocity),    
                    (float)Math.Sin(angleOfVelocity)    
                );

                var variance = RandomNumber.Get(-(int)explosion.Strength, 30);
                var velocity = direction * (explosion.Strength + variance);

                var color = _colorRange[RandomNumber.Get(0, _colorRange.Count)];
                
                explosion.Particles[i] = new Particle 
                {
                    R = color.R,
                    G = color.G,
                    B = color.B,
                    Position = explosion.Position,
                    Velocity = velocity,
                    TotalLifetime = lifeTime,
                    Mass = RandomNumber.GetFloat(.75f, 1.25f)
                };
            }
        }
    }
}