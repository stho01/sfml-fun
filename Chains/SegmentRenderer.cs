using System.Collections.Generic;
using SFML.Graphics;

namespace Chains;

public class SegmentRenderer
{
    private readonly RenderTarget _renderTarget;

    private readonly Color[] _colors = new[]
        {
            Color.Red,
            Color.Magenta,
            Color.Blue,
            Color.Yellow,
            Color.Green,
        };

    private int _currentColorIndex = 0;

    public SegmentRenderer(RenderTarget renderTarget)
    {
            _renderTarget = renderTarget;
        }
        
    public void Render(Segment segment)
    {
            var color = _colors[_currentColorIndex];
            
            _renderTarget.Draw(new[]
            {
                new Vertex(segment.P1, color),
                new Vertex(segment.P2, color)
            },0, 2, PrimitiveType.Lines);
            
            // _currentColorIndex = (_currentColorIndex + 1) % _colors.Length;
            if (segment.Child != null)
                Render(segment.Child);
        }
}