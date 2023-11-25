using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace CirclePacking;

public class Game : GameBase
{
    public const uint MaxNumberOfElements = 2_000;
        
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly CircleShape _circleShape = new CircleShape(1, 50) {
        FillColor = Color.Transparent,
        OutlineColor = Color.White,
        OutlineThickness = 2
    };
    private readonly List<Circle> _circles = new List<Circle>();
    private float _time = 0;
    private bool _unableToSpawnMoreCircles = false;
    private readonly List<Vector2f> _positions = new List<Vector2f>();
        
    //**********************************************************
    //** ctor:
    //**********************************************************

    public Game(RenderWindow window) : base(window)
    {
            
        }
          
    //**********************************************************
    //** props:
    //**********************************************************

    public float SpawnRate { get; set; } = 50f; // ms 
    public uint SpawnCount { get; set; } = 10;
    public int WindowSpawnPadding { get; set; } = 5;
        
    /// <summary>Grow px pr sec</summary>
    public float GrowRate { get; set; } = 60f;

    public int CircleCount => _circles.Count;
    public bool UseTexture { get; set; } = true;
          
    //**********************************************************
    //** methods:
    //**********************************************************

    public override void Initialize()
    {
            ShowFps = true;
            
            if (UseTexture)
            {
                var image = new Image("2020.png");
                for (uint x = 0; x < image.Size.X; x++)
                    for (uint y = 0; y < image.Size.Y; y++)
                        if (image.GetPixel(x, y) == Color.White)
                            _positions.Add(new Vector2f(x, y));
            }
            else
            {
                for (var x = 0; x < WindowWidth; x++)
                    for (var y = 0; y < WindowHeight; y++)
                        _positions.Add(new Vector2f(x, y));
            }
        }

    protected override void Update()
    {
            _time += Timer.DeltaTimeMilliseconds;
            if (_time > SpawnRate)
            {
                Spawn();
                _time = 0f;
            }
            
            _circles
                .Where(c => c.Growing)
                .ToList()
                .ForEach(c => {
                    
                    c.Growing = c.Position.X + c.Radius < WindowWidth
                                && c.Position.X - c.Radius > 0
                                && c.Position.Y + c.Radius < WindowHeight
                                && c.Position.Y - c.Radius > 0
                                && !IntersectOther(c);
                    
                    if (c.Growing)
                        c.Radius += GrowRate * Timer.DeltaTimeSeconds;
                  
                });
        }

    private bool IntersectOther(Circle c)
    {
            return _circles.Any(other =>
            {
                if (other == c)
                    return false;

                var sqrLength = (other.Position - c.Position).Length();
                var sqrDistance = (other.Radius) + (c.Radius);
                return sqrLength <= sqrDistance + 4;
            });
        }
        
    private void Spawn()
    {
            if (_unableToSpawnMoreCircles || CircleCount >= MaxNumberOfElements)
                return;
            
            for (var i = 0; i < SpawnCount; i++)
            {
                if (!TryGetSpawnLocation(out Vector2f pos))
                {
                    _unableToSpawnMoreCircles = true;
                    Console.WriteLine("Unable to spawn more circles... spawning of circles are disabled...");
                    return;
                }
                
                var circle = new Circle
                {
                    Growing = true,
                    Position = pos,
                    Radius = 1f
                };
                _circles.Add(circle);
            }
        }

    private bool TryGetSpawnLocation(out Vector2f pos)
    {
            var retryCount = 0;
            do
            {
                pos = _positions[RandomNumber.Get(0, _positions.Count - 1)];
            } while (PointInsideAnyCircle(pos) && ++retryCount < 100_000);
            
            if (retryCount == 100_000 - 1)
            {
                Console.WriteLine("Unable to spawn more circles... spawning of circles are disabled...");
                return false;
            }

            return true;
        }

    private bool PointInsideAnyCircle(Vector2f point)
    {
            return _circles.Any(c => (c.Position - point).Length() <= (c.Radius) + 2 );
        }

    protected override void Render()
    {
            _circles.ForEach(c =>
            {
                _circleShape.Position = c.Position;
                _circleShape.Radius = c.Radius;
                _circleShape.Origin = new Vector2f(c.Radius, c.Radius);
                Window.Draw(_circleShape);
            });
        }
}