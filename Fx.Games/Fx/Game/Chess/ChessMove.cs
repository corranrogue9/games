using System.Drawing;
using Microsoft.VisualBasic;

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

        public static ChessMove KingSideCastling(ChessPieceColor color)
        {
            var rank = color == ChessPieceColor.White ? 0 : 7;
            return new(new ChessPiece(color, ChessPieceKind.King), new(4, rank), new(6, rank));
        }

        public static ChessMove QueenSideCastling(ChessPieceColor color)
        {
            var rank = color == ChessPieceColor.White ? 0 : 7;
            return new(new ChessPiece(color, ChessPieceKind.King), new(4, rank), new(2, rank));
        }

        public static readonly ChessMove WhiteKingSideCastling = KingSideCastling(ChessPieceColor.White);

        public static readonly ChessMove BlackKingSideCastling = KingSideCastling(ChessPieceColor.Black);

        public static readonly ChessMove WhiteQueenSideCastling = QueenSideCastling(ChessPieceColor.White);

        public static readonly ChessMove BlackQueenSideCastling = QueenSideCastling(ChessPieceColor.Black);

        public bool IsCastling => this.Piece.Kind == ChessPieceKind.King
            // && this.From.x == 4
            && Math.Abs(this.To.x - this.From.x) == 2;

        public bool IsKingSideCastling => this.Piece.Kind == ChessPieceKind.King
            && this.From.x == 4
            && this.To.x == 6;

        public bool IsQueenSideCastling => this.Piece.Kind == ChessPieceKind.King
            && this.From.x == 4
            && this.To.x == 2;

        #endregion
    }
}