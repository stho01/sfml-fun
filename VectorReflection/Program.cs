using System;
using Stho.SFML.Extensions;

namespace VectorReflection;

class Program
{
    static void Main(string[] args)
    {
        var sandbox = new Game(WindowFactory.CreateDefault());
        sandbox.Initialize();
        sandbox.Start();
    }
}