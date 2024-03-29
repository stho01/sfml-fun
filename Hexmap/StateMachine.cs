using System;
using System.Collections.Generic;
using SFML.Graphics;
using Stho.SFML.Extensions;

namespace Hexmap;

public sealed class StateMachine(Game game)
{
    private readonly Dictionary<Type, StateProxy> _states = new();
    private IState? _current;
    private Text _name = new()
    {
        Font = Fonts.Roboto
    };

    public void AddState(IState state)
    {
        _states.Add(state.GetType(), StateProxy.From(state));
    }

    public void Load<T>()
        where T : IState
    {
        var newStateType = typeof(T);
        if (_current?.GetType() == newStateType) 
            return;
        
        _current?.Pause(game);
        _current = _states[typeof(T)];
        _name.DisplayedString = _current?.Name;
        _current?.Load(game);
        _current?.Resume(game);
    }

    public void Update()
    {
        _current?.Update(game);
    }

    public void Draw(RenderTarget target)
    {
        target.Draw(_name);
        _current?.Draw(game, target);
    }

    private sealed class StateProxy(IState state) : IState
    {
        private bool _loaded;

        public static StateProxy From(IState state) => new(state);
        
        public void Load(Game game)
        {
            if (_loaded) return;
            state.Load(game);
            _loaded = true;
        }

        public string Name => state.Name;
        public void Pause(Game game) => state.Pause(game);
        public void Resume(Game game) => state.Resume(game);
        public void Update(Game game) => state.Update(game);
        public void Draw(Game game, RenderTarget target) => state.Draw(game, target);
    }
}