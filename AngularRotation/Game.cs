using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace AngularRotation;

public class Game(RenderWindow window) : GameBase(window)
{
    private readonly RectangleShape _shape = new();
    private float _angularAcceleration = 0f;
    private float _angularVelocity = 0f;
    private float _angle = 0;
    private Vector2f? _p1 = null;
    private Vector2f? _p2 = null;
    private bool _dragging = false;
    private bool _applyingForce = false;
    private Vector2f? _pointOnShape = null;

    public override void Initialize()
    {
        _shape.Size = new Vector2f(150, 150);
        _shape.Position = WindowCenter;
        _shape.OutlineThickness = -1;
        _shape.OutlineColor = Color.Magenta;
        _shape.Origin = _shape.Size * .5f;
    }

    protected override void Update()
    {
        var mousePos = (Vector2f)GetMousePosition();
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            var floatRect = new FloatRect(_shape.Position - _shape.Size * .5f, _shape.Size);

            if (floatRect.Contains(mousePos.X, mousePos.Y))
            {
                _angularVelocity = 0f;

                if (!_dragging)
                    _p1 = mousePos;


                //_angularAcceleration = 10f * Timer.DeltaTimeSeconds;

                // var distanceFromCenter = _pointOnShape - _shape.Position;
                // var direction = (mousePos - _pointOnShape)?.ToPolarCoordinates();
                // _angularAcceleration = direction?.Angle
            }


            _dragging = true;
            _p2 = mousePos;
        }
        else
        {
            if (_p1.HasValue && _p2.HasValue)
            {
                // torque = r * F * sin(theta)
                // torque = r * sin(theta)

                var lever = _p1.Value - (_shape.Position - _shape.Size / 2);
                var force = (_p2 - _p1).Value;
                var r = lever.Length();

                var ll = lever.Length();
                var fl = force.Length();
                var llfl = ll * fl;

                var angle = llfl == 0 ? 0 : Math.Acos(lever.Dot(force) / (ll * fl));

                Console.WriteLine($"{angle}, {ll}, {fl}");
                var torque = r * Math.Sin(angle);

                _angularAcceleration += (float)torque;
            }


            _p1 = null;
            _p2 = null;
            _dragging = false;
        }


        _angularVelocity += _angularAcceleration;
        _angle += _angularVelocity * Timer.DeltaTimeSeconds;
        _shape.Rotation = _angle;
        _angularAcceleration = 0f;
    }

    protected override void Render()
    {
        Window.Draw(_shape);

        if (_p1.HasValue && _p2.HasValue)
        {
            Window.Draw(new[]
            {
                new Vertex(_p1.Value, Color.Green),
                new Vertex(_p2.Value, Color.Green)
            }, 0, 2, PrimitiveType.Lines);
        }
    }
}