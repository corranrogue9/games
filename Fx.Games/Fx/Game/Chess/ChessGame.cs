

namespace Fx.Game.Chess
{


    /// <threadsafety static="true" instance="true"/>
    public sealed class ChessGame<TPlayer> : IGame<ChessGame<TPlayer>, ChessGameState, ChessMove, TPlayer>
    {

        private readonly TPlayer white;

        private readonly TPlayer black;

        public ChessGameState Board { get; }

        public ChessGame(TPlayer white, TPlayer black)
            : this(white, black, new ChessGameState(), ChessPieceColor.White)
        {
        }

        private ChessGame(TPlayer white, TPlayer black, ChessGameState newState, ChessPieceColor newCurrentPlayerColor)
        {
            this.white = white;
            this.black = black;

            Board = newState;
            CurrentPlayerColor = newCurrentPlayerColor;
        }

        public TPlayer CurrentPlayer => this.CurrentPlayerColor == ChessPieceColor.Black ? this.black : this.white;

        private ChessPieceColor CurrentPlayerColor { get; }

        private ChessPieceColor OpponentPlayerColor => (ChessPieceColor)((((int)this.CurrentPlayerColor) + 1) % 2);

        public IReadOnlyDictionary<TPlayer, ChessPieceColor> PlayerColors => new Dictionary<TPlayer, ChessPieceColor>
        { {this.white, ChessPieceColor.White }, {this.black, ChessPieceColor.Black } };

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
                var currentPlayerMoves = CalculateMoves(this.CurrentPlayerColor, this.Board);

                return currentPlayerMoves.Where(move => !HasACheckMove(this.OpponentPlayerColor, this.CommitMove(move).Board));

                if (HasACheckMove(this.OpponentPlayerColor, this.Board))
                {
                    return currentPlayerMoves.Where(move => !HasACheckMove(this.OpponentPlayerColor, this.CommitMove(move).Board));
                }
                else
                {
                    return currentPlayerMoves;
                }
            }
        }

        public static bool HasACheckMove(ChessPieceColor playerWhoHasACheckMove, ChessGameState chessGameState)
        {
            return CalculateMoves(playerWhoHasACheckMove, chessGameState)
                .Where(move => chessGameState.Board[move.To]?.Kind == ChessPieceKind.King) //// TODO we should check the color of the piece too, but the opponent can't move to their own king's square
                .Any();
        }

        private static IEnumerable<ChessMove> CalculateMoves(ChessPieceColor currentPlayerColor, ChessGameState chessGameState)
        {
            //// TODO implement being in check
            var board = chessGameState.Board;
            //// TODO we don't need this; we should have a lookup of currently populated spaces
            foreach (Square source in Square.All)
            {
                var maybeSourcePiece = board[source];
                if (maybeSourcePiece == null || maybeSourcePiece.Value.Color != currentPlayerColor)
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
                        //// TODO possible bugs: do pawns capture when moving forward?
                        var forward = new Direction(0, sourcePiece.Color == ChessPieceColor.White ? 1 : -1);
                        var startingRank = sourcePiece.Color == ChessPieceColor.White ? 1 : 6;
                        var finalRank = sourcePiece.Color == ChessPieceColor.White ? 7 : 0;

                        // single step move
                        var singleStep = source + forward;
                        if (singleStep.IsOnBoard
                            && !board.TryGetPiece(singleStep, out var _)
                            && singleStep.y != finalRank)
                        {
                            yield return new ChessMove(sourcePiece, source, singleStep);
                        };

                        // initial two step moves
                        if (source.y == startingRank && !board[source + forward].HasValue)
                        {
                            yield return new ChessMove(sourcePiece, source, source + forward * 2);
                        }

                        // capture opponent's piece
                        foreach (Direction dir in new[] { new Direction(1, forward.dy), new Direction(-1, forward.dy) })
                        {
                            var target = source + dir;
                            if (target.IsOnBoard && board.TryGetPiece(target, out var targetPiece) && targetPiece.Color != currentPlayerColor)
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
                                var promotecInto = new ChessPiece(currentPlayerColor, promoteIntoKind);
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
                        var rank = currentPlayerColor == ChessPieceColor.White ? 0 : 7;
                        if (source == new Square(4, rank) // king on its original position
                                                          //  && this.Board.CastlingAvailable(CurrentPlayerColor)
                        )
                        {
                            //// TODO the castling logic should use the lookup that is used for the overall moves computation to look for pieces that could possible attack the empty spaces
                            var right = Direction.E;
                            var kingsideRookSquare = board[source + right * 3];
                            if (!board[source + right * 1].HasValue
                                && !board[source + right * 2].HasValue
                                && kingsideRookSquare.HasValue && kingsideRookSquare.Value.Kind == ChessPieceKind.Rook)
                            {
                                yield return ChessMove.KingSideCastling(currentPlayerColor);
                            }
                            var left = Direction.W;
                            var queensideRookSquare = board[source + left * 4];
                            if (chessGameState.CastlingAvailable(currentPlayerColor)
                                && !board[source + left * 1].HasValue
                                && !board[source + left * 2].HasValue
                                && !board[source + left * 3].HasValue
                                && queensideRookSquare.HasValue && queensideRookSquare.Value.Kind == ChessPieceKind.Rook)
                            {
                                yield return ChessMove.QueenSideCastling(currentPlayerColor);
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


        public Outcome<TPlayer> Outcome
        {
            get
            {
                if (this.Moves.Any()) //// TODO if the player has no moves, but isn't in check, then it's actually a draw...
                {
                    return null;
                }

                return new Outcome<TPlayer>(new[] { this.CurrentPlayerColor == ChessPieceColor.White ? this.black : this.white });
            }
        }

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

            // Castling is irepresented by the king moving 2 squares horizontally
            if (move.IsCastling)
            {
                // Move Rook. King is already moved above.
                if (move.From.x < move.To.x) // castling right i.e. king side, move rook left
                {
                    Swap(ref clonedBoard, new Square(7, move.To.y), new Square(5, move.To.y));
                }
                else if (move.From.x > move.To.x) // castling left i.e. queen side, move rook right
                {
                    Swap(ref clonedBoard, new Square(0, move.To.y), new Square(3, move.To.y));
                }
            }

            var newBoard = new ChessBoard(clonedBoard);
            var newGameState = new ChessGameState(newBoard, this.Board.HalfMoveCount + 1);

            var game = new ChessGame<TPlayer>(this.white, this.black, newGameState, (ChessPieceColor)(((int)CurrentPlayerColor + 1) % 2));
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
