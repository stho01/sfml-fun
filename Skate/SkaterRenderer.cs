using SFML.Graphics;
using SFML.System;

namespace Skate;

public class SkaterRenderer(RenderTarget renderTarget)
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly CircleShape _skaterShape = new(10, 3);

    //**********************************************************
    //** methods:
    //**********************************************************

    public void Render(Skater skater)
    {
        var radius = skater.Mass * .9f;
        _skaterShape.Radius = radius;
        _skaterShape.Scale = new Vector2f(.6f, 1f);
        _skaterShape.Rotation = skater.Angle;
        _skaterShape.Origin = new Vector2f(radius, radius);
        _skaterShape.Position = skater.Position;
        _skaterShape.FillColor = Color.Blue;

        renderTarget.Draw(_skaterShape);
    }
}