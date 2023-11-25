using System;
using SFML.System;
using Stho.SFML.Extensions;

namespace Fireworks;

public class RocketUpdater(Game game, ExplosionSpawner explosionSpawner)
{
    //**********************************************************
    //** methods:
    //**********************************************************

    public void Update(Rocket rocket)
    {
        UpdateAge(rocket);

        if (rocket.IsDead)
        {
            explosionSpawner.Spawn(rocket.Position);
            return;
        }

        UpdatePosition(rocket);
        UpdateTrail(rocket);

        if (IntersectsWithEarth(rocket))
            HandleEarthIntersection(rocket);

        UseFuel(rocket);
    }


    private void UpdateAge(Rocket rocket)
    {
        rocket.Age += Timer.DeltaTimeMilliseconds;
    }

    private void UpdatePosition(Rocket rocket)
    {
        var gravity = (rocket.Position - game.Earth.Position).Normalize() * game.Gravity;

        var force = new Vector2f();
        if (rocket.HasFuel)
            force = rocket.Direction.Normalize() * rocket.Strength;

        rocket.Acceleration = ((force + gravity) / rocket.Mass) * Timer.DeltaTimeSeconds;
        rocket.Velocity += rocket.Acceleration;
        rocket.Position += rocket.Velocity * Timer.DeltaTimeMilliseconds;
    }

    private void UpdateTrail(Rocket rocket)
    {
        rocket.Trail.ForEach(p => { p.Age += Timer.DeltaTimeMilliseconds; });
        rocket.Trail.RemoveAll(p => p.IsDead);
        rocket.TrailTimeSinceLastEmit += Timer.DeltaTimeMilliseconds;

        if (rocket.HasFuel && rocket.TrailTimeSinceLastEmit >= rocket.TrailEmitTime)
        {
            rocket.TrailTimeSinceLastEmit = 0;
            rocket.Trail.Add(new Particle
            {
                Position = rocket.Position,
                TotalLifetime = 150,
                Mass = 1f,
                R = 255,
                G = 165,
                B = 74
            });
        }
    }

    private bool IntersectsWithEarth(Rocket rocket)
    {
        return (rocket.Position - game.Earth.Position).Length() <= game.Earth.Radius;
    }

    private void HandleEarthIntersection(Rocket rocket)
    {
        var dotProduct = rocket.Velocity.Dot(game.Earth.Position); // if rocket is moving away from earth then dotProduct > 0
        if (dotProduct > 0)
            rocket.Acceleration = new Vector2f();

        var intersectionPoint = game.Earth.Position + (rocket.Position - game.Earth.Position).Normalize() * game.Earth.Radius;
        rocket.Position = intersectionPoint;
        rocket.Velocity = new Vector2f();
        rocket.Done = true;
    }

    private void UseFuel(Rocket rocket)
    {
        rocket.Fuel = Math.Max(0f, rocket.Fuel - (200 * Timer.DeltaTimeSeconds)); // fuel lasts 1 sec
    }
}