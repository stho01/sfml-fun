using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace RayCasting;

public class BallRenderer(RenderTarget renderTarget)
{
    private readonly CircleShape _circle = new(10)
    {
        Origin = new Vector2f(10, 10),
        FillColor = Color.White
    };

    public void Render(Ball ball, Shape[] shapes)
    {
        _circle.Position = ball.Position;
        renderTarget.Draw(_circle);

        var lines = ball.Cast(shapes);
        foreach (var floatLine in lines)
            RenderLine(floatLine);
    }

    private void RenderLine(FloatLine line)
    {
        renderTarget.Draw(new[]
        {
            new Vertex(line.P1),
            new Vertex(line.P2)
        }, 0, 2, PrimitiveType.Lines);
    }
}