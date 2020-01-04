namespace Minesweeper
{
    public class Cell
    {
        private int _neighborMines = 0;
        
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
          
        //**********************************************************
        //** method:
        //**********************************************************

        public int X { get; }
        public int Y { get; }
        public bool IsMine { get; set; }
        public bool Revelead { get; set; }

        public int NeighborMines
        {
            get => IsMine ? -1 : _neighborMines;
            set => _neighborMines = value;
        }
        
    }
}