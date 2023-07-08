namespace Fx.Game.Chess
{
    public record struct SANMove(SANPiece Piece, (char, short)? Start, bool Take, (char, short) Target, bool Check)
    {

        public override readonly string ToString()
        {
            return $"{{Move {Piece} {Start} {Take} {Target} {Check}}}";
        }
    }

    public enum SANPiece { King, Queen, Rook, Bishop, Knight, Pawn }

    public static class SanMoveExtensions
    {
        public static bool Matches(this SANMove san, ChessMove move)
        {
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

        public static bool Matches(this (char, short)? san, Coordinate coord)
        {
            return
                !san.HasValue ||
                san.Value.Matches(coord);
        }

        public static bool Matches(this (char, short) san, Coordinate coord)
        {
            return
                (san.Item1 == 0 || san.Item1 - 'a' == coord.x) &&
                (san.Item2 == 0 || san.Item2 - 1 == coord.y);
        }
    }
}