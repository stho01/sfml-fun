using System;
using SFML.System;

namespace Stho.SFML.Extensions;

public struct FloatCircle(float x, float y, float radius)
{
    //**********************************************************
    //** ctor:
    //**********************************************************

    public FloatCircle(Vector2f pos, float radius) : this(pos.X, pos.Y, radius) { }

    //**********************************************************
    //** props:
    //**********************************************************

    public float X { get; set; } = x;
    public float Y { get; set; } = y;
    public float Radius { get; set; } = radius;

    //**********************************************************
    //** methods:
    //**********************************************************

    public bool Intersects(FloatCircle circle)
    {
        return Intersects(circle, out _);
    }

    public bool Intersects(FloatCircle circle, out float overlap)
    {
        var dx = circle.X - X;
        var dy = circle.Y - Y;
        var mSquared = dx * dx + dy * dy;
        var r = Radius + circle.Radius;
        var rSquared = r * r;


        overlap = r - (float)Math.Sqrt(mSquared);

        return mSquared <= rSquared;
    }

    public bool Contains(float x, float y)
    {
        return Intersects(new FloatCircle(x, y, 1));
    }

    //**********************************************************
    //** operator overloads:
    //**********************************************************

    public static implicit operator FloatCircle(Vector2f vec)
    {
        return new FloatCircle
        {
            X = vec.X,
            Y = vec.Y,
            Radius = 1f
        };
    }

    public static explicit operator Vector2f(FloatCircle circle)
    {
        return new Vector2f
        {
            X = circle.X,
            Y = circle.Y
        };
    }
}