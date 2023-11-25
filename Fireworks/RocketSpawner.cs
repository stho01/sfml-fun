using System;
using SFML.System;
using Stho.SFML.Extensions;

namespace Fireworks;

public class RocketSpawner(Game game)
{
    //**********************************************************
    //** props:
    //**********************************************************

    public Range StrengthRange { get; set; } = new(22, 27);
    public Range MassRange { get; set; } = new(10, 15);
    public Range FuelRange { get; set; } = new(50, 75);
    public Range TotalLifeTime { get; set; } = new(500, 1500);

    //**********************************************************
    //** methods:
    //**********************************************************

    public void SpawnOnEarthSurface()
    {
        var angle = MathUtils.DegreeToRadian(RandomNumber.Get(0, 360));
        var dir = new Vector2f(
            (float)Math.Cos(angle),
            (float)Math.Sin(angle)
        );
        var position = game.Earth.Position + dir * (game.Earth.Radius + 10);

        var randomAngle = angle + RandomNumber.GetFloat(-1, 1) * Math.PI * .13f;

        // Just to make it more interesting.      
        var randomDir = new Vector2f(
            (float)Math.Cos(randomAngle),
            (float)Math.Sin(randomAngle)
        );

        var rocket = new Rocket(RandomNumber.Get(StrengthRange))
        {
            Position = position,
            Direction = randomDir,
            Mass = RandomNumber.Get(MassRange),
            Fuel = RandomNumber.Get(FuelRange),
            TotalLifetime = RandomNumber.Get(TotalLifeTime)
        };

        game.AddRocket(rocket);
    }
}