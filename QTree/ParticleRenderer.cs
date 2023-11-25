using SFML.Graphics;
using SFML.System;

namespace QTree;

public class ParticleRenderer
{
    private readonly RenderTarget _renderTarget;
    private readonly CircleShape _particleShape = new CircleShape();

    public ParticleRenderer(RenderTarget renderTarget)
    {
            _renderTarget = renderTarget;
        }
        
    public void Render(Particle particle)
    {
            _particleShape.Radius = particle.Radius;
            _particleShape.Origin = new Vector2f(_particleShape.Radius, _particleShape.Radius);
            _particleShape.FillColor = particle.Colliding ? Color.Green : Color.White;
            _particleShape.Position = particle.Position;
            
            _renderTarget.Draw(_particleShape);
        }
}