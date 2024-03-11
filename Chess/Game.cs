using System.Security.Cryptography.X509Certificates;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace Chess;

public class Game : GameBase
{
    private readonly BoardRenderer _boardRenderer;
    private readonly PieceRenderer _pieceRenderer;
    private readonly MoveRenderer _moveRenderer;
    private readonly NamePlateRenderer _namePlateRenderer;
    private readonly SpriteAtlas _spriteAtlas = new();
    private bool _leftMouseClickedLastFrame;
    
    public Game(RenderWindow window) : base(window)
    {
        _boardRenderer = new BoardRenderer(window, this);
        _pieceRenderer = new PieceRenderer(window, _spriteAtlas) {
            Scale = new Vector2f(1f, 1f)
        };
        MoveController = new(this);
        _moveRenderer = new MoveRenderer(window, this);
        _namePlateRenderer = new(window, this);
    }

    public MoveController MoveController { get; }
    public Board Board { get; } = new();
    public PieceColor CurrentPlayer { get; private set; } = PieceColor.White;

    public override void Initialize()
    {
        Window.SetFramerateLimit(30);
        ClearColor = new Color(0x21100EFF);
        _spriteAtlas.Load("chess2");
        
        _pieceRenderer.Initialize();
        _boardRenderer.Initialize();
        
        Board.Size = new Vector2f(WindowWidth, WindowHeight) * 0.8f;
        Board.Position = WindowCenter - Board.Size / 2;
        
        SetupGame();
    }

    private void SetupGame()
    {
        Board.Clear();
        Board.PlacePiece(0, 0, new Piece(PieceType.Rook, PieceColor.Black));
        Board.PlacePiece(1, 0, new Piece(PieceType.Knight, PieceColor.Black));
        Board.PlacePiece(2, 0, new Piece(PieceType.Bishop, PieceColor.Black));
        Board.PlacePiece(3, 0, new Piece(PieceType.Queen, PieceColor.Black));
        Board.PlacePiece(4, 0, new Piece(PieceType.King, PieceColor.Black));
        Board.PlacePiece(5, 0, new Piece(PieceType.Bishop, PieceColor.Black));
        Board.PlacePiece(6, 0, new Piece(PieceType.Knight, PieceColor.Black));
        Board.PlacePiece(7, 0, new Piece(PieceType.Rook, PieceColor.Black));
        
        for (var i = 0; i < Board.MaxTileCount; i++)
        {
            Board.PlacePiece(i, 1, new Piece(PieceType.Pawn, PieceColor.Black));
            Board.PlacePiece(i, 6, new Piece(PieceType.Pawn, PieceColor.White));
        } 
        
        Board.PlacePiece(0, 7, new Piece(PieceType.Rook, PieceColor.White));
        Board.PlacePiece(1, 7, new Piece(PieceType.Knight, PieceColor.White));
        Board.PlacePiece(2, 7, new Piece(PieceType.Bishop, PieceColor.White));
        Board.PlacePiece(3, 7, new Piece(PieceType.Queen, PieceColor.White));
        Board.PlacePiece(4, 7, new Piece(PieceType.King, PieceColor.White));
        Board.PlacePiece(5, 7, new Piece(PieceType.Bishop, PieceColor.White));
        Board.PlacePiece(6, 7, new Piece(PieceType.Knight, PieceColor.White));
        Board.PlacePiece(7, 7, new Piece(PieceType.Rook, PieceColor.White));
    }

    protected override void Update()
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            if (_leftMouseClickedLastFrame) {
                return;
            }
            
            var position = Board.PositionFromScreenCoords(GetMousePosition());
            if (MoveController.SelectedCell is null)
            {
                MoveController.SelectCell(position);
                if (MoveController.SelectedCell?.Piece?.Color != CurrentPlayer)
                    MoveController.DeselectCell();
            }
            else if (MoveController.IsSelectedCell(position))
            {
                MoveController.DeselectCell();
            }
            else
            {
                if (MoveController.MoveSelectedPiece(position))
                {
                    CurrentPlayer = CurrentPlayer == PieceColor.White 
                        ? PieceColor.Black 
                        : PieceColor.White;    
                }
            }
                
            _leftMouseClickedLastFrame = true;
        }
        else
        {
            _leftMouseClickedLastFrame = false;
        }
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            SetupGame();
        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            MoveController.DeselectCell();
    }

    protected override void Render()
    {
        _boardRenderer.Render(Board);
        _moveRenderer.Render(MoveController);
        
        foreach (var cell in Board.Cells)
        {
            if (cell.Piece is not null) {
                _pieceRenderer.Render(Board, cell.Piece);
            }
        }
        
        _namePlateRenderer.Render();
    }
}