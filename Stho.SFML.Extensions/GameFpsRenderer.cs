using SFML.Graphics;
using SFML.System;

namespace Stho.SFML.Extensions;

public class GameFpsRenderer
{
    private readonly Text _fpsText = new();
    private readonly RenderTarget _renderTarget;

    public GameFpsRenderer(RenderTarget renderTarget)
    {
        _renderTarget = renderTarget;
        _fpsText.Font = Fonts.Roboto;
    }

    public bool ShowFps { get; set; }

    public void Render()
    {
        if (!ShowFps) return;

        _fpsText.DisplayedString = $"FPS: {Timer.Fps.ToString()}";
        _fpsText.Position = new Vector2f(5f, 5f);
        _fpsText.CharacterSize = 18;
        _fpsText.FillColor = Color.Red;

        _renderTarget.Draw(_fpsText);
    }
}