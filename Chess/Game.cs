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
        Board.Setup();
    }

    private bool _isCheck;
    
    protected override void Update()
    {
        if (_isCheck) {
            Console.WriteLine("Check");
            
            var king = CurrentPlayer == PieceColor.White 
                ? Board.KingWhite 
                : Board.KingBlack;
            
            MoveController.SelectCell(king.Position);
        }
        
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
            else if (MoveController.IsSelectedCell(position) && !_isCheck)
            {
                MoveController.DeselectCell();
            }
            else {
                if (MoveController.MoveSelectedPiece(position))
                {
                    CurrentPlayer = CurrentPlayer == PieceColor.White 
                        ? PieceColor.Black 
                        : PieceColor.White;

                    _isCheck = MoveController.CheckForCheck();
                }
            }
                
            _leftMouseClickedLastFrame = true;
        }
        else
        {
            _leftMouseClickedLastFrame = false;
        }
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            Board.Setup();
        
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