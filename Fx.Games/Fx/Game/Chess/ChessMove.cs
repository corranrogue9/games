namespace Fx.Game.Chess
{

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
            return $"Move{{{Piece} {From} {To}}}";
        }
    }

}