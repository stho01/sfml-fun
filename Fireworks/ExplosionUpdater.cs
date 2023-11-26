using Stho.SFML.Extensions;

namespace Fireworks;

public class ExplosionUpdater(Game game)
{
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
            explosionParticle.Age += (float)Timer.DeltaTimeMilliseconds;

            var gravityForce = (explosionParticle.Position - game.Earth.Position).Normalize() * game.Gravity * AirResistance;
            var airResistanceDecay = -(explosionParticle.Velocity.Normalize() * AirResistance);

            explosionParticle.Acceleration += (gravityForce + airResistanceDecay) / explosionParticle.Mass * (float)Timer.DeltaTimeSeconds;
            explosionParticle.Velocity += explosionParticle.Acceleration;
            explosionParticle.Position += explosionParticle.Velocity * (float)Timer.DeltaTimeSeconds;
        }
    }
}