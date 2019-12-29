using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Flocking
{
    public class FlockingBehaviour : GameBase
    {
        private readonly int _numberOfAgents;
        private readonly AgentUpdater _agentUpdater;
        private readonly AgentRenderer _agentRenderer;
        private readonly List<Agent> _agents;
        private readonly GameFpsRenderer _gameFpsRenderer;
        private QuadTree<Agent> _qtree;

        public FlockingBehaviour(RenderWindow window, int numberOfAgents) : base(window)
        {
            _numberOfAgents = numberOfAgents;
            _agentUpdater = new AgentUpdater(this);
            _agentRenderer = new AgentRenderer(this, window);
            _agents = new List<Agent>();
            _gameFpsRenderer = new GameFpsRenderer(window);
        }

        public bool ShowCollider { get; set; }
        public bool ShowNeighborhood { get; set; } = true;
        public Agent SelectedAgent { get; private set; }
        public List<Agent> Agents => _agents;
        public bool RenderQuadTree { get; set; }
        public bool UseQuadTree { get; set; } = true;
        public uint QuadTreeCapacity { get; set; } = 4;

        public bool ShowFps
        {
            get => _gameFpsRenderer.ShowFps;
            set => _gameFpsRenderer.ShowFps = value;
        }

        public float AgentSpeed
        {
            get => _agentUpdater.MaxSpeed;
            set => _agentUpdater.MaxSpeed = value;
        }
        
        public override void Initialize()
        {
            ApplyRandomPositionAndDirectionToAgents();
        }

        public void Reset()
        {
            ShowCollider = false;
            AgentSpeed = 20f;
            _agents.Clear();
            ApplyRandomPositionAndDirectionToAgents();
        }

        private void ApplyRandomPositionAndDirectionToAgents()
        {
            for (var i = 0; i < _numberOfAgents; i++)
                SpawnAgent();
        }

        public void SpawnAgent()
        {
            _agents.Add(new Agent
            {
                Pos = RandomVector.Get(0, Program.ScreenWidth, 0, Program.ScreenHeight),
                Acceleration = RandomVector.GetAndReflect(1, 10).Normalize()
            });
        }

        public void RemoveLastAgent()
        {
            _agents.Remove(_agents.Last());
        }
        
        public Agent[] GetNeighbors(Agent a)
        {
            if (UseQuadTree)
            {
                return _qtree
                    .QueryRange(new FloatCircle(a.Pos, a.NeighborhoodRadius))
                    .Where(x => x != a)
                    .ToArray();    
            }

            return FindNeighbors(a);
        }
        
        
        public Agent[] FindNeighbors(Agent agent)
        {
            return Agents
                .Where(potentialNeighbor => IsNeighbor(agent, potentialNeighbor))
                .ToArray();
        }

        public bool IsNeighbor(Agent agent, Agent potentialNeighbor)
        {
            return potentialNeighbor != agent
                   && (potentialNeighbor.Pos - agent.Pos).SqrLength() < agent.NeighborhoodRadius * agent.NeighborhoodRadius;
        }

        protected override void Update()
        {
            UpdateAgents();
        }

        protected override void Render()
        {
            if (RenderQuadTree && UseQuadTree)
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
        }

        private void UpdateAgents()
        {
            if (UseQuadTree)
            {
                _qtree = new QuadTree<Agent>(new FloatRect(0, 0, Window.Size.X, Window.Size.Y))
                {
                    BoundaryCapacity = QuadTreeCapacity
                };
                _agents.ForEach(a => _qtree.Insert(a.Pos, a));
            }
            
            _agents.ForEach(a =>
            {
                _agentUpdater.Update(a);
                _agentRenderer.Render(a);
            });
        }

        public void SelectAgent(int index)
        {
            if (index < 0 && index > _agents.Count - 1)
                throw new InvalidOperationException();

            SelectedAgent = _agents[index];
        }
    }
}