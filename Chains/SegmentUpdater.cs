using SFML.System;
using Stho.SFML.Extensions;

namespace Chains;

public class SegmentUpdater(Game game)
{
    private readonly Game _game = game;

    public static void SetPosition(Segment segment, Vector2f pos)
    {
        segment.P1 = pos;

        var d = segment.P2 - segment.P1;
        var mag = segment.Length - d.Length();
        var change = d.Normalize() * mag;

        segment.P2 += change;

        if (segment.Child != null)
            SetPosition(segment.Child, segment.P2);
    }
}