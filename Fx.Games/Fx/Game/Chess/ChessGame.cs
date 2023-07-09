

namespace Fx.Game.Chess
{


    /// <threadsafety static="true" instance="true"/>
    public sealed class ChessGame<TPlayer> : IGame<ChessGame<TPlayer>, ChessGameState, ChessMove, TPlayer>
    {

        private readonly TPlayer[] players;

        public ChessGameState Board { get; }

        public ChessGame(TPlayer white, TPlayer black)
            : this(white, black, new ChessGameState(), ChessPieceColor.White)
        {
        }

        private ChessGame(TPlayer white, TPlayer black, ChessGameState newState, ChessPieceColor newCurrentPlayerColor)
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
                foreach (Square source in Square.All)
                {
                    var maybeSourcePiece = board[source];
                    if (maybeSourcePiece == null || maybeSourcePiece.Value.Color != CurrentPlayerColor)
                    {
                        continue;
                    }

                    var sourcePiece = maybeSourcePiece.Value;

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
                            var finalRank = CurrentPlayerColor == ChessPieceColor.White ? 7 : 0;

                            // single step move
                            var singleStep = source + forward;
                            if (singleStep.IsOnBoard
                                && !Board.Board.TryGetPiece(singleStep, out var _)
                                && singleStep.y != finalRank)
                            {
                                yield return new ChessMove(sourcePiece, source, singleStep);
                            };

                            // initial two step moves
                            if (source.y == startingRank && !Board.Board[source + forward].HasValue)
                            {
                                yield return new ChessMove(sourcePiece, source, source + forward * 2);
                            }

                            // capture opponent's piece
                            foreach (Direction dir in new[] { new Direction(1, forward.dy), new Direction(-1, forward.dy) })
                            {
                                var target = source + dir;
                                if (target.IsOnBoard && Board.Board.TryGetPiece(target, out var targetPiece) && targetPiece.Color != CurrentPlayerColor)
                                {
                                    yield return new ChessMove(sourcePiece, source, target);
                                };
                            }
                            // Promotion
                            if (source.y == finalRank)
                            {
                                var promotionTarget = new Square(source.x, finalRank);
                                foreach (ChessPieceKind promoteIntoKind in new[] { ChessPieceKind.Queen, ChessPieceKind.Knight })
                                {
                                    // TODO: ensure that CommitMove understands this as a promotion
                                    var promotecInto = new ChessPiece(CurrentPlayerColor, promoteIntoKind);
                                    yield return new ChessMove(promotecInto, source, promotionTarget);
                                }
                            }
                            // TODO en passant
                            break;
                        case ChessPieceKind.King:
                            foreach (var move in ComputeMoves(source, sourcePiece, board, KING_DIRECTIONS, 1))
                            {
                                yield return move;
                            }
                            // castling
                            var rank = CurrentPlayerColor == ChessPieceColor.White ? 0 : 7;
                            if (source == new Square(4, rank) // king on its original position
                            //  && this.Board.CastlingAvailable(CurrentPlayerColor)
                            )
                            {
                                var right = Direction.E;
                                var kingsideRookSquare = this.Board.Board[source + right * 3];
                                if (!this.Board.Board[source + right * 1].HasValue
                                    && !this.Board.Board[source + right * 2].HasValue
                                    && kingsideRookSquare.HasValue && kingsideRookSquare.Value.Kind == ChessPieceKind.Rook)
                                {
                                    yield return ChessMove.KingSideCastling(CurrentPlayerColor);
                                }
                                var left = Direction.W;
                                var queensideRookSquare = this.Board.Board[source + left * 4];
                                if (this.Board.CastlingAvailable(CurrentPlayerColor)
                                    && !this.Board.Board[source + left * 1].HasValue
                                    && !this.Board.Board[source + left * 2].HasValue
                                    && !this.Board.Board[source + left * 3].HasValue
                                    && queensideRookSquare.HasValue && queensideRookSquare.Value.Kind == ChessPieceKind.Rook)
                                {
                                    yield return ChessMove.QueenSideCastling(CurrentPlayerColor);
                                }
                            }
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

        private static IEnumerable<ChessMove> ComputeMoves(Square source, ChessPiece sourcePiece, ChessBoard board, Direction[] directions, int maxDistance)
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

        public ChessGame<TPlayer> CommitMove(ChessMove move)
        {
            /*if (!this.Moves.Contains(move)) //// TODO is this equatable?
            {
                throw new IllegalMoveExeption();
            }*/

            var clonedBoard = this.Board.Board.Board.Clone2();
            var piece = clonedBoard[move.From.y, move.From.x];
            clonedBoard[move.From.y, move.From.x] = null;
            clonedBoard[move.To.y, move.To.x] = piece;

            // move rook when castling. Castling is irepresented by the king moving 2 squares horizontally
            if (move.IsCastling)
            {
                if (move.From.x < move.To.x) // castling right i.e. king side, move rook left
                {
                    Swap(ref clonedBoard, new Square(7, move.To.y), new Square(5, move.To.y));
                }
                else if (move.From.x > move.To.x) // castling right i.e. queen side, move rook right
                {
                    Swap(ref clonedBoard, new Square(0, move.To.y), new Square(3, move.To.y));
                }
            }

            var newBoard = new ChessBoard(clonedBoard);
            var newGameState = new ChessGameState(newBoard);

            var game = new ChessGame<TPlayer>(this.players[0], this.players[1], newGameState, (ChessPieceColor)(((int)CurrentPlayerColor + 1) % 2));
            return game;
        }

        private static void Swap(ref ChessPiece?[,] board, Square square1, Square square2)
        {
            var piece1 = board[square1.y, square1.x];
            var piece2 = board[square2.y, square2.x];
            board[square1.y, square1.x] = piece2;
            board[square2.y, square2.x] = piece1;
        }
    }
}
