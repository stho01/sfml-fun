using System;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace Skate;

public class SkaterUpdater(Game game)
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly Game _game = game;

    //**********************************************************
    //** methods:
    //**********************************************************

    public void Update(Skater skater)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            skater.Angle -= (360f * Timer.DeltaTimeSeconds);
        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            skater.Angle += (360f * Timer.DeltaTimeSeconds);

        var rad = MathUtils.DegreeToRadian(skater.Angle - 90);
        var dir = new Vector2f(
            (float)Math.Cos(rad),
            (float)Math.Sin(rad)
        );

        var force = new Vector2f();
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            force += dir * skater.Strength;
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            force += dir * 100f;

        force += -(skater.Velocity.Normalize() * 100f);

        var a = (force / skater.Mass);
        skater.Acceleration = a;
        skater.Velocity += a;
        skater.Position += skater.Velocity;
    }
}