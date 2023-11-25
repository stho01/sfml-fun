using System;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SFML.Window;

namespace Stho.SFML.Extensions;

public abstract class GameBase
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly RenderWindow _window;
    private readonly GameFpsRenderer _gameFpsRenderer;
            
    //**********************************************************
    //** events
    //**********************************************************

    public event EventHandler<EventArgs> OnUpdated; 
    public event EventHandler<EventArgs> OnRendered; 
        
    //**********************************************************
    //** ctor:
    //**********************************************************
        
    protected GameBase(RenderWindow window)
    {
            
            _window = window;
            _window.Closed += OnWindowClosed;
            _gameFpsRenderer = new GameFpsRenderer(window);
        }
          
    //**********************************************************
    //** props:
    //**********************************************************

    public RenderWindow Window => _window;
        
    public bool ShowFps
    {
        get => _gameFpsRenderer.ShowFps;
        set => _gameFpsRenderer.ShowFps = value;
    }

    public uint WindowWidth => Window.Size.X;
    public uint WindowHeight => Window.Size.Y;
    public Vector2f WindowCenter => new Vector2f(WindowWidth/2, WindowHeight/2);
    public Color ClearColor { get; set; } = Color.Black;
    public FloatRect WindowBounds => new FloatRect(0, 0, WindowWidth, WindowHeight);
          
    //**********************************************************
    //** abstract methods:
    //**********************************************************

    public abstract void Initialize();
    protected abstract void Update();
    protected abstract void Render();
          
    //**********************************************************
    //** methods:
    //**********************************************************

    public void Start()
    {
            while (_window.IsOpen)
            {
                Timer.Update();
                _window.DispatchEvents();
                _window.Clear(ClearColor);
                Update();
                OnUpdated?.Invoke(this, EventArgs.Empty);
                Render();
                OnRendered?.Invoke(this, EventArgs.Empty);
                _gameFpsRenderer.Render();
                _window.Display();
            }
        }
        
    public void Stop()
    {
            _window.Close();
        }

    public Vector2i GetMousePosition() => Mouse.GetPosition(Window);
        
    //**********************************************************
    //** event handlers:
    //**********************************************************

    protected virtual void OnWindowClosed(object source, EventArgs args) 
    {
            _window.Close();
        }
}