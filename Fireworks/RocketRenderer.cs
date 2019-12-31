using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Fireworks
{
    public class RocketRenderer
    {
        //**********************************************************
        //** fields:
        //**********************************************************
        
        private readonly RenderTarget _renderTarget;
        private readonly ParticleRenderer _particleRenderer;

        private readonly CircleShape _circleShape = new CircleShape(1f, 3)
        {
            FillColor = Color.White
        };
        
        //**********************************************************
        //** ctors:
        //**********************************************************
        
        public RocketRenderer(RenderTarget renderTarget, ParticleRenderer particleRenderer)
        {
            _renderTarget = renderTarget;
            _particleRenderer = particleRenderer;
        }

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
            _renderTarget.Draw(_circleShape);
            
            rocket.Trail.ForEach(_particleRenderer.Render);
        }
    }
}