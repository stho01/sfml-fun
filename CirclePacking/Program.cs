﻿using CirclePacking;
using Stho.SFML.Extensions;

var win = WindowFactory.CreateDefault();
win.SetTitle("Circle Packing");
var game = new Game(win);
            
var debug = new DebugWindow<Game>(game);
debug.Add(g => $"Circle count {g.CircleCount} of max {Game.MaxNumberOfElements}");
debug.Show();
            
game.Initialize();
game.Start();