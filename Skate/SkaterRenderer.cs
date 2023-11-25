using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Skate;

public class SkaterRenderer
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly RenderTarget _renderTarget;
    private readonly CircleShape _skaterShape = new CircleShape(10, 3);
  
    //**********************************************************
    //** ctor:
    //**********************************************************

    public SkaterRenderer(RenderTarget renderTarget)
    {
            _renderTarget = renderTarget;
        }
  
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
            
            _renderTarget.Draw(_skaterShape);
        }
}