using System;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace TankGame;

public class TankRenderer(RenderTarget renderTarget)
{
    //**********************************************************
    //** fields:
    //**********************************************************

    //**********************************************************
    //** ctors:
    //**********************************************************

    //**********************************************************
    //** methods:
    //**********************************************************

    public void Render(Tank tank)
    {
        RenderBody(tank);
        RenderBarrel(tank);
        RenderDirection(tank);
    }

    public void RenderBody(Tank tank)
    {
        var shape = new RectangleShape
        {
            FillColor = Color.Green,
            OutlineColor = Color.Black,
            OutlineThickness = -1,
            Size = new Vector2f(46, 30),
            Origin = new Vector2f(23, 15),
            Position = tank.Position,
            Rotation = tank.Body.Angle
        };

        renderTarget.Draw(shape);
    }

    public void RenderBarrel(Tank tank)
    {
        var radius = 12;
        var offset = new Vector2f(0, 0);

        var barrelBody = new CircleShape
        {
            Radius = radius,
            Position = tank.Position + offset,
            Origin = new Vector2f(radius, radius),
            FillColor = Color.Black,
            Rotation = tank.Barrel.Angle
        };
        renderTarget.Draw(barrelBody);

        var barrel = new RectangleShape
        {
            FillColor = Color.Black,
            Size = new Vector2f(10, 30),
            Origin = new Vector2f(5, 30),
            Position = tank.Position + offset,
            Rotation = tank.Barrel.Angle
        };
        renderTarget.Draw(barrel);
    }

    public void RenderDirection(Tank tank)
    {
        var angle = MathUtils.DegreeToRadian(tank.Body.Angle);
        var dir = new Vector2f(
            (float)Math.Cos(angle),
            (float)Math.Sin(angle));

        var p1 = dir * 30 + tank.Position;
        var p2 = tank.Position;

        renderTarget.Draw(new[]
        {
            new Vertex(p1, Color.Red),
            new Vertex(p2, Color.Red)
        }, 0, 2, PrimitiveType.Lines);
    }
}