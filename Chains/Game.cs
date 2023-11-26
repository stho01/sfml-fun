using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Chains;

public class Game(RenderWindow window) : GameBase(window)
{
    private readonly SegmentRenderer _segmentRenderer = new(window);
    private Segment _head;
    private const int ChainLength = 1;

    public override void Initialize()
    {
        _head = new Segment(ChainLength);

        var current = _head;
        for (var i = 0; i < 1000; i++)
            current = current.Child = new Segment(ChainLength);
    }

    protected override void Update()
    {
        SegmentUpdater.SetPosition(_head, (Vector2f)GetMousePosition());
    }

    protected override void Render()
    {
        _segmentRenderer.Render(_head);
    }
}