using System;
using System.Runtime.Intrinsics.X86;
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
        public Vector2f Wind { get; set; } = new Vector2f(10f, 0);
        public float Restitution { get; set; } = 0.9f; // Works as a fake restitution when collision is happening.
  
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
    
            if (ball.Position.X - radius / 2 < 0)
            {
                ball.Position = new Vector2f(radius, ball.Position.Y);
                ball.Velocity = new Vector2f(-ball.Velocity.X, ball.Velocity.Y);
                ball.Velocity *= Restitution;
            }

            if (ball.Position.X + radius > _game.WindowWidth)
            {
                ball.Position = new Vector2f(_game.WindowWidth - radius, ball.Position.Y);
                ball.Velocity = new Vector2f(-ball.Velocity.X, ball.Velocity.Y);
                ball.Velocity *= Restitution;
            }

            if (ball.Position.Y + radius > _game.WindowHeight)
            {
                ball.Position = new Vector2f(ball.Position.X, _game.WindowHeight - radius);
                ball.Velocity = new Vector2f(ball.Velocity.X, -ball.Velocity.Y);
                ball.Velocity *= Restitution;
            }
        }

        private void CheckBallCollisions(Ball ball)
        {
            foreach (var other in _game.Balls)
            {
                if (other != ball && Intersects(ball, other))
                {
                    var delta = other.Position - ball.Position;
                    var deltaNormalized = delta.Normalize();
                    var velocityLength = ball.Velocity.Length();

                    var totalMass = ball.Mass + other.Mass;
                    ball.Velocity *= (((ball.Mass - other.Mass) / totalMass) + ((2 * other.Mass) / totalMass)); // elastic collision
                    ball.Velocity = (ball.Velocity.Reflect(deltaNormalized) * velocityLength * Restitution);           // reflect current velocity against other ball
                    
                    var intersectionAmount = (delta.Length() - (ball.Radius + other.Radius)) / 2;          // calculate amount of intersection 
                    ball.Position += deltaNormalized * intersectionAmount;                                      // set position back to avoid objects to be glued together. 
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