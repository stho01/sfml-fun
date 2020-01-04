using SFML.Graphics;

namespace Minesweeper
{
    public class Theme
    {
        public Color UnrevealedCellColor { get; set; } = new Color(0x848E99FF);
        public Color RevealedCellColor { get; set; } = Color.White;
        public Color CellStrokeColor { get; set; } = Color.Black;
        public int CellStrokeWidth { get; set; } = -1;
        public Color TextColor { get; set; } = Color.Black;
        public Color MineColor { get; set; } = Color.Red;
        public Color MineStrokeColor { get; set; } = Color.Black;
        public int MineStrokeWidth { get; set; } = -2;
    }
}