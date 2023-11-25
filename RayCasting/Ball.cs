using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace RayCasting;

public class Ball
{
    //**********************************************************
    //** fields:
    //**********************************************************

    public readonly List<Ray> _rays = new();

    //**********************************************************
    //** ctor:
    //**********************************************************

    public Ball()
    {
        for (var i = 0f; i <= 360; i += .5f)
        {
            _rays.Add(new Ray
            {
                Position = Position,
                Direction = new Vector2f(
                    (float)Math.Cos(MathUtils.DegreeToRadian(i)),
                    (float)Math.Sin(MathUtils.DegreeToRadian(i))
                )
            });
        }
    }

    //**********************************************************
    //** props:
    //**********************************************************

    public Vector2f Position { get; set; }
    public Ray[] Rays => _rays.ToArray();

    //**********************************************************
    //** methods:
    //**********************************************************

    public FloatLine[] Cast(Shape[] shapes)
    {
        var lines = new List<FloatLine>();

        for (var i = 0; i < _rays.Count; i++)
        {
            var ray = _rays[i];
            ray.Position = Position;
            var intersection = ray.Cast(shapes);

            if (intersection.HasValue)
            {
                lines.Add(new FloatLine(Position, intersection.Value));
            }
            else
            {
                lines.Add(new FloatLine(Position, Position + ray.Direction * 20f));
            }
        }

        return lines.ToArray();
    }
}