using System;
using SFML.Graphics;
using SFML.Window;

namespace Stho.SFML.Extensions;

public class WindowFactory
{
    //**********************************************************
    //** fields:
    //**********************************************************

    public const int DefaultScreenWidth = 1200; 
    public const int DefaultScreenHeight = 675; 
        
    //**********************************************************
    //** methods:
    //**********************************************************
        
    public static RenderWindow CreateDefault(uint width = DefaultScreenWidth, uint height = DefaultScreenHeight)
    {
            var videoMode = new VideoMode(width, height);
            var settings = new ContextSettings
            {
                AntialiasingLevel = 8
            };
            var window = new RenderWindow(videoMode, "Game", Styles.Default, settings);

            return window;
        }
}