namespace Fx.Game.Chess
{
    /// <summary>
    /// https://www.wikiwand.com/en/Forsyth%E2%80%93Edwards_Notation
    /// </summary>
    public sealed class ChessGameState
    {
        public ChessGameState()
            : this(new ChessBoard())
        {
        }

        public ChessGameState(ChessBoard board)
        {
            Board = board;
        }

        public ChessBoard Board { get; }

        public bool WhiteCastlingAvailability { get; }

        public bool BlackCastlingAvailability { get; }

        // TODO: En passant target square and move-clock.
    }
}
