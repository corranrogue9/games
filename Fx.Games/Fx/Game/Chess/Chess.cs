
namespace Fx.Game.Chess
{


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
                            var forward = new Direction(0, CurrentPlayerColor == ChessPieceColor.White ? 1 : -1);
                            var startingRank = CurrentPlayerColor == ChessPieceColor.White ? 1 : 6;

                            // single step move
                            var singleStep = source + forward;
                            if (singleStep.IsOnBoard && !Board.Board.TryGetPiece(singleStep, out var _))
                            {
                                yield return new ChessMove(sourcePiece, source, singleStep);
                            };

                            // initial two step moves
                            if (source.y == startingRank)
                            {
                                yield return new ChessMove(sourcePiece, source, source + forward * 2);
                            }

                            // capture opponent's piece
                            foreach (var dir in new[] { new Direction(1, forward.dy), new Direction(1, forward.dy) })
                            {
                                var target = source + dir;
                                if (target.IsOnBoard && Board.Board.TryGetPiece(target, out var targetPiece) && targetPiece.Color != CurrentPlayerColor)
                                {
                                    yield return new ChessMove(sourcePiece, source, target);
                                };
                            }
                            //// TODO en passant
                            //// TODO Conversion
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
