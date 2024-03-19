using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Stho.SFML.Extensions;

public abstract class GameBase
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly GameFpsRenderer _gameFpsRenderer;

    //**********************************************************
    //** ctor:
    //**********************************************************

    protected GameBase(RenderWindow window)
    {
        Window = window;
        Window.Closed += OnWindowClosed;
        _gameFpsRenderer = new GameFpsRenderer(window);
        window.Resized += OnWindowResized;
        WindowWidth = window.Size.X;
        WindowHeight = window.Size.Y;
        WindowCenter = new Vector2f(WindowWidth / 2f, WindowHeight / 2f);
    }

    //**********************************************************
    //** props:
    //**********************************************************

    public RenderWindow Window { get; }

    public bool ShowFps
    {
        get => _gameFpsRenderer.ShowFps;
        set => _gameFpsRenderer.ShowFps = value;
    }

    public uint WindowWidth { get; private set; }
    public uint WindowHeight { get; private set; }
    public Vector2f WindowCenter { get; private set; }
    public Color ClearColor { get; set; } = Color.Black;
    public FloatRect WindowBounds => new(0, 0, WindowWidth, WindowHeight);

    //**********************************************************
    //** events
    //**********************************************************

    public event EventHandler<EventArgs> OnUpdated;
    public event EventHandler<EventArgs> OnRendered;

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
        GC.Collect();  
        
        while (Window.IsOpen)
        {
            Timer.Update();
            Window.DispatchEvents();
            Window.Clear(ClearColor);
            Update();
            OnUpdated?.Invoke(this, EventArgs.Empty);
            Render();
            OnRendered?.Invoke(this, EventArgs.Empty);
            _gameFpsRenderer.Render();
            Window.Display();
        }
    }

    public void Stop()
    {
        Window.Close();
    }

    public Vector2i GetMousePosition()
    {
        return Mouse.GetPosition(Window);
    }

    //**********************************************************
    //** event handlers:
    //**********************************************************

    protected virtual void OnWindowClosed(object source, EventArgs args)
    {
        Window.Close();
    }

    protected virtual void OnWindowResized(object source, SizeEventArgs args)
    {
        WindowWidth = args.Width;
        WindowHeight = args.Height;
        WindowCenter = new Vector2f(WindowWidth / 2f, WindowHeight / 2f);
        Window.SetView(new View(WindowCenter, new Vector2f(WindowWidth, WindowHeight)));
    }
}