using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace CircleIntersection;

public class Game(RenderWindow window) : GameBase(window)
{
    public CircleShape c1;
    public CircleShape c2;
    public Vertex[] line;

    public float SqrDistanceFromEachOther => (c2.Position - c1.Position).SqrLength();
    public float SqrRadius => (c1.Radius + c2.Radius) * (c1.Radius + c2.Radius);
    public bool Colliding => SqrDistanceFromEachOther <= SqrRadius;
    public int Radius { get; set; } = 60;

    public override void Initialize()
    {
        c1 = new CircleShape
        {
            Position = new Vector2f(50, 50),
            Radius = 30,
            Origin = new Vector2f(30, 30)
        };

        c2 = new CircleShape
        {
            Position = new Vector2f(Window.Size.X / 2f, Window.Size.Y / 2f),
            Radius = 60,
            Origin = new Vector2f(60, 60),
            FillColor = Color.Blue
        };

        line = new[]
        {
            new Vertex(c1.Position),
            new Vertex(c2.Position),
        };
    }

    protected override void Update()
    {
        c2.Radius = Radius;
        c2.Origin = new Vector2f(Radius, Radius);

        c1.Position = (Vector2f)GetMousePosition();
        c1.FillColor = Colliding ? Color.Red : Color.White;

        if (Colliding)
        {
            var dir = (c1.Position - c2.Position).Normalize();
            c1.Position = c2.Position + (dir * (c1.Radius + c2.Radius));
        }

        line[0] = new Vertex(c1.Position);
        line[1] = new Vertex(c2.Position);
    }

    protected override void Render()
    {
        Window.Draw(c2);
        Window.Draw(c1);
        Window.Draw(line, 0, 2, PrimitiveType.Lines);
    }
}