using System.Runtime.CompilerServices;

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
            : this(white, black, new ChessGameState(), ChessPieceColor.White)
        {
        }

        private Chess(TPlayer white, TPlayer black, ChessGameState newState, ChessPieceColor newCurrentPlayerColor)
        {
            players = new[] { white, black };

            Board = newState;
            CurrentPlayerColor = newCurrentPlayerColor;
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

        private static readonly Direction[] BISHOP_DIRECTIONS = new Direction[]
        {
            Direction.NE,
            Direction.SE,
            Direction.SW,
            Direction.NW,
        };

        private static readonly Direction[] ROOK_DIRECTIONS = new Direction[]
        {
            Direction.N,
            Direction.E,
            Direction.S,
            Direction.W,
        };

        private static readonly Direction[] PAWN_DIRECTIONS = new Direction[]
        {
            Direction.N,
        };

        private static readonly Direction[] KING_DIRECTIONS = new Direction[]
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

        private static readonly Direction[] KNIGHT_DIRECTION = new Direction[]
        {
            new Direction(1, 2),
            new Direction(1, -2),
            new Direction(-1, 2),
            new Direction(-1, -2),
            new Direction(2, 1),
            new Direction(2, -1),
            new Direction(-2, 1),
            new Direction(-2, -1),
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
                            foreach (var move in ComputeMoves(source, sourcePiece, board, QUEEN_DIRECTIONS, 7))
                            {
                                yield return move;  
                            }
                            break;
                        case ChessPieceKind.Bishop:
                            foreach (var move in ComputeMoves(source, sourcePiece, board, BISHOP_DIRECTIONS, 7))
                            {
                                yield return move;
                            }
                            break;
                        case ChessPieceKind.Rook:
                            foreach (var move in ComputeMoves(source, sourcePiece, board, ROOK_DIRECTIONS, 7))
                            {
                                yield return move;
                            }
                            break;
                        case ChessPieceKind.Pawn:
                            foreach (var move in ComputeMoves(source, sourcePiece, board, PAWN_DIRECTIONS, 1))
                            {
                                yield return move;
                            }
                            //// TODO other pawn moves here
                            break;
                        case ChessPieceKind.King:
                            foreach (var move in ComputeMoves(source, sourcePiece, board, KING_DIRECTIONS, 1))
                            {
                                yield return move;
                            }
                            //// TODO other king moves here
                            break;
                        case ChessPieceKind.Knight:
                            foreach (var move in ComputeMoves(source, sourcePiece, board, KNIGHT_DIRECTION, 1))
                            {
                                yield return move;
                            }
                            break;
                        default:
                            throw new InvalidOperationException($"No piece of kind {sourcePiece.Kind} found");
                    }
                }
            }
        }

        private static IEnumerable<ChessMove> ComputeMoves(Coordinate source, ChessPiece sourcePiece, ChessBoard board, Direction[] directions, int maxDistance)
        {

            foreach (var dir in directions)
            {
                for (int distance = 1; distance <= maxDistance; distance++)
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
        }


        public Outcome<TPlayer> Outcome => null; //// TODO TODO TODO this and "check" rules

        public Chess<TPlayer> CommitMove(ChessMove move)
        {
            /*if (!this.Moves.Contains(move)) //// TODO is this equatable?
            {
                throw new IllegalMoveExeption();
            }*/

            var clonedBoard = this.Board.Board.Board.Clone2();
            var piece = clonedBoard[move.From.y, move.From.x];
            clonedBoard[move.From.y, move.From.x] = null;
            clonedBoard[move.To.y, move.To.x] = piece;

            var newBoard = new ChessBoard(clonedBoard);
            var newGameState = new ChessGameState(newBoard);

            var game = new Chess<TPlayer>(this.players[0], this.players[1], newGameState, (ChessPieceColor)(((int)CurrentPlayerColor + 1) % 2));
            return game;
        }
    }
}


public static class CloneExtension
{
    public static void DoWork()
    {
        new Foo().Clone2();
        new Bar().Clone2();
    }

    private class Foo : ICloneable
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    private struct Bar : ICloneable
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public static T Clone2<T>(this T toClone) where T : class, ICloneable
    {
        return toClone.Clone() as T;
    }

    public static T Clone2<T>(this T toClone, [CallerMemberName] string caller = null) where T : struct, ICloneable
    {
        return (T)toClone.Clone();
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
