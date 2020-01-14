using System.Collections.Generic;

namespace Maze
{
    public class Maze
    {
        //**********************************************************
        //** ctor:
        //**********************************************************

        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Cell[Width,Height];
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public int Width { get; }
        public int Height { get; }
        public Cell[,] Cells { get; } 
    }
}