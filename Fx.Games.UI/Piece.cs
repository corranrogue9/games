namespace Games;

public enum PlayerColor { Black, White };

public enum PieceKind { king, Queen, Rook, Bishop, Knight, Pawn };

public readonly record struct Piece(PlayerColor Player, PieceKind Kind) : IFormattable
{

    public static readonly Piece BlackKing = new(PlayerColor.Black, PieceKind.king);
    public static readonly Piece BlackQueen = new(PlayerColor.Black, PieceKind.Queen);
    public static readonly Piece BlackBishop = new(PlayerColor.Black, PieceKind.Bishop);
    public static readonly Piece BlackKnight = new(PlayerColor.Black, PieceKind.Knight);
    public static readonly Piece BlackRook = new(PlayerColor.Black, PieceKind.Rook);
    public static readonly Piece BlackPawn = new(PlayerColor.Black, PieceKind.Pawn);

    public static readonly Piece WhiteKing = new(PlayerColor.White, PieceKind.king);
    public static readonly Piece WhiteQueen = new(PlayerColor.White, PieceKind.Queen);
    public static readonly Piece WhiteBishop = new(PlayerColor.White, PieceKind.Bishop);
    public static readonly Piece WhiteKnight = new(PlayerColor.White, PieceKind.Knight);
    public static readonly Piece WhiteRook = new(PlayerColor.White, PieceKind.Rook);
    public static readonly Piece WhitePawn = new(PlayerColor.White, PieceKind.Pawn);


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
