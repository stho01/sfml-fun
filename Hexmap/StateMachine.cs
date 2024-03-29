using System;
using System.Collections.Generic;
using Hexmap.States;
using SFML.Graphics;

namespace Hexmap;

public sealed class StateMachine(Game game)
{
    private readonly Dictionary<Type, IState> _states = new();
    private IState? _current;

    public void AddState(IState state)
    {
        _states.Add(state.GetType(), state);
    }

    public void Load<T>()
        where T : IState
    {
        var newStateType = typeof(T);
        if (_current?.GetType() == newStateType) 
            return;
        
        _current?.Suspend(game);
        _current = _states[typeof(T)];
        _current?.Load(game);
    }

    public void Update()
    {
        _current?.Update(game);
    }

    public void Draw(RenderTarget target)
    {
        _current?.Draw(game, target);
    }
}