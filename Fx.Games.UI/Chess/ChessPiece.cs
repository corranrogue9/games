namespace Games.Chess;

public enum PlayerColor { Black, White };

public enum PieceKind { king, Queen, Rook, Bishop, Knight, Pawn };

public readonly record struct ChessPiece(PlayerColor Player, PieceKind Kind) : IFormattable
{

    public static readonly ChessPiece BlackKing = new(PlayerColor.Black, PieceKind.king);
    public static readonly ChessPiece BlackQueen = new(PlayerColor.Black, PieceKind.Queen);
    public static readonly ChessPiece BlackBishop = new(PlayerColor.Black, PieceKind.Bishop);
    public static readonly ChessPiece BlackKnight = new(PlayerColor.Black, PieceKind.Knight);
    public static readonly ChessPiece BlackRook = new(PlayerColor.Black, PieceKind.Rook);
    public static readonly ChessPiece BlackPawn = new(PlayerColor.Black, PieceKind.Pawn);

    public static readonly ChessPiece WhiteKing = new(PlayerColor.White, PieceKind.king);
    public static readonly ChessPiece WhiteQueen = new(PlayerColor.White, PieceKind.Queen);
    public static readonly ChessPiece WhiteBishop = new(PlayerColor.White, PieceKind.Bishop);
    public static readonly ChessPiece WhiteKnight = new(PlayerColor.White, PieceKind.Knight);
    public static readonly ChessPiece WhiteRook = new(PlayerColor.White, PieceKind.Rook);
    public static readonly ChessPiece WhitePawn = new(PlayerColor.White, PieceKind.Pawn);


    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return format switch
        {
            "A" => this.Symbol,
            _ => $"{Enum.GetName<PlayerColor>(this.Player)} {Enum.GetName<PieceKind>(this.Kind)}",
        };
    }

    public string Symbol => SYMBOL[6 * (int)this.Player + (int)this.Kind].ToString();

    static readonly char[] SYMBOL = ['k', 'q', 'r', 'b', 'n', 'p', 'K', 'Q', 'R', 'B', 'N', 'P'];
}
