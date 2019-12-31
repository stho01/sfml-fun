using System;
using SFML.Graphics;
using SFML.System;

namespace FireWorks
{
    public enum ParticleFade
    {
        Linear,
        Exponential
    }
    
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
        //** props:
        //**********************************************************

        public ParticleFade FadeMode { get; set; }
        
        //**********************************************************
        //** methods
        //**********************************************************

        public void Render(Particle particle)
        {
            if (particle.IsDead) return;

            double opacity = 255f;
            
            if (FadeMode == ParticleFade.Linear)
            {
                opacity = 255 - ((particle.Age / particle.TotalLifetime) * 255);    
            }
            else
            {
                var t = (particle.Age / particle.TotalLifetime);
                opacity = 255 - (Math.Pow(2f, t) * 255);
            }
          
            _particle.Position = particle.Position;
            _particle.FillColor = new Color(
                particle.R, 
                particle.G, 
                particle.B, 
                (byte)opacity);
            
            _renderTarget.Draw(_particle);
        }
    }
}