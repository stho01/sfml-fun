using System;
using LuaScripting.Models;
using NLua;

namespace LuaScripting;

public sealed class Script(GameObject gameObject, string file) : IDisposable
{
    private readonly Lua _lua = new();
    private LuaFunction? _onUpdate;

    public void Load()
    {
        _lua.LoadCLRPackage();

        const string imports =
            """
            import ('LuaScripting', 'LuaScripting.Models')
            import ('SFML.System', 'SFML.System')
            import ('SFML.Window', 'SFML.Window')
            """;

        _lua.DoString(imports);

        _lua["gameObject"] = gameObject;

        _lua.DoFile(file);

        var load = _lua.GetFunction("OnLoad");
        load?.Call();

        _onUpdate = _lua.GetFunction("OnUpdate");
    }

    public void Update(float dt)
    {
        _onUpdate?.Call(dt);
    }

    public void Dispose()
    {
        _onUpdate = null;
        _lua.Dispose();
    }
}