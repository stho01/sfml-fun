using System;
using SFML.System;
using Stho.SFML.Extensions;

namespace SATCollision;

public class Box(float width, float height)
{
    public float Width { get; } = width;
    public float Height { get; } = height;
    public bool Intersected { get; set; }

    public float Left => Width * -0.5f;
    public float Right => Width * 0.5f;
    public float Top => Height * -0.5f;
    public float Bottom => Height * 0.5f;

    public Vector2f Position { get; set; }
    public float Rotation { get; set; }

    public Vector2f[] GetPoints()
    {
        var radians = (float)MathUtils.DegreeToRadian(Rotation);

        return [
            RotateVector(new(Left, Top), radians) + Position,
            RotateVector(new(Right, Top), radians) + Position,
            RotateVector(new(Right, Bottom), radians) + Position,
            RotateVector(new(Left, Bottom), radians) + Position
        ];
    }

    private Vector2f RotateVector(Vector2f vec, float radians)
    {
        return new Vector2f(
            (float)(vec.X * Math.Cos(radians) - vec.Y * Math.Sin(radians)),
            (float)(vec.X * Math.Sin(radians) + vec.Y * Math.Cos(radians))
        );
    }
}