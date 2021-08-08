﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        private static readonly List<Interval> _intervals = new List<Interval>();
          
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
            
            _intervals.ForEach(x =>
            {
                if (!x.Pause) x.UpdateInterval();
            });
        }

        public static Interval SetInterval(int ms, Action callback)
        {
            var interval = new Interval(ms, callback); 
            _intervals.Add(interval);
            return interval;
        }

        public static void ClearInterval(Interval interval)
        {
            _intervals.Remove(interval);
        }

        public class Interval
        {
            private int _ms;
            private readonly Action _callback;
            private float _elapsed = 0;

            internal Interval(int ms, Action callback)
            {
                _ms = ms;
                _callback = callback;
            }

            public int Ms
            {
                get => _ms;
                set => _ms = Math.Max(0, value);
            }

            public bool Pause { get; set; }

            public void UpdateInterval()
            {
                _elapsed += DeltaTimeMilliseconds;
                if (_elapsed < _ms) 
                    return;
                
                _callback.Invoke();
                _elapsed = 0f;
            }
        }
    }
}