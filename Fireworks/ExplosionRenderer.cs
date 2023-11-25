using SFML.Graphics;

namespace Fireworks;

public class ExplosionRenderer(RenderTarget target, ParticleRenderer particleRenderer)
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly RenderTarget _target = target;

    //**********************************************************
    //** ctor:
    //**********************************************************

    //**********************************************************
    //** methods:
    //**********************************************************

    public void Render(Explosion explosion)
    {
        foreach (var explosionParticle in explosion.Particles)
            particleRenderer.Render(explosionParticle);
    }
}