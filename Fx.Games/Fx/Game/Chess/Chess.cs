namespace Fx.Game.Chess
{
    using System;
    using System.Collections.Generic;

    public sealed class ChessMove
    {
        public ChessMove(ChessPiece piece, Coordinate from, Coordinate to)
        {
            this.Piece = piece;
            this.From = from;
            this.To = to;
        }

        public ChessPiece Piece { get; }

        public Coordinate From { get; }

        public Coordinate To { get; }

        [Obsolete]
        public ChessPiece? Captured { get; }

        override public string ToString()
        {
            return $"Move {Piece} {From} {To}";
        }
    }


    /// <threadsafety static="true" instance="true"/>
    public sealed class Chess<TPlayer> : IGame<Chess<TPlayer>, ChessGameState, ChessMove, TPlayer>
    {

        private readonly TPlayer[] players;

        public ChessGameState Board { get; }

        public Chess(TPlayer white, TPlayer black)
        {
            players = new[] { white, black };

            Board = new ChessGameState();
            CurrentPlayerColor = ChessPieceColor.White;
        }

        public TPlayer CurrentPlayer => players[(int)CurrentPlayerColor];

        private ChessPieceColor CurrentPlayerColor { get; }

        private static readonly Direction[] QUEEN_DIRECTIONS = new Direction[]
        {
            Direction.N,
            Direction.NE,
            Direction.E, 
            Direction.SE,
            Direction.S, 
            Direction.SW,
            Direction.W,
            Direction.NW,
        };

        public IEnumerable<ChessMove> Moves
        {
            get
            {
                var board = Board.Board;
                foreach (var source in Coordinate.All)
                {
                    var maybeSourcePiece = board[source];
                    if (maybeSourcePiece == null || maybeSourcePiece.Value.Color != CurrentPlayerColor)
                    {
                        continue;
                    }

                    var sourcePiece = maybeSourcePiece.Value;
                    // TODO: simplify above

                    switch (sourcePiece.Kind)
                    {
                        case ChessPieceKind.Queen:
                            foreach (var dir in QUEEN_DIRECTIONS)
                            {
                                for (int distance = 1; distance < 8; distance++)
                                {
                                    var target = source + dir * distance;
                                    if (!target.IsOnBoard)
                                    {
                                        break;
                                    }
                                    if (board[target] == null)
                                    {
                                        yield return new ChessMove(sourcePiece, source, target);
                                    }
                                    else if (board[target].Value.Color == sourcePiece.Color)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        // capturing move
                                        yield return new ChessMove(sourcePiece, source, target);
                                        break;
                                    }

                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        public Outcome<TPlayer> Outcome => throw new NotImplementedException();

        public Chess<TPlayer> CommitMove(ChessMove move)
        {
            throw new NotImplementedException();
        }
    }
}


public sealed class Coordinate
{
    internal Coordinate(int x, int y) { this.x = x; this.y = y; }

    public readonly int x;
    public readonly int y;

    public static Coordinate operator +(Coordinate coordinate, Direction dir)
    {
        return new Coordinate(coordinate.x + dir.dx, coordinate.y + dir.dy);
    }

    public override string ToString() => $"{"abcdefgh"[x]}{y + 1}";

    public static IEnumerable<Coordinate> All
    {
        get
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    yield return new Coordinate(x, y);
                }
            }
        }
    }

    public bool IsOnBoard { get => this.x >= 0 && this.y >= 0 && this.x < 8 && this.y < 8; }
}

public sealed class Direction
{
    public Direction(int dx, int dy) { this.dx = dx; this.dy = dy; }
    public readonly int dx;
    public readonly int dy;

    public static Direction operator *(Direction dir, int distance)
    {
        return new Direction(dir.dx * distance, dir.dy * distance);
    }

    public static readonly Direction N = new Direction(0, -1);
    public static readonly Direction S = new Direction(0, +1);
    public static readonly Direction W = new Direction(-1, 0);
    public static readonly Direction E = new Direction(+1, 0);

    public static readonly Direction NW = new Direction(-1, -1);
    public static readonly Direction SW = new Direction(-1, +1);
    public static readonly Direction NE = new Direction(+1, -1);
    public static readonly Direction SE = new Direction(+1, +1);

}
