using System;
using SFML.System;
using Stho.SFML.Extensions;

namespace FireWorks
{
    public class ExplosionUpdater
    {
        //**********************************************************
        //** fields:
        //**********************************************************
        
        private readonly Game _game;

        //**********************************************************
        //** ctor
        //**********************************************************

        public ExplosionUpdater(Game game)
        {
            _game = game;
        }

        //**********************************************************
        //** props
        //**********************************************************
        
        public float AirResistance { get; set; } = 1.7f;

        //**********************************************************
        //** methods:
        //**********************************************************

        public void Update(Explosion explosion)
        {
            foreach (var explosionParticle in explosion.Particles)
            {
                explosionParticle.Age += Timer.DeltaTimeMilliseconds;
                explosionParticle.Velocity -= explosionParticle.Velocity.Normalize() * AirResistance * Timer.DeltaTimeSeconds;
                explosionParticle.Position += explosionParticle.Velocity * Timer.DeltaTimeSeconds;
            }
        }
    }
}