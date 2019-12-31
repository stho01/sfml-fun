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
        
        public float AirResistance { get; set; } = 0.09f;

        //**********************************************************
        //** methods:
        //**********************************************************

        public void Update(Explosion explosion)
        {
            foreach (var explosionParticle in explosion.Particles)
            {
                explosionParticle.Age += Timer.DeltaTimeMilliseconds;
                
                var gravityForce = (explosionParticle.Position - _game.Earth.Position).Normalize() * _game.Gravity * AirResistance;
                var airResistanceDecay = -(explosionParticle.Velocity.Normalize() * AirResistance);
                
                explosionParticle.Acceleration += (gravityForce + airResistanceDecay) / explosionParticle.Mass * Timer.DeltaTimeSeconds;
                explosionParticle.Velocity += explosionParticle.Acceleration;
                explosionParticle.Position += explosionParticle.Velocity * Timer.DeltaTimeSeconds;
            }
        }
    }
}