namespace Fx.Game.Chess
{

    public sealed class ChessMove : IEquatable<ChessMove>
    {
        public ChessMove(ChessPiece piece, Square from, Square to)
        {
            this.Piece = piece;
            this.From = from;
            this.To = to;
        }



        public ChessPiece Piece { get; }

        public Square From { get; }

        public Square To { get; }

        [Obsolete]
        public ChessPiece? Captured { get; }

        override public string ToString()
        {
            return $"Move{{{Piece} {From} {To}}}";
        }

        public bool Equals(ChessMove? other)
        {
            return other != null
                && this.Piece == other.Piece
                && this.From == other.From
                && this.To == other.To;
        }

        #region  castling
        // https://en.wikipedia.org/wiki/Castling
        // Castling consists of moving the king two squares towards a rook
        public static readonly ChessMove WhiteKingSideCastling =
            new(ChessPiece.WhiteKing, new(4, 0), new(6, 0));

        public static readonly ChessMove WhiteQueenSideCastling =
            new(ChessPiece.WhiteKing, new(4, 0), new(2, 0));

        public static readonly ChessMove BlackKingSideCastling =
                   new(ChessPiece.BlackKing, new(4, 7), new(6, 7));

        public static readonly ChessMove BlackQueenSideCastling =
            new(ChessPiece.BlackKing, new(4, 7), new(2, 7));

        public bool IsKingSideCastling => this.Piece.Kind == ChessPieceKind.King
            && this.From.x == 4
            && this.To.x == 6;

        public bool IsQueenSideCastling => this.Piece.Kind == ChessPieceKind.King
            && this.From.x == 4
            && this.To.x == 2;

        public bool IsCastling => this.Piece.Kind == ChessPieceKind.King
            && this.From.x == 4
            && Math.Abs(this.To.x - this.From.x) == 2;
        #endregion
    }
}