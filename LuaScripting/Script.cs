using System;
using LuaScripting.Models;
using NLua;

namespace LuaScripting
{
    public sealed class Script : IDisposable
    {
        private readonly GameObject _gameObject;
        private readonly string _file;
        private readonly Lua _lua = new Lua();
        private LuaFunction? _onUpdate;
    
        public Script(GameObject gameObject, string file)
        {
            _gameObject = gameObject;
            _file = file;
        }
    
        public void Load()
        {
            _lua.LoadCLRPackage();

            const string imports = @"
                import ('LuaScripting', 'LuaScripting.Models')
                import ('SFML.System', 'SFML.System')
                import ('SFML.Window', 'SFML.Window')
            ";

            _lua.DoString(imports);
            
            _lua["gameObject"] = _gameObject;
        
            _lua.DoFile(_file);
        
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
}