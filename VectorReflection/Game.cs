using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace VectorReflection;

public class Game : GameBase
{
    private FloatLine _ray;
    private FloatLine _mirror;
    private float _mirrorAngle = 90f;
    private int _selectedPoint = 0;
        
    public Game(RenderWindow window) : base(window) { }

    public override void Initialize()
    {
        _ray = new FloatLine(
            WindowWidth, WindowHeight,
            WindowWidth/2, WindowHeight/2
        );
        _mirror = new FloatLine();
    }

    protected override void Update()
    {
        UpdateMirror();
            
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
        {
            _mirrorAngle += 180 * Timer.DeltaTimeSeconds;
        }

        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            _ray.P1 = (Vector2f) GetMousePosition();
                
            _ray.P2 = _ray.Intersects(_mirror) ?? _ray.P2;
        }
    }

    private void UpdateMirror()
    {
        var mirrorDirection = new Vector2f(
            (float)Math.Cos(MathUtils.DegreeToRadian(_mirrorAngle)),
            (float)Math.Sin(MathUtils.DegreeToRadian(_mirrorAngle))
        ); 
        var center = new Vector2f(WindowWidth / 2, WindowHeight / 2);
        var mirror = new FloatLine(
            center + (mirrorDirection * 1000),
            center + (mirrorDirection * -1000));
            
        _mirror = mirror;
    }
        
    protected override void Render()
    {
        RenderLine(_ray, Color.White);
        RenderLine(_mirror, Color.Red);

        var normalDir = _mirror.NormalDirection();
        var lineCenter = _mirror.Center();
        var normal = new FloatLine(lineCenter, lineCenter + (normalDir * 1000));
        RenderLine(normal, Color.Green);

        var intersectionPoint = _ray.Intersects(_mirror);
        if (intersectionPoint.HasValue)
        {
            var reflectedDir = _ray.Reflect(_mirror);
            var reflection = new FloatLine(
                intersectionPoint.Value, 
                intersectionPoint.Value + (reflectedDir * 1000)
            );
            RenderLine(reflection, Color.White);
        }
    }

    private void RenderLine(FloatLine line, Color color)
    {
        Window.Draw(new []
        {
            new Vertex(line.P1, color),
            new Vertex(line.P2, color), 
        }, 0, 2, PrimitiveType.Lines);
    }
}