using System;
using System.Collections.Generic;

namespace Stho.SFML.Extensions
{
    public static class Timer
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private static long? _previous;
          
        //**********************************************************
        //** props:
        //**********************************************************

        public static float DeltaTime { get; private set; }
        public static float DeltaTimeSeconds => DeltaTime / 10_000_000;
        public static float DeltaTimeMilliseconds => DeltaTime / 10_000;
        public static int Fps { get; private set; }
          
        //**********************************************************
        //** methods:
        //**********************************************************

        public static void Update()
        {
            _previous ??= DateTime.UtcNow.Ticks;
            var now = DateTime.UtcNow.Ticks;
            var dt = (now - _previous.Value); 
            DeltaTime = dt;
            _previous = now;
            
            Fps = (int)(1f / DeltaTimeSeconds);
        }

    }
}