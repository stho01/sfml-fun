using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace Chess;

public class Game : GameBase
{
    private readonly Board _board = new();
    private readonly BoardRenderer _boardRenderer;
    private readonly PieceRenderer _pieceRenderer;
    private readonly SpriteAtlas _spriteAtlas = new();

    public Game(RenderWindow window) : base(window)
    {
        _boardRenderer = new BoardRenderer(window, this);
        _pieceRenderer = new PieceRenderer(window, _spriteAtlas) {
            Scale = new Vector2f(1f, 1f)
        };
    }
    
    public override void Initialize()
    {
        Window.SetFramerateLimit(30);
        ClearColor = new Color(0x222222ff);
        ShowFps = true;
        
        _board.Size = new Vector2f(WindowWidth, WindowHeight) * 0.8f;
        _board.Position = WindowCenter - _board.Size / 2;

        _spriteAtlas.Load("chess2");
        
        // Type[] spriteOrder = [
        //     typeof(Rook),
        //     typeof(Bishop),
        //     typeof(Queen),
        //     typeof(King),
        //     typeof(Knight),
        //     typeof(Pawn)
        // ];
        // for (var i = 0; i < spriteOrder.Length; i++) {
        //     var pieceType = spriteOrder[i];
        //     var x = i * 300;
        //     var name = pieceType.Name;
        //     _spriteAtlas.Define($"{name}-{PieceColor.Dark}", x, 0, 300, 400);
        //     _spriteAtlas.Define($"{name}-{PieceColor.Light}", x, 400, 300, 400);
        // }
        _boardRenderer.Initialize();
        
        SetupGame();
    }

    public void SetupGame()
    {
        _board.Clear();
        _board.PlacePiece(0, 0, new Rook(PieceColor.Dark));
        _board.PlacePiece(1, 0, new Knight(PieceColor.Dark));
        _board.PlacePiece(2, 0, new Bishop(PieceColor.Dark));
        _board.PlacePiece(3, 0, new Queen(PieceColor.Dark));
        _board.PlacePiece(4, 0, new King(PieceColor.Dark));
        _board.PlacePiece(5, 0, new Bishop(PieceColor.Dark));
        _board.PlacePiece(6, 0, new Knight(PieceColor.Dark));
        _board.PlacePiece(7, 0, new Rook(PieceColor.Dark));
        
        for (var i = 0; i < 8; i++)
        {
            _board.PlacePiece(i, 1, new Pawn(PieceColor.Dark));
            _board.PlacePiece(i, 6, new Pawn(PieceColor.Light));
        }
        
        _board.PlacePiece(0, 7, new Rook(PieceColor.Light));
        _board.PlacePiece(1, 7, new Knight(PieceColor.Light));
        _board.PlacePiece(2, 7, new Bishop(PieceColor.Light));
        _board.PlacePiece(3, 7, new Queen(PieceColor.Light));
        _board.PlacePiece(4, 7, new King(PieceColor.Light));
        _board.PlacePiece(5, 7, new Bishop(PieceColor.Light));
        _board.PlacePiece(6, 7, new Knight(PieceColor.Light));
        _board.PlacePiece(7, 7, new Rook(PieceColor.Light));
    }

    protected override void Update()
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            
        }
    }

    protected override void Render()
    {
        _boardRenderer.Render(_board);
        
        foreach (var cell in _board.Cells)
        {
            if (cell.Piece is not null) {
                _pieceRenderer.Render(_board, cell.Piece);
            }
        }
    }
}