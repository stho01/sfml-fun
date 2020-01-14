using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace BallCollision
{
    public class BallRenderer
    {
          
        //**********************************************************
        //** fields:
        //**********************************************************

        private readonly RenderTarget _renderTarget;
        private readonly CircleShape _shape = new CircleShape();
        private readonly LineShape _line = new LineShape();
        
        //**********************************************************
        //** ctors:
        //**********************************************************

        public BallRenderer(RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
        }
          
        //**********************************************************
        //** methods:
        //**********************************************************

        public void Render(Ball ball)
        {
            var radius = ball.Size / 2;
            _shape.Radius = radius;
            _shape.Position = ball.Position;
            _shape.Origin = new Vector2f(radius, radius);
            _shape.FillColor = Color.Cyan;
            _shape.OutlineColor = Color.Black;
            _shape.OutlineThickness = -1;
            _renderTarget.Draw(_shape);

            var dir = (ball.Velocity).Normalize();
            _line.P1 = _shape.Position;
            _line.P2 = _shape.Position + (dir * radius);
            
            _renderTarget.Draw(new []
            {
                new Vertex(_line.P1, Color.Black), 
                new Vertex(_line.P2, Color.Black), 
            }, 0, 2, PrimitiveType.Lines);
        }
    }
}