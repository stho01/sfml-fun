using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Springs;

public class Spring
{
    private Bob? _connected;

    private readonly Shape _anchor = new RectangleShape(new Vector2f(10f, 10f))
    {
        OutlineColor = new Color(0xff, 0xff, 0xff),
        OutlineThickness = 1f,
        FillColor = new Color(0xff, 0xff, 0xff, 100),
        Origin = new Vector2f(5f, 5f)
    };
    
    public Vector2f Anchor { get; set; }
    public float Length { get; set; }
    public float K { get; set; }
    

    public void Connect(Bob o)
    {
        _connected = o;
        
        var force = o.Position - Anchor;
        var d = force.Length();
        
        var stretch = d - Length;
        var forceMagnitude = -K * stretch;
        
        force = force.Normalize() * forceMagnitude;
        
        o.ApplyForce(force);
    }

    public void Render(RenderWindow window)
    {
        if (_connected is not null)
        {
            window.Draw([
                new Vertex(Anchor), 
                new Vertex(_connected.Position)
            ], PrimitiveType.Lines);    
        }
        
        
        _anchor.Position = Anchor;
        window.Draw(_anchor);
    }
}