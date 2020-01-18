﻿using System;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace SimpleCollision
{
    public class Block
    {
        private readonly RectangleShape _shape;
        
        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; set; }
        public Vector2f Acceleration { get; set; }
        public float Mass { get; set; } = 1;

        public Block()
        {
            _shape = new RectangleShape(new Vector2f(100, 100))
            {
                FillColor = Color.Red,
                OutlineColor = Color.White,
                OutlineThickness = -1,
                Position = Position
            };
        }

        public RectangleShape Shape()
        {
            _shape.Position = Position;
            return _shape;
        }
        public FloatRect Bounds() => new FloatRect(Position, _shape.Size);

        public void Update()
        {
            Velocity += Acceleration;
            Position += Velocity * Timer.DeltaTimeSeconds;
            Acceleration = new Vector2f();
        }
    }
    
    public class Game : GameBase
    {
        private Block _block1 = new Block();
        private Block _block2 = new Block();
        
        public Game(RenderWindow window) : base(window) { }

        public override void Initialize()
        {
            _block1.Position = new Vector2f(400, WindowHeight/2);
            _block2.Position = new Vector2f(800, WindowHeight/2);
            _block2.Velocity = new Vector2f(-100, 0);
        }

        protected override void Update()
        {
            _block1.Update();
            _block2.Update();

            var bounds1 = _block1.Bounds();
            var bounds2 = _block2.Bounds();

            if (bounds1.Intersects(bounds2))
            {
                // var overlap = (_block1.Position.X + _block2.Position.X) -
                              // (_block1.Bounds() + _block2.Bounds());
                // _block1.Position = 
                
                _block1.Velocity = CalculateElasticCollision(_block1, _block2);
                _block2.Velocity = CalculateElasticCollision(_block2, _block1);
            }
        }

        public Vector2f CalculateElasticCollision(Block b1, Block b2)
        {
            var totalMass = _block1.Mass + _block2.Mass;
            var a = ((b1.Mass - b2.Mass) / totalMass) * b1.Velocity;
            var b = (2 * b2.Mass / totalMass) * b2.Velocity;

            return a + b;
        }

        protected override void Render()
        {
            Window.Draw(_block1.Shape());
            Window.Draw(_block2.Shape());
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var window = WindowFactory.CreateDefault();
            var game = new Game(window);
            game.Initialize();
            game.Start();
        }
    }
}