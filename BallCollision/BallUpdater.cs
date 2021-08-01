using System.Collections.Generic;
using SFML.Graphics;
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
            // We need to delay mutation of the velocities until after 
            // the elastic collision calculation in order to use the correct variables for 
            // the other ball. 
            var updatedVelocities = new List<(Ball, Vector2f)>();
            
            foreach (var other in _game.Balls)
            {
                if (other != ball && Intersects(ball, other))
                {
                    var delta = other.Position - ball.Position;
                    var deltaNormalized = delta.Normalize();
                    var velocityLength = ball.Velocity.Length();

                    ball.Velocity = ball.Velocity.Multiply(CalculateElasticCollision(ball, other));
                    var newVelocity = (ball.Velocity.Reflect(deltaNormalized) * velocityLength * Restitution);     // reflect current velocity against other ball
                    
                    updatedVelocities.Add( 
                        (ball, newVelocity) 
                    );
                    
                    var overlap = (delta.Length() - (ball.Radius + other.Radius)) / 2;          // calculate amount of intersection 
                    ball.Position += deltaNormalized * overlap;                                      // set position back to avoid objects to be glued together. 
                }    
            }
            
            // We can now update velocities for colliding objects. 
            updatedVelocities.ForEach((tuple => { tuple.Item1.Velocity = tuple.Item2; }));
        }

        public Vector2f CalculateElasticCollision(Ball b1, Ball b2)
        {
            return CollisionHelper.CalculateElasticCollision(
                b1.Mass,
                b1.Velocity,
                b2.Mass,
                b2.Velocity
            );
        }

        private bool Intersects(Ball b1, Ball b2)
        {
            var distance = (b2.Position - b1.Position).Length();
            
            return distance <= b1.Radius + b2.Radius;
        }
        
    }
}