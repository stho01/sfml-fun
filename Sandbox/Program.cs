using SFML.Graphics;
using Stho.SFML.Extensions;

namespace Sandbox;

class Sandbox(RenderWindow window) : GameBase(window)
{
    public override void Initialize() { }

    protected override void Update() { }

    protected override void Render() { }
}

class Program
{
    static void Main(string[] args)
    {
        var sandbox = new Sandbox(WindowFactory.CreateDefault());
        sandbox.Initialize();
        sandbox.Start();
    }
}