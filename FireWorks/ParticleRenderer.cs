using SFML.Graphics;
using SFML.System;

namespace FireWorks
{
    public class ParticleRenderer
    {
        //**********************************************************
        //** fields:
        //**********************************************************
    
        private readonly RenderTarget _renderTarget;
        public readonly CircleShape _particle = new CircleShape(1f)
        {
            Origin = new Vector2f(1f, 1f)
        };
          
        //**********************************************************
        //** ctor:
        //**********************************************************

        public ParticleRenderer(RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
        }
        
        //**********************************************************
        //** methods
        //**********************************************************

        public void Render(Particle particle)
        {
            if (particle.IsDead)
                return;
            
            var opacity = (byte)((particle.Age / particle.TotalLifetime) * 255);
            
            _particle.Position = particle.Position;
            _particle.FillColor = new Color(
                particle.R, 
                particle.G, 
                particle.B, 
                opacity);
            
            _renderTarget.Draw(_particle);
        }
    }
}