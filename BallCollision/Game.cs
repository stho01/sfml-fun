using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
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
            _balls.Add(new Ball
            {
                Position = new Vector2f(100, 100),
                Mass = 10
            });
            
            _balls.Add(new Ball
            {
                Position = new Vector2f(200, 100),
                Mass = 20
            });
        }

        protected override void Update()
        {
            _balls.ForEach(_updater.Update);
        }

        protected override void Render()
        {
            _balls.ForEach(_renderer.Render);
        }
    }
}