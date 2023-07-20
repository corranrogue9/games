namespace Fx.Game.Chess;

[Flags]
public enum CastlingAvailability
{
    None = 0,

    WhiteQueenSide = 1,
    WhiteKingSide = 2,
    BlackQueenSide = 4,
    BlackKingSide = 8,

    All = 1 | 2 | 4 | 8,
}

public static class CastlingAvailabilities
{
    public static CastlingAvailability FromChars(string chars)
    {
        var res = CastlingAvailability.None;
        foreach (var ch in chars)
        {
            switch (ch)
            {
                case 'k': res |= CastlingAvailability.BlackKingSide; break;
                case 'q': res |= CastlingAvailability.BlackQueenSide; break;
                case 'K': res |= CastlingAvailability.WhiteKingSide; break;
                case 'Q': res |= CastlingAvailability.WhiteQueenSide; break;
                default:
                    throw new InvalidDataException($"invalid char {ch} for castling");
            }
        }
        return res;
    }
}


/// <summary>
/// https://www.wikiwand.com/en/Forsyth%E2%80%93Edwards_Notation
/// </summary>
public sealed class ChessGameState
{
    public ChessGameState()
        : this(new ChessBoard(), CastlingAvailability.All, ChessPieceColor.White, 0, 0)
    {
    }

    public ChessGameState(ChessBoard board, CastlingAvailability avail, ChessPieceColor active, int? halfMoveClock, int? fullMoveNumber)
    {
        Board = board;
        CastlingAvailability = avail;
        Active = active;
        HalfMoveClock = halfMoveClock;
        //// TODO this tightly couples game state with number of moves and cannot make its way to the final product because we won't wnat to tvalidate the corectness of that assertion
        FullMoveNumber = fullMoveNumber;
    }

    public ChessBoard Board { get; }


    public CastlingAvailability CastlingAvailability { get; }
    public ChessPieceColor Active { get; }
    public int? HalfMoveClock { get; }
    public int? FullMoveNumber { get; }

    internal bool IsCastlingAvailable(ChessPieceColor playerColor, bool queenSide)
    {
        var a = (playerColor, queenSide) switch
        {
            (ChessPieceColor.White, true) => CastlingAvailability.WhiteQueenSide,
            (ChessPieceColor.White, false) => CastlingAvailability.WhiteKingSide,
            (ChessPieceColor.Black, true) => CastlingAvailability.BlackQueenSide,
            (ChessPieceColor.Black, false) => CastlingAvailability.BlackKingSide,
            _ => throw new InvalidDataException($"invalid Color/castling side combination ${(playerColor, queenSide)}"),
        };
        return this.CastlingAvailability.HasFlag(a);
    }

    // internal bool CastlingAvailable(ChessPieceColor playerColor)
    // {
    //     return playerColor == ChessPieceColor.White ? WhiteCastlingAvailability : BlackCastlingAvailability;
    // }

    // TODO: En passant target square and move-clock.
}
