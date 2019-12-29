using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace LineCircleIntersection
{
    public class Game : GameBase
    {
          
        //**********************************************************
        //** fields:
        //**********************************************************
        
        private FloatLine _line1;
        private FloatLine _line2;
        private CircleShape _circleShape;
        private CircleShape _pointShape;
        private List<Vector2f> _intersectionPoints; 
          
        //**********************************************************
        //** ctor:
        //**********************************************************
        
        public Game(RenderWindow window) : base(window)
        {
            
        }
          
        //**********************************************************
        //** methods:
        //**********************************************************

        public override void Initialize()
        {
            
            _pointShape = new CircleShape(5f)
            {
                Origin = new Vector2f(5f, 5f),
                FillColor = Color.Red
            };
            
            _circleShape = new CircleShape(100) {
                Origin = new Vector2f(100, 100),
                Position = new Vector2f(Window.Size.X * .5f, Window.Size.Y * .5f),
                FillColor = Color.Transparent,
                OutlineColor = Color.White,
                OutlineThickness = 1
            };
            
            // _line = new FloatLine(200, Window.Size.Y *.5f, Window.Size.X * 0.7f, Window.Size.Y * 0.6f);
            _line1 = new FloatLine(200, Window.Size.Y *.5f, 300, Window.Size.Y * 0.5f);
            _line2 = new FloatLine(200, Window.Size.Y *.55f, 900, Window.Size.Y * 0.55f);
            
        }

        protected override void Update()
        {
            _intersectionPoints = new List<Vector2f>();
            
            var circle = new FloatCircle(_circleShape.Position, _circleShape.Radius);

            // _line.P2 = (Vector2f) GetMousePosition();
            
            _intersectionPoints.AddRange(_line1.Intersects(circle));
            _intersectionPoints.AddRange(_line2.Intersects(circle));
        }

        protected override void Render()
        {
            Window.Draw(_circleShape);
            
            Window.Draw(new []
            {
                new Vertex(_line1.P1), 
                new Vertex(_line1.P2), 
            }, 0, 2, PrimitiveType.Lines);
            
            Window.Draw(new []
            {
                new Vertex(_line2.P1), 
                new Vertex(_line2.P2), 
            }, 0, 2, PrimitiveType.Lines);

            
            _intersectionPoints?.ForEach(p =>
            {
                _pointShape.Position = p;
                Window.Draw(_pointShape);
            });
        }
    }
}