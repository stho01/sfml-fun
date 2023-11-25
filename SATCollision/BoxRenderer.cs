using System;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace SATCollision;

public class BoxRenderer
{
    private readonly RenderTarget _renderTarget;
    private readonly ConvexShape _shape;

    public BoxRenderer(RenderTarget renderTarget)
    {
        _renderTarget = renderTarget;
        _shape = new ConvexShape(4);
        _shape.SetPoint(0, new Vector2f(-.5f, -.5f));
        _shape.SetPoint(1, new Vector2f(.5f, -.5f));
        _shape.SetPoint(2, new Vector2f(.5f, .5f));
        _shape.SetPoint(3, new Vector2f(-.5f, .5f));
    }

    public void Draw(Box box)
    {
        var scale = new Vector2f(box.Width, box.Height);
        var color = box.Intersected ? Color.Red : Color.Green;

        _shape.Position = box.Position;
        _shape.Rotation = box.Rotation;
        _shape.Scale = scale;
        _shape.OutlineColor = color;
        _shape.FillColor = Color.Transparent;
        _shape.OutlineThickness = 1f / scale.X;
        _renderTarget.Draw(_shape);

        var point = _shape.GetPoint(0);
        point = RotateVector(point, new Vector2f(0f, 0f), (float)MathUtils.DegreeToRadian(box.Rotation));
        var first = point.Multiply(scale) + box.Position;

        _renderTarget.Draw(new[]
        {
            new Vertex(box.Position, color),
            new Vertex(first, color)
        }, 0, 2, PrimitiveType.Lines);
    }

    private Vector2f RotateVector(Vector2f vec, Vector2f origin, float radians)
    {
        return new Vector2f(
            (float)(vec.X * Math.Cos(radians) - vec.Y * Math.Sin(radians)),
            (float)(vec.X * Math.Sin(radians) + vec.Y * Math.Cos(radians))
        );
    }
}