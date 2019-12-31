using SFML.Graphics;

namespace Fireworks
{
    public class ExplosionRenderer
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private readonly RenderTarget _target;
        private readonly ParticleRenderer _particleRenderer;
          
        //**********************************************************
        //** ctor:
        //**********************************************************

        public ExplosionRenderer(RenderTarget target, ParticleRenderer particleRenderer)
        {
            _target = target;
            _particleRenderer = particleRenderer;
        }
          
        //**********************************************************
        //** methods:
        //**********************************************************

        public void Render(Explosion explosion)
        {
            foreach (var explosionParticle in explosion.Particles)
                _particleRenderer.Render(explosionParticle);
        }
    }
}