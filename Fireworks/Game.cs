using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace Fireworks;

public class Game : GameBase
{
    //**********************************************************
    //** fields
    //**********************************************************

    public readonly float Gravity = -9.81f * .75f;
    private readonly List<Rocket> _rockets = new List<Rocket>();
    private readonly List<Explosion> _explosions = new List<Explosion>();
    private readonly RocketSpawner _rocketSpawner;
    private readonly RocketUpdater _rocketUpdater;
    private readonly RocketRenderer _rocketRenderer;
    private readonly ExplosionRenderer _explosionRenderer;
    private readonly ExplosionUpdater _explosionUpdater;
    private readonly ExplosionSpawner _explosionSpawner;
    private readonly ParticleRenderer _explosionParticleRenderer;
    private readonly CircleShape _earth = new CircleShape(120, 60);
    private Sprite _earthSprite;
        
    //**********************************************************
    //** ctor:
    //**********************************************************
        
    public Game(RenderWindow window) : base(window)
    {
            _explosionParticleRenderer = new ParticleRenderer(Window)
            {
                FadeMode = ParticleFade.Exponential
            };
            _explosionRenderer = new ExplosionRenderer(window, _explosionParticleRenderer);
            _explosionUpdater  = new ExplosionUpdater(this);
            _explosionSpawner  = new ExplosionSpawner(this);
            _rocketSpawner     = new RocketSpawner(this);
            _rocketUpdater     = new RocketUpdater(this, _explosionSpawner);
            _rocketRenderer    = new RocketRenderer(window, new ParticleRenderer(Window));
        }

    //**********************************************************
    //** props:
    //**********************************************************

    public Rocket Rocket => _rockets.FirstOrDefault();
    public CircleShape Earth => _earth;
    public Range RocketSpawnTimeRange { get; set; } = 50..300;
    public Range RocketSpawnRange { get; set; } = 2..4;
    public float CurrentSpawnTimeAccumulator { get; set; } = 0f;
    public float CurrentSpawnTimer { get; set; } = 0f;

    public int ExplosionCount => _explosions.Count;
    public int RocketCount => _rockets.Count;

    public ParticleFade ExplosionParticleFadeMode
    {
        get => _explosionParticleRenderer.FadeMode;
        set => _explosionParticleRenderer.FadeMode = value;
    }

    //**********************************************************
    //** methods:
    //**********************************************************
        
    public override void Initialize()
    {
            ShowFps = true;
            ClearColor = new Color(0x000000AA);
            
            _earth.Origin = new Vector2f(_earth.Radius, _earth.Radius);
            _earth.Position = new Vector2f(WindowWidth/2, WindowHeight/2);
            _earth.FillColor = Color.Transparent;
            _earth.OutlineColor = Color.Green;
            _earth.OutlineThickness = -1;
            
            var texture = new Texture("Assets/earth.png");
            _earthSprite = new Sprite(texture);
            _earthSprite.Scale = new Vector2f(_earth.Radius / texture.Size.X*2, _earth.Radius / texture.Size.Y*2);
            _earthSprite.Position = _earth.Position - _earth.Origin;
        }

    public void AddRocket(Rocket rocket) => _rockets.Add(rocket);
    public void AddExplosion(Explosion explosion) => _explosions.Add(explosion);
        
    protected override void Update()
    {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                _explosionSpawner.Spawn(new Vector2f(WindowWidth/2, WindowHeight/2));
            
            _rockets.ForEach(_rocketUpdater.Update);
            _explosions.ForEach(_explosionUpdater.Update);
            
            _rockets.RemoveAll(r => r.IsDead);
            _explosions.RemoveAll(r => r.Done);
            
            IncrementRocketSpawnTimer();
            SpawnRocketsIfTime();
        }

    private void IncrementRocketSpawnTimer()
    {
            CurrentSpawnTimeAccumulator += Timer.DeltaTimeMilliseconds;   
        }
        
    private void SpawnRocketsIfTime()
    {
            if (CurrentSpawnTimeAccumulator >= CurrentSpawnTimer)
            {
                CurrentSpawnTimer = RandomNumber.Get(RocketSpawnTimeRange);
                var spawnCount = RandomNumber.Get(RocketSpawnRange);
                
                for (var i = 0; i < spawnCount; i++) 
                    SpawnRocket();

                CurrentSpawnTimeAccumulator = 0;
            }
        }

    protected override void Render()
    {
            Window.Draw(_earthSprite);
            _rockets.ForEach(_rocketRenderer.Render);
            _explosions.ForEach(_explosionRenderer.Render);
        }

    public void SpawnRocket()
    {
            _rocketSpawner.SpawnOnEarthSurface();
        }
}