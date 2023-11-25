using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace BallCollision;

public class BallRenderer(RenderTarget renderTarget)
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly CircleShape _shape = new();
    private readonly LineShape _line = new();

    //**********************************************************
    //** methods:
    //**********************************************************

    public void Render(Ball ball)
    {
        var radius = ball.Size / 2;
        _shape.Radius = radius;
        _shape.Position = ball.Position;
        _shape.Origin = new Vector2f(radius, radius);
        _shape.FillColor = ball.Selected ? Color.Green : Color.Cyan;
        _shape.OutlineColor = Color.Black;
        _shape.OutlineThickness = -1;
        renderTarget.Draw(_shape);

        var dir = (ball.Velocity).Normalize();
        _line.P1 = _shape.Position;
        _line.P2 = _shape.Position + (dir * radius);

        renderTarget.Draw(new[]
        {
            new Vertex(_line.P1, Color.Black),
            new Vertex(_line.P2, Color.Black),
        }, 0, 2, PrimitiveType.Lines);
    }
}