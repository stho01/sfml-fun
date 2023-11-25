using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace BallCollisionSimple;

public class Ball
{
    public Vector2f Velocity { get; set; }
    public Vector2f Acceleration { get; set; }
    public Vector2f Position { get; set; }
    public float Radius { get; set; }
    public float Mass { get; set; } = 1f;


    public void ApplyForce(Vector2f force)
    {
            Acceleration += force / Mass;
        }
}
    
// m v1i + m v2i = m v1f + m v2f
// v1f = v1i + v2i - v2f
    
public class Game : GameBase
{
    private Ball _b1 = new Ball();
    private Ball _b2 = new Ball();
    private CircleShape _shape = new CircleShape();
    private FloatLine _line = new FloatLine();
    private bool _dragging = false;
        
    public Game(RenderWindow window) : base(window)
    {
            
        }

    public FloatCircle C1 => new FloatCircle(_b1.Position, _b1.Radius);
    public FloatCircle C2 => new FloatCircle(_b2.Position, _b2.Radius);
        
    public override void Initialize()
    {
            _b1.Position = new Vector2f(400, WindowHeight/2 + 78);
            _b1.Radius = 40;
            //_b1.Velocity = Vector2Extensions.Right * 100;
            _b1.Mass = 1f;
            
            _b2.Position = new Vector2f(600, WindowHeight/2);
            _b2.Radius = 40;
            _b2.Velocity = Vector2Extensions.Zero;
            //_b2.Velocity = Vector2Extensions.Left * 50;
        }

    protected override void Update()
    {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space)) Initialize();
            
            // mv1 + mv2 = (m1 + m2)vf
            if (C1.Intersects(C2))
                ResolveCollision(_b1, _b2);

            UpdatePositions();
            CheckBounds(_b1);
            CheckBounds(_b2);
            HandleMouseInput();
        }

    private void UpdatePositions()
    {
            _b1.Velocity += _b1.Acceleration;
            _b2.Velocity += _b2.Acceleration;
            
            _b1.Position += _b1.Velocity * Timer.DeltaTimeSeconds;
            _b2.Position += _b2.Velocity * Timer.DeltaTimeSeconds;

            _b1.Acceleration = Vector2Extensions.Zero;
            _b2.Acceleration = Vector2Extensions.Zero;
        }

    private void CheckBounds(Ball ball)
    {
            if (ball.Position.X - ball.Radius < 0f)
            {
                ball.Position = new Vector2f(ball.Radius, ball.Position.Y);
                ball.Velocity = ball.Velocity.Multiply(new Vector2f(-1f, 1));
            }
            if (ball.Position.X + ball.Radius > WindowWidth)
            {
                ball.Position = new Vector2f(WindowWidth - ball.Radius, ball.Position.Y);
                ball.Velocity = ball.Velocity.Multiply(new Vector2f(-1f, 1));
            }
            if (ball.Position.Y - ball.Radius < 0f)
            {
                ball.Position = new Vector2f(ball.Position.X, ball.Radius);
                ball.Velocity = ball.Velocity.Multiply(new Vector2f(1, -1f));
            }
            if (ball.Position.Y + ball.Radius > WindowHeight)
            {
                ball.Position = new Vector2f(ball.Position.X, WindowHeight - ball.Radius);
                ball.Velocity = ball.Velocity.Multiply(new Vector2f(1, -1f));
            }
        }
        
    private void HandleMouseInput()
    {
            var mousePos = (Vector2f) GetMousePosition();
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (C1.Contains(mousePos.X, mousePos.Y))
                {
                    if (!_dragging)
                    {
                        _line.P1 = _b1.Position;
                        _b1.Velocity = new Vector2f();
                    }
                }
                if (_line.P1 != Vector2Extensions.Zero)
                    _line.P2 = mousePos;
                
                _dragging = true;
            }
            else
            {
                var force = (_line.P1 - _line.P2);
                _b1.ApplyForce(force);
                
                _line.P1 = Vector2Extensions.Zero;
                _line.P2 = Vector2Extensions.Zero;
                _dragging = false;
            }
        }

    private void ResolveCollision(Ball b1, Ball b2)
    {
            var normal = b2.Position - b1.Position;
            var tangent = new Vector2f(-normal.Y, normal.X);
            var un = normal.Normalize();
            var ut = tangent.Normalize(); 
            
            var v1n = un.Dot(b1.Velocity);
            var t1n = ut.Dot(b1.Velocity);
            var v2n = un.Dot(b2.Velocity);
            var t2n = ut.Dot(b2.Velocity);

            var totalMass = b1.Mass + b2.Mass;
            var v1np = v1n * (b1.Mass - b2.Mass) + (2 * b2.Mass * v2n) / totalMass;
            var v2np = v2n * (b2.Mass - b1.Mass) + (2 * b1.Mass * v1n) / totalMass;

            var v1npVec = v1np * un;
            var v1tpVec = t1n * ut;
            var v2npVec = v2np * un;
            var v2tpVec = t2n * ut;

            b1.Velocity = v1npVec + v1tpVec;
            b2.Velocity = v2npVec + v2tpVec;
        }
      
    protected override void Render()
    {
            Draw(_b1, Color.Green);
            Draw(_b2, Color.White);
            
            Window.Draw(new[] {
                new Vertex(_line.P1, Color.Blue),
                new Vertex(_line.P2, Color.Red), 
            }, 0, 2, PrimitiveType.Lines);
        }

    private void Draw(Ball ball, Color color)
    {
            _shape.Radius = ball.Radius;
            _shape.Position = ball.Position;
            _shape.OutlineColor = color;
            _shape.OutlineThickness = -2;
            _shape.Origin = new Vector2f(ball.Radius, ball.Radius);
            Window.Draw(_shape);
        }
}