using SFML.System;
using Stho.SFML.Extensions;

namespace Flocking;

public class AgentUpdater(FlockingBehaviour flockingBehaviour)
{
    //**********************************************************
    //** fields:
    //**********************************************************

    public float AlignmentAmount = 1f;
    public float SeparationAmount = 1.5f;
    public float CohesionAmount = 1f;
    public float MaxSteeringForce = .95f;

    //**********************************************************
    //** props:
    //**********************************************************

    public float MaxSpeed { get; set; } = 150f;

    //**********************************************************
    //** methods:
    //**********************************************************

    public void Update(Agent agent)
    {
        agent.Acceleration = Flock(agent);
        agent.Velocity = LimitMagnitude(agent.Acceleration + agent.Velocity, MaxSpeed);
        agent.Pos += agent.Velocity * (float)Timer.DeltaTimeSeconds;

        Wraparound(agent);
    }

    public Vector2f Flock(Agent agent)
    {
        var neighbors = flockingBehaviour.GetNeighbors(agent);

        if (neighbors.Length == 0)
            return agent.Acceleration;

        var alignment = new Vector2f(0f, 0f);
        var cohesion = new Vector2f(0f, 0f);
        var separation = new Vector2f(0f, 0f);
        var separationCount = 0;

        foreach (var neighbor in neighbors)
        {
            var distanceFromNeighbor = agent.Pos - neighbor.Pos;

            alignment += neighbor.Velocity;
            cohesion += neighbor.Pos;

            if ((agent.Pos - neighbor.Pos).SqrLength() < (agent.NeighborhoodRadius * agent.NeighborhoodRadius) * .5)
            {
                separation += distanceFromNeighbor.Normalize() / distanceFromNeighbor.Length();
                separationCount++;
            }
        }

        alignment = Steer(agent, (alignment / neighbors.Length).Normalize() * MaxSpeed);
        cohesion = Steer(agent, ((cohesion / neighbors.Length) - agent.Pos).Normalize() * MaxSpeed);
        separation = separationCount == 0 ? separation : Steer(agent, (separation / separationCount).Normalize() * MaxSpeed);

        return alignment * AlignmentAmount
               + cohesion * CohesionAmount
               + separation * SeparationAmount;
    }


    public void Wraparound(Agent agent)
    {
        var x = agent.Pos.X;
        var y = agent.Pos.Y;

        if ((agent.Pos.X + agent.Size) < 0) x = Program.ScreenWidth + agent.Size - 1;
        if ((agent.Pos.X - agent.Size) > Program.ScreenWidth) x = -agent.Size + 1;
        if ((agent.Pos.Y + agent.Size) < 0) y = Program.ScreenHeight + agent.Size - 1;
        if ((agent.Pos.Y - agent.Size) > Program.ScreenHeight) y = -agent.Size + 1;

        agent.Pos = new Vector2f(x, y);
    }

    private Vector2f Steer(Agent agent, Vector2f desired)
    {
        var steer = desired - agent.Velocity;

        steer = LimitMagnitude(steer, MaxSteeringForce);

        return steer;
    }

    private Vector2f LimitMagnitude(Vector2f baseVector, float maxMagnitude)
    {
        if (baseVector.SqrLength() > maxMagnitude * maxMagnitude)
        {
            baseVector = baseVector.Normalize() * maxMagnitude;
        }

        return baseVector;
    }
}