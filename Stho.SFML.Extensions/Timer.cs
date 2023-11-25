using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Stho.SFML.Extensions;

public static class Timer
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private static long? _previous;

    private static readonly List<Interval> _intervals = new();

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
        var now = Stopwatch.GetTimestamp();

        DeltaTime = now - _previous.GetValueOrDefault(now);

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
        private readonly Action _callback;
        private float _elapsed;
        private int _ms;

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