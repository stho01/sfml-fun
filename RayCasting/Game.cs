using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace RayCasting
{
    public class Game : GameBase
    {
        //**********************************************************
        //** fields:
        //**********************************************************
        
        private readonly List<Shape> _shapes = new List<Shape>();
        private Ball _ball;
        private readonly BallRenderer _ballRenderer;
        private CircleShape _circle;
        
        //**********************************************************
        //** ctor:
        //**********************************************************
    
        public Game(RenderWindow window) : base(window)
        {
            _ballRenderer = new BallRenderer(window);
        }

        public Vector2f? Intersection { get; set; }

        public override void Initialize()
        {
            ShowFps = true;
            CreateShapes();
            
            _ball = new Ball() {
                Position = new Vector2f(100, 250)
            };
        }     

        protected override void Update()
        {
            _ball.Position = (Vector2f) GetMousePosition();
        }

        protected override void Render()
        {
            _shapes.ForEach(shape =>
            {
                if (shape is LineShape line)
                {
                    // TODO: Implement LineShape draw method right.
                    Window.Draw(new[] {
                        new Vertex(line.P1, Color.Red),
                        new Vertex(line.P2, Color.Red),
                    }, 0, 2, PrimitiveType.Lines);
                }
                else
                {
                    Window.Draw(shape);
                }
            });
            
            Window.Draw(_circle);
            
            _ballRenderer.Render(_ball, _shapes.ToArray());
        }

        public void Reset()
        {
            _shapes.Clear();
            CreateShapes();
        }

        private void CreateShapes()
        {
            for (var i = 0; i < 3; i++)
            {
                _shapes.Add(new LineShape(
                    RandomNumber.Vector(400, 800, 157, 507),
                    RandomNumber.Vector(400, 800, 157, 507)
                ));
            }
            _shapes.Add(new LineShape(0, 0, Window.Size.X, 0));
            _shapes.Add(new LineShape(0, 0, 0, Window.Size.Y));
            _shapes.Add(new LineShape(Window.Size.X, 0, Window.Size.X, Window.Size.Y));
            _shapes.Add(new LineShape(0, Window.Size.Y, Window.Size.X, Window.Size.Y));
            
            _shapes.Add(new RectangleShape(new Vector2f(100, 100))
            {
                FillColor = Color.Red,
                OutlineColor = Color.White,
                OutlineThickness = 2,
                Position = RandomNumber.Vector(  
                    100, 
                    (int) (Window.Size.X - 100),
                    100,
                    (int) (Window.Size.Y - 100))
            });

            var radius = 50;
            _circle = new CircleShape(radius);
            _circle.FillColor = Color.Red;
            _circle.OutlineColor = Color.White;
            _circle.OutlineThickness = 2;
            _circle.Origin = new Vector2f(radius, radius);
            _circle.Position = RandomNumber.Vector(
                radius, 
                (int) (Window.Size.X - radius),
                radius,
                (int) (Window.Size.Y - radius));
            
            _shapes.Add(_circle);
        }
    }
}