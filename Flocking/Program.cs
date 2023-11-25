using System;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using Stho.SFML.Extensions;
using Timer = Stho.SFML.Extensions.Timer;

namespace Flocking;

class Program
{
    public const int ScreenWidth = 1600;
    public const int ScreenHeight = 900;
    public const int DefaultNumberOfAgents = 300;


    static void Main(string[] args)
    {
        var window = new RenderWindow(new VideoMode(ScreenWidth, ScreenHeight), "Flocking");
        var game = new FlockingBehaviour(window, DefaultNumberOfAgents);
        var currentSelectedIndex = 0f;

        if (args.Contains("debug"))
            StartDebugScreen(game);

        window.KeyPressed += (source, args) =>
        {
            if (args.Code == Keyboard.Key.C)
                game.ShowCollider = !game.ShowCollider;
            if (args.Code == Keyboard.Key.F)
                game.ShowFps = !game.ShowFps;
            if (args.Code == Keyboard.Key.N)
                game.ShowNeighborhood = !game.ShowNeighborhood;
            if (args.Code == Keyboard.Key.Num1)
                game.SelectAgent(0);
            if (args.Code == Keyboard.Key.Tab)
                game.SelectAgent((int)(++currentSelectedIndex) % DefaultNumberOfAgents);
            if (args.Code == Keyboard.Key.Up)
                game.AgentSpeed = Math.Min(game.AgentSpeed + 4f, 150f);
            if (args.Code == Keyboard.Key.Down)
                game.AgentSpeed = Math.Max(game.AgentSpeed - 4f, 0f);
            if (args.Code == Keyboard.Key.R)
                game.Reset();
            if (args.Code == Keyboard.Key.Q)
                window.Close();
            if (args.Code == Keyboard.Key.T)
                game.RenderQuadTree = !game.RenderQuadTree;
            if (args.Code == Keyboard.Key.Y)
                game.UseQuadTree = !game.UseQuadTree;
            if (args.Code == Keyboard.Key.Left)
                game.QuadTreeCapacity = Math.Max(1, game.QuadTreeCapacity - 1);
            if (args.Code == Keyboard.Key.Right)
                game.QuadTreeCapacity += 1;
            if (args.Code == Keyboard.Key.S)
                game.SpawnAgent();
            if (args.Code == Keyboard.Key.D)
                game.RemoveLastAgent();
        };

        game.Initialize();
        game.Start();
    }

    public static void StartDebugScreen(FlockingBehaviour flockingBehaviour)
    {
        var debugWindow = new DebugWindow<FlockingBehaviour>(flockingBehaviour)
        {
            Width = 300,
        };

        debugWindow.Add(g => $"FPS: {Timer.Fps}");
        debugWindow.Add(g => $"(S-D) Agents {g.Agents.Count}");
        debugWindow.Add(g => $"(Y) Use QuadTree {g.UseQuadTree}");
        debugWindow.Add(g => $"(T) Render QuadTree {g.RenderQuadTree}");
        debugWindow.Add(g => $"(<>) Quad tree capacity {g.QuadTreeCapacity}");
        debugWindow.Add(g => $"(^v) Render AgentSpeed {g.AgentSpeed}");
        debugWindow.Add(g => $"(C) Render Show Collider {g.ShowCollider}");
        debugWindow.Add(g => $"(N) Render Show Neighborhood {g.ShowNeighborhood}");


        debugWindow.Show();
    }
}