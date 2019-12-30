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
        private readonly CircleShape _earth = new CircleShape(50, 60);
        
        //**********************************************************
        //** ctor:
        //**********************************************************
        
        public Game(RenderWindow window) : base(window)
        {
            _rocketSpawner = new RocketSpawner(this);
            _rocketUpdater = new RocketUpdater(this);
            _rocketRenderer = new RocketRenderer(window);
        }

        //**********************************************************
        //** props:
        //**********************************************************

        public Rocket Rocket => _rockets.FirstOrDefault();
        public CircleShape Earth => _earth;

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
            
            SpawnRocket();
            SpawnRocket();
            SpawnRocket();
        }

        public void AddRocket(Rocket rocket) => _rockets.Add(rocket);
        
        protected override void Update()
        {
            _rockets.ForEach(_rocketUpdater.Update);
            _rockets.RemoveAll(r => r.Done);
        }

        protected override void Render()
        {
            Window.Draw(_earth);
            _rockets.ForEach(_rocketRenderer.Render);
        }

        public void SpawnRocket()
        {
            _rocketSpawner.SpawnOnEarthSurface();
        }
    }
}