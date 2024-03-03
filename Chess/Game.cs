using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Chess;

public class Game : GameBase
{
    private readonly Board _board = new();
    private readonly BoardRenderer _boardRenderer;
    private readonly PieceRenderer _pieceRenderer;
    private readonly SpriteAtlas _spriteAtlas = new("Assets/chess.png");

    private Knight _piece1 = new(PieceColor.Dark);
    private Pawn _piece2 = new(PieceColor.Light);
    private Bishop _piece3 = new(PieceColor.Dark);

    public Game(RenderWindow window) : base(window)
    {
        _boardRenderer = new BoardRenderer(window, this);
        _pieceRenderer = new PieceRenderer(window, _spriteAtlas) {
            Scale = new Vector2f(0.2f, 0.2f)
        };
    }
    
    
    public override void Initialize()
    {
        ClearColor = new Color(0x222222ff);
        Window.SetFramerateLimit(30);
        ShowFps = true;

        _spriteAtlas.Load();
        
        Type[] spriteOrder = [
            typeof(Rook),
            typeof(Bishop),
            typeof(Queen),
            typeof(King),
            typeof(Knight),
            typeof(Pawn)
        ];
        
        for (var i = 0; i < spriteOrder.Length; i++) {
            var pieceType = spriteOrder[i];
            var x = i * 300;
            var name = pieceType.Name;
            _spriteAtlas.Define($"{name}-{PieceColor.Dark}", x, 0, 300, 400);
            _spriteAtlas.Define($"{name}-{PieceColor.Light}", x, 400, 300, 400);
        }
        
        _boardRenderer.Initialize();
    }

    protected override void Update()
    {
        
    }

    protected override void Render()
    {
        var boardSize = new Vector2f(WindowWidth, WindowHeight) * 0.75f;
        var center = WindowCenter - boardSize / 2; 
        
        _boardRenderer.Render(
            _board, 
            center,
            boardSize);
        
        _pieceRenderer.Render(_piece1, new Vector2f(100, 100));
        _pieceRenderer.Render(_piece2, new Vector2f(300, 300));
        _pieceRenderer.Render(_piece3, new Vector2f(500, 500));
        
        // Window.Draw(_spriteAtlas.GetSprite(false));
    }
}