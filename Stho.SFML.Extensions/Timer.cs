using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Stho.SFML.Extensions;

public static class Timer
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private static long _previous = -1;

    private static readonly List<Interval> Intervals = [];

    //**********************************************************
    //** props:
    //**********************************************************

    public static TimeSpan DeltaTime { get; private set; }
    public static double DeltaTimeSeconds => DeltaTime.TotalSeconds; // / 10_000_000;
    public static double DeltaTimeMilliseconds => DeltaTime.TotalMilliseconds; // DeltaTime / 10_000;
    public static int Fps { get; private set; }

    //**********************************************************
    //** methods:
    //**********************************************************

    public static void Update()
    {
        var now = Stopwatch.GetTimestamp();

        DeltaTime = _previous == -1 
            ? TimeSpan.Zero 
            : Stopwatch.GetElapsedTime(_previous);

        _previous = now;

        Fps = (int)(1f / DeltaTimeSeconds);


        foreach (var interval in Intervals)
        {
            if (!interval.Pause) 
                interval.UpdateInterval();
        }
    }

    public static Interval SetInterval(int ms, Action callback)
    {
        var interval = new Interval(ms, callback);
        Intervals.Add(interval);
        return interval;
    }

    public static void ClearInterval(Interval interval)
    {
        Intervals.Remove(interval);
    }

    public sealed class Interval
    {
        private readonly Action _callback;
        private TimeSpan _elapsed;
        private TimeSpan _time;

        internal Interval(int ms, Action callback)
        {
            _elapsed = TimeSpan.Zero;
            _time = TimeSpan.FromMilliseconds(ms);
            _callback = callback;
        }

        public bool Pause { get; set; }

        public void UpdateInterval()
        {
            _elapsed += DeltaTime;
            if (_elapsed < _time)
                return;
            
            _callback.Invoke();
            _elapsed -= _time;
        }
    }
}