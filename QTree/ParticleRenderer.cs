using SFML.Graphics;
using SFML.System;

namespace QTree;

public class ParticleRenderer(RenderTarget renderTarget)
{
    private readonly CircleShape _particleShape = new();

    public void Render(Particle particle)
    {
        _particleShape.Radius = particle.Radius;
        _particleShape.Origin = new Vector2f(_particleShape.Radius, _particleShape.Radius);
        _particleShape.FillColor = particle.Colliding ? Color.Green : Color.White;
        _particleShape.Position = particle.Position;

        renderTarget.Draw(_particleShape);
    }
}