using System;
using SFML.System;
using Stho.SFML.Extensions;

namespace TankGame;

public class TankController(Game game)
{
    public void RotateBodyClockWise(Tank tank)
    {
        tank.Body.Angle += tank.BodySteeringStrength * (float)Timer.DeltaTimeSeconds;
    }

    public void RotateBodyAntiClockWise(Tank tank)
    {
        tank.Body.Angle -= tank.BodySteeringStrength * (float)Timer.DeltaTimeSeconds;
    }

    public void BarrelLookAtMousePosition(Tank tank)
    {
        tank.Barrel.Angle = tank.Position
            .LookAt((Vector2f)game.GetMousePosition())
            .ToPolarCoordinates()
            .Angle;
    }

    public void MoveForward(Tank tank)
    {
        var angle = MathUtils.DegreeToRadian(tank.Body.Angle);
        var dir = new Vector2f(
            (float)Math.Cos(angle),
            (float)Math.Sin(angle));

        tank.Acceleration += dir * tank.EngineStrength / tank.Mass;
    }

    public void ApplyFriction(Tank tank)
    {
        var velDir = tank.Velocity.Normalize();
        var w = tank.Mass * 9.8f;
        var friction = velDir * -0.1f * w;

        tank.Acceleration += friction / tank.Mass;
    }

    public void Update(Tank tank)
    {
        tank.Velocity += tank.Acceleration;
        tank.Position += tank.Velocity * (float)Timer.DeltaTimeSeconds;
        tank.Acceleration = new Vector2f();
    }

    public void Reset(Tank tank)
    {
        tank.Acceleration = new Vector2f();
        tank.Velocity = new Vector2f();
        tank.Position = new Vector2f(game.WindowWidth / 2, game.WindowHeight / 2);
    }
}