using System;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace BallCollision
{
    public class BallUpdater
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private readonly Game _game;
  
        //**********************************************************
        //** ctors:
        //**********************************************************

        public BallUpdater(Game game)
        {
            _game = game;
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public Vector2f Gravity { get; set; } = new Vector2f(0, 1f);
        public Vector2f Wind { get; set; } = new Vector2f(1f, 0);
  
        //**********************************************************
        //** methods:
        //**********************************************************
        
        public void Update(Ball ball)
        {
            ball.ApplyForce(Gravity * ball.Mass);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                ball.ApplyForce(Wind);
            
            ball.Velocity += ball.Acceleration;
            ball.Position += ball.Velocity * Timer.DeltaTimeSeconds;

            
            CheckBoundary(ball);
            CheckBallCollisions(ball);

            
            ball.ResetAcceleration();
        }

        private void CheckBoundary(Ball ball)
        {
            var radius = ball.Size / 2;
            
            if (ball.Position.X - radius < 0
             || ball.Position.X + radius > _game.WindowWidth
             || ball.Position.Y + radius > _game.WindowHeight)
            {
                // ball.Velocity = ball.Velocity.Reflect()
            }
            
            
            if (ball.Position.X - radius / 2 < 0)
            {
                ball.Position = new Vector2f(radius, ball.Position.Y);
                ball.Velocity = new Vector2f(-ball.Velocity.X, ball.Velocity.Y);
            }

            if (ball.Position.X + radius > _game.WindowWidth)
            {
                ball.Position = new Vector2f(_game.WindowWidth - radius, ball.Position.Y);
                ball.Velocity = new Vector2f(-ball.Velocity.X, ball.Velocity.Y);
            }

            if (ball.Position.Y + radius > _game.WindowHeight)
            {
                ball.Position = new Vector2f(ball.Position.X, _game.WindowHeight - radius);
                ball.Velocity = new Vector2f(ball.Velocity.X, -ball.Velocity.Y);
            }
        }

        private void CheckBallCollisions(Ball ball)
        {
            foreach (var other in _game.Balls)
            {
                if (other != ball && Intersects(ball, other))
                {
                    // Colliding 
                    Console.WriteLine("Colliding");
                    
                }    
            }
        }

        private bool Intersects(Ball b1, Ball b2)
        {
            var distance = (b2.Position - b1.Position).Length();
            
            return distance <= b1.Radius + b2.Radius;
        }
        
    }
}