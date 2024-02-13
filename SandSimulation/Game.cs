using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;
using Timer = Stho.SFML.Extensions.Timer;

namespace SandSimulation;

public class Game(RenderWindow window, int tileSize) : GameBase(window)
{
    private readonly List<Vector2f> _grains = [];
    private RectangleShape _shape;
    private Vector2f?[,] _grid;
    
    public int TilesX => (int)(WindowWidth/(float)tileSize);
    public int TilesY => (int)(WindowHeight/(float)tileSize);
    
    public override void Initialize()
    {
        ClearColor = Color.Black;
        
        _grid = new Vector2f?[TilesX, TilesY];
        _shape = new RectangleShape {
            Size = new Vector2f(tileSize, tileSize),
            FillColor = new Color(239, 228, 176),
        };
    }

    protected override void Update()
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            var brushSize = 5;
            var tilePos = ToTilePosition((Vector2f)Mouse.GetPosition(Window));
            
            for (var brushX = 0; brushX < brushSize; brushX++)
            for (var brushY = 0; brushY < brushSize; brushY++)
            {
                var x = tilePos.X + brushX;
                var y = tilePos.Y + brushY;
                
                if (GetGrain(x, y) is null)
                {
                    var pos = (Vector2f)ToScreenPosition(x, y);
                    _grid[tilePos.X, tilePos.Y] = pos;
                    _grains.Add(pos);
                }    
            }
        }
        
        for (var i = 0; i < _grains.Count; i++)
        {
            var oldPosition = _grains[i];
            var oldTilePosition = ToTilePosition(oldPosition);
            
            var newPosition = _grains[i] + new Vector2f(0, 750) * (float)Timer.DeltaTimeSeconds;
            var newTilePosition = ToTilePosition(newPosition);
            
            var grainAtNewPosition = GetGrain(newTilePosition);
            var isColliding = grainAtNewPosition != null && grainAtNewPosition != _grains[i];

            if (!IsIndexOutOfBounds(newTilePosition))
            {
                if (!isColliding)
                {
                    _grains[i] = newPosition;
                    _grid[oldTilePosition.X, oldTilePosition.Y] = null;
                    _grid[newTilePosition.X, newTilePosition.Y] = _grains[i];    
                } 
                else
                {
                    var negX = newTilePosition.X - 1;
                    var negY = newTilePosition.Y;
                    var negCount = 0;
                    while (GetGrain(negX, negY) == null && IsIndexOutOfBounds(negX, negY) == false)
                    {
                        negCount++;
                        negY++;
                    }
                    
                    var posX = newTilePosition.X + 1;
                    var posY = newTilePosition.Y;
                    var posCount = 0;
                    while (GetGrain(posX, posY) == null && IsIndexOutOfBounds(posX, posY) == false)
                    {
                        posCount++;
                        posY++;
                    }

                    if (posCount >= negCount && posCount > 0)
                    {
                        _grains[i] = (Vector2f)ToScreenPosition(new Vector2i(posX, newTilePosition.Y )); 
                        _grid[oldTilePosition.X, oldTilePosition.Y] = null;
                        _grid[posX, newTilePosition.Y] = _grains[i];
                    }
                    else if (negCount > 0)
                    {
                        _grains[i] = (Vector2f)ToScreenPosition(new Vector2i(negX, newTilePosition.Y)); 
                        _grid[oldTilePosition.X, oldTilePosition.Y] = null;
                        _grid[negX, newTilePosition.Y] = _grains[i];
                    }
                }    
            }
        }
    }

    protected override void Render()
    {
        foreach (var grain in _grains)
        {
            var pos = ToScreenPosition(ToTilePosition(grain));
            _shape.Position = (Vector2f)pos;
            Window.Draw(_shape);
        }
    }

    public Vector2i ToTilePosition(Vector2f position) => (Vector2i)(position / tileSize);
    public Vector2i ToScreenPosition(Vector2i tilePosition) => tilePosition * tileSize;
    public Vector2i ToScreenPosition(int x, int y) => new Vector2i(x,y) * tileSize;


    public Vector2f? GetGrain(Vector2i vec) => GetGrain(vec.X, vec.Y);
    public Vector2f? GetGrain(int x, int y)
    {
        return IsIndexOutOfBounds(x, y) ? null : _grid[x, y];
    }

    public bool IsIndexOutOfBounds(Vector2i vec) => IsIndexOutOfBounds(vec.X, vec.Y);
    public bool IsIndexOutOfBounds(int x, int y)
    {
        return 
            !(x >= 0
            && x < _grid.GetLength(0) -1
            && y >= 0
            && y < _grid.GetLength(1) -1);
    }
}