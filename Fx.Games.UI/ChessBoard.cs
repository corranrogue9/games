

namespace Games;

internal class ChessBoard
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

    public Piece? this[int file, int rank] { get => grid[file, rank]; }
    public Piece? this[Coordinate coordinate]
    {
        get => grid[coordinate.File, coordinate.Rank];
        private set => grid[coordinate.File, coordinate.Rank] = value;
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
        var initialFile = piece.Player == PlayerColor.White ? 6 : 1;

        var destination = origin + fwd;
        // one forward
        if (OnBoard(destination) && this[destination] == null)
        {
            yield return new Move(origin, destination, false);
        }
        // two forward if on initial rank
        if (origin.File == initialFile)
        {
            destination = origin + fwd * 2;
            if (OnBoard(destination) && this[destination] == null)
            {
                yield return new Move(origin, destination, false);
            }
        }
        // capture to left or right 
        foreach (var x in new[] { 1, -1 })
        {
            destination = origin + fwd + new Coordinate(x, 0);
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
            if (destinationPiece?.Player == piece?.Player)
            {
                continue;
            }

            var isCapture = destinationPiece != null && destinationPiece?.Player != piece?.Player;
            yield return (destination, isCapture);
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
                    yield return (destination, false);
                }
                else if (destinationPiece.Value.Player != piece.Value.Player)
                {
                    yield return (destination, true);
                    break;
                }
                else if (destinationPiece.Value.Player == piece.Value.Player)
                {
                    break;
                }
            }
        }
    }

    private static bool OnBoard(Coordinate c) => c.Rank >= 0 && c.Rank < 8 && c.File >= 0 && c.File < 8;

    private static IEnumerable<Coordinate> OffsetsFromLeap((int M, int N) leap)
    {
        var (m, n) = leap;
        if (m == n)
        {
            return [(m, n), (m, -n), (-m, n), (-m, -n)];
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


internal record struct Move(Coordinate Origin, Coordinate Destination, bool Capture)
{ }
