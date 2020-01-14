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
            _balls.ForEach(_updater.Update);
            if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                for (var i = 0; i < _balls.Count; i++) {
                    _balls[i].Position = new Vector2f(50 + i * 100, 100);
                    _balls[i].Velocity = new Vector2f();
                }
        }

        protected override void Render()
        {
            _balls.ForEach(_renderer.Render);
        }
    }
}