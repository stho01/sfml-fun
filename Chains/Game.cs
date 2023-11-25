using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Chains;

public class Game : GameBase
{
    private readonly SegmentUpdater _segmentUpdater;
    private readonly SegmentRenderer _segmentRenderer;
    private Segment _head;
    private const int ChainLength = 1;

    public Game(RenderWindow window) : base(window)
    {
        _segmentUpdater = new SegmentUpdater(this);
        _segmentRenderer = new SegmentRenderer(window);
    }

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