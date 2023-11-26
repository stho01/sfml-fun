using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace QTree;

public class Game(RenderWindow window, int numberOfParticles) : GameBase(window)
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly List<Particle> _particles = new();
    private readonly ParticleRenderer _renderer = new(window);
    private QuadTree<Particle> _qtree;
    private float delay;

    //**********************************************************
    //** props
    //**********************************************************

    public IEnumerable<Particle> Particles => _particles;
    public bool RenderQuadTree { get; set; }
    public int LargestCluster { get; private set; }

    //**********************************************************
    //** methods:
    //**********************************************************

    public override void Initialize()
    {
        ShowFps = true;

        for (var i = 0; i < numberOfParticles; i++)
        {
            var pos = RandomNumber.Vector(0, (int)Window.Size.X, 0, (int)Window.Size.Y);
            _particles.Add(new Particle
            {
                OriginalPosition = pos,
                Position = pos
            });
        }
    }

    protected override void Update()
    {
        _qtree = new QuadTree<Particle>(new FloatRect(0, 0, Window.Size.X, Window.Size.Y));

        _particles.ForEach(p =>
        {
            p.Colliding = false;
            _qtree.Insert(p);
        });

        delay += (float)Timer.DeltaTimeMilliseconds;
        if (delay >= 200)
        {
            _particles.ForEach(p => { p.Position = p.OriginalPosition + RandomNumber.Vector(-5, 5); });
            delay = 0;
        }

        foreach (var particle in _particles)
        {
            var queryBounds = new FloatRect(particle.Position - new Vector2f(particle.Radius, particle.Radius), new Vector2f(particle.Radius * 3, particle.Radius * 3));
            var neighbors = _qtree.QueryRange(queryBounds);

            foreach (var other in neighbors)
            {
                if (other == particle) continue;

                if (!particle.Colliding)
                    particle.Colliding = particle.Intersects(other);
            }

            LargestCluster = Math.Max(neighbors.Length, LargestCluster);
        }
    }

    public bool Intersects(Particle p1, Particle p2)
    {
        var sqrLength = (p2.Position - p1.Position).SqrLength();
        var sqrRadius = (p2.Radius + p1.Radius) * (p2.Radius + p1.Radius);

        return sqrLength <= sqrRadius;
    }

    protected override void Render()
    {
        if (RenderQuadTree)
        {
            var boundaries = _qtree.GetBoundaries();
            foreach (var floatRect in boundaries)
            {
                var shape = new RectangleShape
                {
                    Position = new Vector2f(floatRect.Left, floatRect.Top),
                    Size = new Vector2f(floatRect.Width, floatRect.Height),
                    FillColor = Color.Transparent,
                    OutlineColor = Color.Yellow,
                    OutlineThickness = 1,
                    Origin = new Vector2f(0f, 0f)
                };
                Window.Draw(shape);
            }
        }

        _particles.ForEach(_renderer.Render);
    }
}