namespace Fx.Game
{
    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class ChessBoard<TPiece>
    {
        public ChessBoard(TPiece[][] board)
        {
            this.Board = board; //// TODO duplicate this, maybe revamp the entire chessboard construct
        }

        public TPiece[][] Board { get; }
    }
}
