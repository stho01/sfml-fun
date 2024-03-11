using SFML.System;

namespace Chess;

public class MoveController(Game game)
{
    private Cell? _selectedCell; 
    
    public Cell? SelectedCell
    {
        get => _selectedCell;
        
        private set
        {
            _selectedCell = value;
            SelectedCellEligibleMoves = 
                value is { Piece: not null } 
                    ? GetEligibleMoves(value.Piece) 
                    : null;
        } 
    }
    
    public Cell[]? SelectedCellEligibleMoves { get; private set; } 
  
    public bool MoveSelectedPiece(Vector2i position)
    {
        if (SelectedCellEligibleMoves?.All(cell => cell.Position != position) ?? true)
            return false;

        if (SelectedCell is not { Piece: not null }) 
            return false;

        if (SelectedCell.Piece.PieceType == PieceType.King && CheckForCheckAtPosition(position))
            return false;
            
        if (game.Board.MovePiece(SelectedCell, game.Board.GetCell(position)))
        {
            SelectedCell = null;
            return true;
        }
        return false;
    }
    
    public Cell[] GetAllOppositeMoves(PieceColor color)
    {
        var enemyPieces = game.Board.GetOppositeColorCells(color);
        return enemyPieces.SelectMany(cell => GetEligibleMoves(cell.Piece!)).ToArray();
    }
    
    public void SelectCell(Vector2i position)
    {
        var cell = game.Board.GetCell(position);
        if (cell is { Piece: not null }) {
            SelectedCell = cell;
        }
    }

    public bool IsSelectedCell(Vector2i position)
    {
        var cell = game.Board.GetCell(position);
        
        return cell == SelectedCell;
    }

    public void DeselectCell()
    {
        SelectedCell = null;
    }
    
    // ReSharper disable once ReturnTypeCanBeEnumerable.Local
    public Cell[] GetEligibleMoves(Piece piece)
    {
        var moves = piece.PieceType switch {
            PieceType.Pawn => PawnMoves(piece),
            PieceType.King => KingMoves(piece),
            PieceType.Rook => RookMoves(piece),
            PieceType.Bishop => BishopMoves(piece),
            PieceType.Queen => QueenMoves(piece),
            PieceType.Knight => KnightMoves(piece),
            _ => throw new ArgumentOutOfRangeException()
        };
        return moves.ToArray();
    }

    private IEnumerable<Cell> PawnMoves(Piece piece)
    {
        var board = game.Board;
        var yDir = piece.Color == PieceColor.Black ? 1 : -1;
        
        if (piece.MoveCounter == 1 && board.TryGetCell(piece.Position + new Vector2i(0, yDir * 2), out var startMove) && startMove is { Piece: null }) 
            yield return startMove;
        if (board.TryGetCell(piece.Position + new Vector2i(0, yDir), out var move) && move is {Piece: null })
            yield return move;
        if (board.TryGetEnemyCell(piece.Position + new Vector2i(1, yDir), piece.Color, out var attack1))
            yield return attack1!;
        if (board.TryGetEnemyCell(piece.Position + new Vector2i(-1, yDir), piece.Color, out var attack2))
            yield return attack2!;
    }

    private IEnumerable<Cell> KingMoves(Piece piece)
    {
        var board = game.Board;
        
        Cell?[] moves = [
            board.GetCell(piece.Position + new Vector2i(-1, -1)),
            board.GetCell(piece.Position + new Vector2i( 0, -1)),
            board.GetCell(piece.Position + new Vector2i( 1, -1)),
            board.GetCell(piece.Position + new Vector2i( 1,  0)),
            board.GetCell(piece.Position + new Vector2i( 1,  1)),
            board.GetCell(piece.Position + new Vector2i( 0,  1)),
            board.GetCell(piece.Position + new Vector2i(-1,  1)),
            board.GetCell(piece.Position + new Vector2i(-1,  0)),
        ];

        return moves
            .Where(x => x is not null && (x.Piece == null || x.Piece.Color != piece.Color))
            .Cast<Cell>();
    }

    private IEnumerable<Cell> RookMoves(Piece piece)
    {
        return GetCellsInDirection(piece, new Vector2i(0, -1))
            .Concat(GetCellsInDirection(piece, new Vector2i(0, 1)))
            .Concat(GetCellsInDirection(piece, new Vector2i(-1, 0)))
            .Concat(GetCellsInDirection(piece, new Vector2i(1, 0)));
    }
    
    private IEnumerable<Cell> BishopMoves(Piece piece)
    {
        return GetCellsInDirection(piece, new Vector2i(-1, -1))
            .Concat(GetCellsInDirection(piece, new Vector2i(1, -1)))
            .Concat(GetCellsInDirection(piece, new Vector2i(1, 1)))
            .Concat(GetCellsInDirection(piece, new Vector2i(-1, 1)));
    }
    
    private IEnumerable<Cell> QueenMoves(Piece piece)
    {
        return BishopMoves(piece).Concat(RookMoves(piece));
    }

    private IEnumerable<Cell> KnightMoves(Piece piece)
    {
        var board = game.Board;
        
        Cell?[] cells = [
            board.GetCell(piece.Position + new Vector2i( 1, -2)),
            board.GetCell(piece.Position + new Vector2i( 2, -1)),
            board.GetCell(piece.Position + new Vector2i( 2,  1)),
            board.GetCell(piece.Position + new Vector2i( 1,  2)),
            board.GetCell(piece.Position + new Vector2i(-1,  2)),
            board.GetCell(piece.Position + new Vector2i(-2,  1)),
            board.GetCell(piece.Position + new Vector2i(-2, -1)),
            board.GetCell(piece.Position + new Vector2i(-1, -2)),
        ];
        
        return cells
            .Where(x => x is not null && (x.Piece == null || x.Piece.Color != piece.Color))
            .Cast<Cell>();
    }
 
    private IEnumerable<Cell> GetCellsInDirection(Piece piece, Vector2i dir)
    {
        var board = game.Board;
        
        for (var i = 0; i < Board.MaxTileCount; i++)
        {
            var cell = board.GetCell(piece.Position + dir * (i+1));
            if (cell is null) break;
            if (cell is { Piece: not null })
            {
                if (cell.Piece.Color != piece.Color)
                    yield return cell;
                
                yield break;
            }
            
            yield return cell;
        }
    }
    
    public bool CheckForCheck()
    {
        var king = game.CurrentPlayer == PieceColor.White ? game.Board.KingWhite : game.Board.KingBlack;
        var enemyMoves = GetAllOppositeMoves(game.CurrentPlayer);
        return enemyMoves.Any(cell => cell.Piece == king);
    }
    
    private bool CheckForCheckAtPosition(Vector2i position)
    {
        var enemyMoves = GetAllOppositeMoves(game.CurrentPlayer);
        return enemyMoves.Any(cell => cell.Position == position);
    }
}