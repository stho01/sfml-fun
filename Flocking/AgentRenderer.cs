using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Flocking
{
    public struct AgentGeometry
    {
        public float Radius { get; set; }
        public Vector2f Origin { get; set; }
    }
    
    public class AgentRenderer
    {
        private readonly FlockingBehaviour _flockingBehaviour;
        private readonly RenderTarget _renderTarget;
        private static readonly CircleShape Ship = new CircleShape(24f, 3);
        private static readonly CircleShape Collider = new CircleShape(24f);
        private static readonly CircleShape Neighborhood = new CircleShape(24f);
        
        public AgentRenderer(FlockingBehaviour flockingBehaviour, RenderTarget renderTarget)
        {
            _flockingBehaviour = flockingBehaviour;
            _renderTarget = renderTarget;
        }
        
        public void Render(Agent agent)
        {
            var geometry = GetGeometry(agent);

            if (_flockingBehaviour.ShowCollider)
                DrawCollider(agent, geometry);

            if (_flockingBehaviour.ShowNeighborhood && agent == _flockingBehaviour.SelectedAgent)
                DrawNeighborhoodAreas(agent, geometry);
            
            DrawAgent(agent, geometry);
        }

        private AgentGeometry GetGeometry(Agent agent)
        {
            var radius = agent.Size * .5f;
            return new AgentGeometry
            {
                Radius = radius,
                Origin = new Vector2f(radius, radius)
            };   
        }

        private void DrawAgent(Agent agent, AgentGeometry geometry)
        {
            Ship.Radius = geometry.Radius;
            Ship.Position = agent.Pos;
            Ship.Scale = new Vector2f(.75f, 1f);
            Ship.Rotation = agent.Velocity.ToPolarCoordinates().Angle;
            Ship.Origin = geometry.Origin;
            Ship.FillColor = agent == _flockingBehaviour.SelectedAgent ? Color.Green : Color.White;
            
            _renderTarget.Draw(Ship);
        }

        public void DrawCollider(Agent agent, AgentGeometry geometry)
        {
            Collider.Radius = geometry.Radius;
            Collider.OutlineColor = Color.Yellow;
            Collider.OutlineThickness = 1f;
            Collider.FillColor = Color.Transparent;
            Collider.Position = agent.Pos;
            Collider.Origin = geometry.Origin;
            
            _renderTarget.Draw(Collider);
        }

        public void DrawNeighborhoodAreas(Agent agent, AgentGeometry geometry)
        {
            Neighborhood.Radius = agent.NeighborhoodRadius;
            Neighborhood.OutlineColor = Color.Red;
            Neighborhood.OutlineThickness = 1f;
            Neighborhood.FillColor = new Color(0xFFAAAA55);
            Neighborhood.Position = agent.Pos;
            Neighborhood.Origin = new Vector2f(agent.NeighborhoodRadius, agent.NeighborhoodRadius);
            
            _renderTarget.Draw(Neighborhood);

            var separationArea = agent.NeighborhoodRadius * .5f;
            Neighborhood.Radius = separationArea;
            Neighborhood.Origin = new Vector2f(separationArea, separationArea);
            _renderTarget.Draw(Neighborhood);            
        }
    }
}