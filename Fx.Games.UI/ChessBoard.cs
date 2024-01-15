namespace Games;

public class ChessBoard
{
    private readonly Piece?[,] grid;

    public ChessBoard()
    {
        grid = new Piece?[8, 8] {
            {Piece.BlackRook, Piece.BlackKnight,  Piece.BlackBishop, Piece.BlackQueen, Piece.BlackKing,  Piece.BlackBishop,  Piece.BlackKnight,  Piece.BlackRook},
            {Piece.BlackPawn, Piece.BlackPawn,  Piece.BlackPawn, Piece.BlackPawn,  Piece.BlackPawn,  Piece.BlackPawn,  Piece.BlackPawn,  Piece.BlackPawn},
            {null, null, null, null, null, null, null, null},
            {null, null, null, null, null, null, null, null},
            {null, null, null, null, null, null, null, null},
            {null, null, null, null, null, null, null, null},
            {Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn,  Piece.WhitePawn,  Piece.WhitePawn,  Piece.WhitePawn,  Piece.WhitePawn,  Piece.WhitePawn},
            {Piece.WhiteRook, Piece.WhiteKnight,  Piece.WhiteBishop,    Piece.WhiteQueen, Piece.WhiteKing, Piece.WhiteBishop,  Piece.WhiteKnight,  Piece.WhiteRook}
        };
    }

    public Piece? this[int file, int rank] { get => grid[rank, file]; }

    public Piece? this[Coordinate coordinate]
    {
        get => grid[coordinate.Rank, coordinate.File];
        private set => grid[coordinate.Rank, coordinate.File] = value;
    }

    public IEnumerable<Move> Moves(Coordinate origin)
    {
        var piece = this[origin];
        if (!piece.HasValue) { yield break; }
        switch (piece.Value.Kind)
        {
            case PieceKind.Knight:
                var destinations = LeaperDestinations(origin, (1, 2));
                foreach (var (dest, capture) in destinations)
                {
                    yield return new Move(origin, dest, capture);
                }
                break;
            case PieceKind.Bishop:
                destinations = RiderDestinations(origin, (1, 1));
                foreach (var (dest, capture) in destinations)
                {
                    yield return new Move(origin, dest, capture);
                }
                break;
            case PieceKind.Rook:
                destinations = RiderDestinations(origin, (1, 0));
                foreach (var (dest, capture) in destinations)
                {
                    yield return new Move(origin, dest, capture);
                }
                break;
            case PieceKind.Queen:
                destinations = RiderDestinations(origin, (1, 0)).Concat(RiderDestinations(origin, (1, 1)));
                foreach (var (dest, capture) in destinations)
                {
                    yield return new Move(origin, dest, capture);
                }
                break;
            case PieceKind.king:
                destinations = LeaperDestinations(origin, (1, 0)).Concat(LeaperDestinations(origin, (1, 1)));
                foreach (var (dest, capture) in destinations)
                {
                    yield return new Move(origin, dest, capture);
                }
                // https://en.wikipedia.org/wiki/Castling
                var startingSquare = new Coordinate(4, piece.Value.Player == PlayerColor.White ? 7 : 0);
                if (origin == startingSquare)
                {
                    yield return new Move(origin, origin + new Coordinate(2, 0), false);
                    yield return new Move(origin, origin + new Coordinate(-2, 0), false);
                }
                break;
            case PieceKind.Pawn:
                foreach (var mv in PawnMoves(origin, piece.Value))
                {
                    yield return mv;
                }
                break;
            default:
                break;
        }
    }

    private IEnumerable<Move> PawnMoves(Coordinate origin, Piece piece)
    {
        var fwd = piece.Player == PlayerColor.White ? new Coordinate(0, -1) : new Coordinate(0, 1);
        var initialRank = piece.Player == PlayerColor.White ? 6 : 1;

        var destination = origin + fwd;
        // one forward
        if (OnBoard(destination) && this[destination] == null)
        {
            yield return new Move(origin, destination, false);
        }
        // two forward if on initial rank
        if (origin.Rank == initialRank)
        {
            destination = origin + fwd * 2;
            if (OnBoard(destination) && this[destination] == null)
            {
                yield return new Move(origin, destination, false);
            }
        }
        // capture to left or right 
        foreach (var lr in new[] { 1, -1 })
        {
            destination = origin + fwd + new Coordinate(lr, 0);
            if (!OnBoard(destination)) { continue; }
            var destinationPiece = this[destination];
            if (destinationPiece != null && destinationPiece.Value.Player != piece.Player)
            {
                yield return new Move(origin, destination, true);
            }
        }
    }

    // https://en.wikipedia.org/wiki/Fairy_chess_piece#Leapers
    private IEnumerable<(Coordinate Destination, bool Capture)> LeaperDestinations(Coordinate origin, (int, int) value)
    {
        var piece = this[origin];
        if (piece == null) { yield break; }

        foreach (var offset in OffsetsFromLeap(value))
        {
            var destination = origin + offset;
            if (!OnBoard(destination)) { continue; } // off board

            var destinationPiece = this[destination];
            if (destinationPiece == null)
            {

                yield return (destination, false);
            }
            else if (destinationPiece.Value.Player != piece.Value.Player)
            {
                yield return (destination, true);
            }
        }
    }

    // https://en.wikipedia.org/wiki/Fairy_chess_piece#Riders
    private IEnumerable<(Coordinate Destination, bool Capture)> RiderDestinations(Coordinate origin, (int, int) dir)
    {
        var piece = this[origin];
        if (piece == null) { yield break; }

        foreach (var offset in OffsetsFromLeap(dir))
        {
            for (int i = 1; i < 8; i++)
            {
                var destination = origin + offset * i;
                if (!OnBoard(destination)) { break; }
                var destinationPiece = this[destination];
                if (destinationPiece == null)
                {
                    // if this reached a an empty square, 
                    // return a non-capturing move and continue the inner loop
                    yield return (destination, false);
                }
                else if (destinationPiece.Value.Player != piece.Value.Player)
                {
                    // if this reached a piece with the opponent's color, 
                    // return a capturing move and end the inner loop
                    yield return (destination, true);
                    break;
                }
                else if (destinationPiece.Value.Player == piece.Value.Player)
                {
                    // if this reached a piece with the player's  color, 
                    // return nothing and end the inner loop
                    break;
                }
            }
        }
    }

    private static bool OnBoard(Coordinate c) => c.File >= 0 && c.File < 8 && c.Rank >= 0 && c.Rank < 8;

    private static IEnumerable<Coordinate> OffsetsFromLeap((int M, int N) leap)
    {
        var (m, n) = leap;
        if (m == 0)
        {
            return [(m, n), (m, -n), (n, m), (-n, m)];
        }
        else if (n == 0)
        {
            return [(m, n), (-m, n), (n, m), (n, -m)];
        }
        else
        {
            return [(m, n), (m, -n), (-m, n), (-m, -n), (n, m), (n, -m), (-n, m), (-n, -m)];
        }
    }

    public bool Commit(Move mv, [MaybeNullWhen(false)] out Piece captured)
    {
        var piece = this[mv.Origin];
        Debug.Assert(piece != null);
        var destinationPiece = this[mv.Destination];
        Debug.Assert(
            mv.Capture ?
                 destinationPiece != null && destinationPiece.Value.Player != piece.Value.Player :
                 destinationPiece == null);

        this[mv.Destination] = piece;
        this[mv.Origin] = null;
        captured = destinationPiece ?? default;
        return destinationPiece != null;
    }
}


public static class ChessBoardExtensions
{
    public static string ToFEN(this ChessBoard board)
    {
        var s = new StringBuilder();
        for (int y = 0; y < 8; y++)
        {
            if (y != 0) { s.Append('/'); }
            for (int x = 0; x < 8; x++)
            {
                var piece = board[x, y];
                if (piece == null)
                {
                    int k = x;
                    while (k < 8 && board[k, y] == null) { k += 1; }
                    s.Append(k - x);
                    x = k - 1;
                }
                else
                {
                    s.Append(piece.Value.Symbol);
                }
            }
        }
        return s.ToString();
    }
}