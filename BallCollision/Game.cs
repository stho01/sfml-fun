using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace BallCollision
{
    public class Game : GameBase
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private readonly BallUpdater _updater;
        private readonly BallRenderer _renderer;
        private readonly List<Ball> _balls;
        private Ball _selected;
  
        //**********************************************************
        //** ctors:
        //**********************************************************

        public Game(RenderWindow window) : base(window)
        {
            _updater = new BallUpdater(this);
            _renderer = new BallRenderer(window);
            _balls = new List<Ball>();
        }
          
        //**********************************************************
        //** props
        //**********************************************************

        public Ball[] Balls => _balls.ToArray();
  
        //**********************************************************
        //** methods:
        //**********************************************************

        public override void Initialize()
        {
            for (var i = 0; i < 12; i++)
            {
                _balls.Add(new Ball
                {
                    Position = new Vector2f(50 + i * 100, 100),
                    Mass = RandomNumber.Get(10..50)
                });
            }
            
        }

        protected override void Update()
        {
            UpdateSelectedBallState();
            
            _balls.ForEach(_updater.Update);
            if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                for (var i = 0; i < _balls.Count; i++) {
                    _balls[i].Position = new Vector2f(50 + i * 100, 100);
                    _balls[i].Velocity = new Vector2f();
                }
        }

        private void UpdateSelectedBallState()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var mousePos = GetMousePosition();

                if (_selected == null)
                {
                    var selected = GetBallFromPoint(mousePos.X, mousePos.Y);
                    if (selected != null)
                    {
                        _selected = selected;
                        _selected.Select();
                    }    
                }
                else
                {
                    var delta = ((Vector2f) mousePos - _selected.Position);
                    _selected.ApplyForce(delta);
                }
            }
            else
            {
                _selected?.Deselect();
                _selected = null;
            }
        }

        private Ball GetBallFromPoint(int x, int y)
        {
            
            foreach (var ball in _balls)
            {
                var circle = new FloatCircle(ball.Position, ball.Radius);
                if (circle.Contains(x, y))
                {
                    return ball;
                }
            }

            return null;
        }
        
        protected override void Render()
        {
            _balls.ForEach(_renderer.Render);

            if (_selected != null)
            {
                var p1 = (Vector2f) GetMousePosition();
                var p2 = _selected.Position;
                    
                Window.Draw(new [] { 
                    new Vertex(p1, Color.Magenta), 
                    new Vertex(p2, Color.Magenta) 
                }, 0, 2, PrimitiveType.Lines);   
            }
        }
    }
}