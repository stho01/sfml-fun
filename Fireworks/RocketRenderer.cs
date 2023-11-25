using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Fireworks;

public class RocketRenderer(RenderTarget renderTarget, ParticleRenderer particleRenderer)
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly CircleShape _circleShape = new(1f, 3)
    {
        FillColor = Color.White
    };

    //**********************************************************
    //** methods:
    //**********************************************************

    public void Render(Rocket rocket)
    {
        var radius = rocket.Mass * 0.25f;
        _circleShape.Position = rocket.Position;
        _circleShape.Scale = new Vector2f(.5f, 1f);
        _circleShape.Rotation = rocket.Velocity.ToPolarCoordinates().Angle;
        _circleShape.Radius = radius;
        _circleShape.Origin = new Vector2f(radius, radius);
        renderTarget.Draw(_circleShape);

        rocket.Trail.ForEach(particleRenderer.Render);
    }
}