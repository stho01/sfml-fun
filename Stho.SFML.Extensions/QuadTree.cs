using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Stho.SFML.Extensions;

public class QuadTree<T>(FloatRect boundary)
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly FloatRect _boundary = boundary;
    private readonly List<DataHolder> _data = new();
    private QuadTree<T> _northEast;
    private QuadTree<T> _northWest;
    private QuadTree<T> _southEast;
    private QuadTree<T> _southWest;

    //**********************************************************
    //** ctor:
    //**********************************************************

    //**********************************************************
    //** props:
    //**********************************************************

    public uint BoundaryCapacity { get; set; } = 4;

    //**********************************************************
    //** methods:
    //**********************************************************

    public bool Insert(Vector2f vector, T data)
    {
        if (!_boundary.Contains(vector.X, vector.Y))
            return false;

        if (_data.Count < BoundaryCapacity && _northWest == null)
        {
            _data.Add(new DataHolder
            {
                Point = vector,
                Data = data
            });
            return true;
        }

        if (_northWest == null)
            Subdivide();

        if (_northEast.Insert(vector, data)) return true;
        if (_northWest.Insert(vector, data)) return true;
        if (_southEast.Insert(vector, data)) return true;
        if (_southWest.Insert(vector, data)) return true;

        return false;
    }

    private void Subdivide()
    {
        var width = _boundary.Width / 2;
        var height = _boundary.Height / 2;

        _northWest = new QuadTree<T>(new FloatRect(_boundary.Left, _boundary.Top, width, height))
        {
            BoundaryCapacity = BoundaryCapacity
        };

        _northEast = new QuadTree<T>(new FloatRect(_boundary.Left + width, _boundary.Top, width, height))
        {
            BoundaryCapacity = BoundaryCapacity
        };

        _southEast = new QuadTree<T>(new FloatRect(_boundary.Left + width, _boundary.Top + height, width, height))
        {
            BoundaryCapacity = BoundaryCapacity
        };

        _southWest = new QuadTree<T>(new FloatRect(_boundary.Left, _boundary.Top + height, width, height))
        {
            BoundaryCapacity = BoundaryCapacity
        };
    }

    public T[] QueryRange(FloatCircle boundary)
    {
        var rect = new FloatRect(
            boundary.X - boundary.Radius,
            boundary.Y - boundary.Radius,
            boundary.Radius,
            boundary.Radius
        );

        if (!_boundary.Intersects(rect))
            return Array.Empty<T>();

        var inRange = new List<T>();
        _data.ForEach(datum =>
        {
            if (boundary.Contains(datum.Point.X, datum.Point.Y))
                inRange.Add(datum.Data);
        });

        if (_northWest == null)
            return inRange.ToArray();

        inRange.AddRange(_northEast.QueryRange(boundary));
        inRange.AddRange(_northWest.QueryRange(boundary));
        inRange.AddRange(_southEast.QueryRange(boundary));
        inRange.AddRange(_southWest.QueryRange(boundary));

        return inRange.ToArray();
    }

    public T[] QueryRange(FloatRect boundary)
    {
        if (!_boundary.Intersects(boundary)) return Array.Empty<T>();

        var inRange = new List<T>();

        _data.ForEach(datum =>
        {
            if (boundary.Contains(datum.Point.X, datum.Point.Y))
                inRange.Add(datum.Data);
        });

        if (_northWest == null)
            return inRange.ToArray();

        inRange.AddRange(_northEast.QueryRange(boundary));
        inRange.AddRange(_northWest.QueryRange(boundary));
        inRange.AddRange(_southEast.QueryRange(boundary));
        inRange.AddRange(_southWest.QueryRange(boundary));

        return inRange.ToArray();
    }

    public FloatRect[] GetBoundaries()
    {
        var boundaries = new List<FloatRect>();
        boundaries.Add(_boundary);

        if (_northEast == null)
            return boundaries.ToArray();

        boundaries.AddRange(_northEast.GetBoundaries());
        boundaries.AddRange(_northWest.GetBoundaries());
        boundaries.AddRange(_southEast.GetBoundaries());
        boundaries.AddRange(_southWest.GetBoundaries());

        return boundaries.ToArray();
    }

    //**********************************************************
    //** inner classes:
    //**********************************************************

    private class DataHolder
    {
        public Vector2f Point { get; set; }
        public T Data { get; set; }
    }
}