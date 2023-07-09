namespace Fx.Game.Chess
{
    /// <summary>
    /// https://www.wikiwand.com/en/Forsyth%E2%80%93Edwards_Notation
    /// </summary>
    public sealed class ChessGameState
    {
        public ChessGameState()
            : this(new ChessBoard(), 0)
        {
        }

        public ChessGameState(ChessBoard board, int halfMoveCount)
        {
            Board = board;
            BlackCastlingAvailability = true;
            WhiteCastlingAvailability = true;
            this.HalfMoveCount = halfMoveCount; //// TODO this tightly couples game state with number of moves and cannot make its way to the final product because we won't wnat to tvalidate the corectness of that assertion
        }

        public ChessBoard Board { get; }

        public bool WhiteCastlingAvailability { get; }

        public bool BlackCastlingAvailability { get; }

        public int HalfMoveCount { get; }

        internal bool CastlingAvailable(ChessPieceColor playerColor)
        {
            return playerColor == ChessPieceColor.White ? WhiteCastlingAvailability : BlackCastlingAvailability;
        }

        // TODO: En passant target square and move-clock.
    }
}
