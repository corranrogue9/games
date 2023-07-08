namespace Fx.Game.Chess
{
    public record struct SANMove(SANPiece Piece, (char, short)? Start, bool Take, (char, short) Target, bool Check)
    {

        public static readonly SANMove KingSideCastling = new(SANPiece.King, ('e', 0), false, ('g', 0), false);
        public static readonly SANMove QueenSideCastling = new(SANPiece.King, ('e', 0), false, ('c', 0), false);

        public override readonly string ToString()
        {
            if (this == KingSideCastling)
            {
                return "{{Move Castling King Side}}";
            }
            else if (this == QueenSideCastling)
            {
                return "{{Move Castling Queen Side}}";
            }
            return $"{{Move {Piece} {Start} {Take} {Target} {Check}}}";
        }
    }

    public enum SANPiece { King, Queen, Rook, Bishop, Knight, Pawn }

    public static class SanMoveExtensions
    {
        public static bool Matches(this SANMove san, ChessMove move)
        {
            if (san == SANMove.KingSideCastling && move.IsKingSideCastling)
            {
                return true;
            }
            if (san == SANMove.QueenSideCastling && move.IsQueenSideCastling)
            {
                return true;
            }
            return
                san.Piece.Matches(move.Piece) &&
                san.Start.Matches(move.From) &&
                san.Target.Matches(move.To);
        }

        public static bool Matches(this SANPiece san, ChessPiece piece)
        {
            return (san, piece.Kind) switch
            {
                (SANPiece.Pawn, ChessPieceKind.Pawn) => true,
                (SANPiece.King, ChessPieceKind.King) => true,
                (SANPiece.Queen, ChessPieceKind.Queen) => true,
                (SANPiece.Knight, ChessPieceKind.Knight) => true,
                (SANPiece.Bishop, ChessPieceKind.Bishop) => true,
                (SANPiece.Rook, ChessPieceKind.Rook) => true,
                _ => false
            };
        }

        public static bool Matches(this (char, short)? san, Square square)
        {
            return
                !san.HasValue ||
                san.Value.Matches(square);
        }

        public static bool Matches(this (char, short) san, Square square)
        {
            return
                (san.Item1 == 0 || san.Item1 - 'a' == square.x) &&
                (san.Item2 == 0 || san.Item2 - 1 == square.y);
        }
    }
}