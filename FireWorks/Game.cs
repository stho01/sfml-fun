using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace FireWorks
{
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
        private readonly CircleShape _earth = new CircleShape(70, 60);
        
        //**********************************************************
        //** ctor:
        //**********************************************************
        
        public Game(RenderWindow window) : base(window)
        {
            var particleRenderer = new ParticleRenderer(window);
            _explosionRenderer = new ExplosionRenderer(window, particleRenderer);
            _explosionUpdater  = new ExplosionUpdater(this);
            _explosionSpawner  = new ExplosionSpawner(this);
            _rocketSpawner     = new RocketSpawner(this);
            _rocketUpdater     = new RocketUpdater(this, _explosionSpawner);
            _rocketRenderer    = new RocketRenderer(window, particleRenderer);
        }

        //**********************************************************
        //** props:
        //**********************************************************

        public Rocket Rocket => _rockets.FirstOrDefault();
        public CircleShape Earth => _earth;
        public Range RocketSpawnTimeRange { get; set; } = 100..1000;
        public Range RocketSpawnRange { get; set; } = 3..20;
        public float CurrentSpawnTimeAccumulator { get; set; } = 0f;
        public float CurrentSpawnTimer { get; set; } = 0f;
        

        //**********************************************************
        //** methods:
        //**********************************************************
        
        public override void Initialize()
        {
            ShowFps = true;
            _earth.Origin = new Vector2f(_earth.Radius, _earth.Radius);
            _earth.Position = new Vector2f(WindowWidth/2, WindowHeight/2);
            _earth.FillColor = Color.Transparent;
            _earth.OutlineColor = Color.Green;
            _earth.OutlineThickness = -1;
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

            SpawnRocketsIfTime();
        }

        private void SpawnRocketsIfTime()
        {
            CurrentSpawnTimeAccumulator += Timer.DeltaTimeMilliseconds;
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
            Window.Draw(_earth);
            _rockets.ForEach(_rocketRenderer.Render);
            _explosions.ForEach(_explosionRenderer.Render);
        }

        public void SpawnRocket()
        {
            _rocketSpawner.SpawnOnEarthSurface();
            
        }
    }
}