using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Stho.SFML.Extensions
{
    public class DebugWindow<TGame> where TGame : GameBase
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private RenderWindow _window;
        private readonly List<Func<TGame, object>> _items = new List<Func<TGame, object>>();
        private Text _text;
        private bool _running;
        private readonly TGame _game;
        private Action<RenderTarget> DrawCallback;
        private bool _shouldRender = false;

        //**********************************************************
        //** ctor:
        //**********************************************************

        public DebugWindow(TGame game)
        {
            _game = game;
            Font = Fonts.GetRoboto();
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public uint Width { get; set; } = 400;
        public Vector2f WindowPadding { get; set; } = new Vector2f(5f, 5f);
        public Vector2f ItemPadding { get; set; } = new Vector2f(0f, 5f);
        public Font Font { get; set; } 
        public uint FontSize { get; set; } = 12;
          
        //**********************************************************
        //** methods:
        //**********************************************************

        public void Add(Func<TGame, object> line)
        {
            _items.Add(line);
        }
        
        public void OnDraw(Action<RenderTarget> drawCallback) => DrawCallback = drawCallback;
        
        public void Show()
        {
            ThreadPool.QueueUserWorkItem((s) => Start());
        }

        private void Start()
        {
            if(_running) return;

            Init();
            Render();

            _running = true; 
        }

        private void Init()
        {
            _window = new RenderWindow(new VideoMode(Width, _game.Window.Size.Y), "Debug");

            _game.Window.Position = new Vector2i(
                _game.Window.Position.X - ((int)_window.Size.X) / 2,
                _game.Window.Position.Y
            );
            
            _window.Position = new Vector2i(
                _game.Window.Position.X +  (int)_game.Window.Size.X,
                _game.Window.Position.Y);
            
            _window.Closed += (source, args) => _window.Close();
            _text = new Text {
                Font = Font,
                CharacterSize = FontSize
            };
            
            _game.OnUpdated += (source, args) => _shouldRender = true;
        }
        
        private void Render()
        {
            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                
                if (!_shouldRender) continue;
                
                _window.Clear();

                var lines = GetLines();
                for (var i = 0; i < lines.Length; i++)
                {
                    _text.Position = new Vector2f(0f, i * (FontSize + 4));
                    _text.DisplayedString = lines[i];
                    
                    _window.Draw(_text);
                    DrawCallback?.Invoke(_window);
                }
                _window.Display();
                _shouldRender = false;
            }
        }

        private string[] GetLines()
        {
            var lines = new List<string>();

            foreach (var item in _items)
            {
                var value = item(_game);
                if (value is string str)
                {
                    lines.Add(str);    
                } 
                else if (value is IEnumerable enumerable)
                {
                    var enumerator = enumerable.GetEnumerator();
                    while (enumerator.MoveNext())
                        lines.Add(enumerator.Current?.ToString());
                }
                else
                {
                    lines.Add(value.ToString());
                }
            }

            return lines.ToArray();
        }
    }
}