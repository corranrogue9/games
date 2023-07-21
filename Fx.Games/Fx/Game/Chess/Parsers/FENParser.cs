using Fx.Game.Chess;

namespace Fx.Game.Chess
{
    // https://www.wikiwand.com/en/Forsyth%E2%80%93Edwards_Notation
    public class FENParser
    {
        public static ChessBoard ParseBoard(string input)
        {
            if (FENParser.Board(input, out var rem, out var board))
            {
                return board;
            }
            throw new FormatException($"parse error at {input}");
        }

        public static ChessGameState ParseGame(string input)
        {
            if (FENParser.Game(input, out var rem, out var game))
            {
                return game;
            }
            throw new FormatException($"parse error at {input}");
        }


        static readonly Parser<char> PieceOrDigit =
             Parsers.Regex("[prbnkqPRBNKQ12345678]").Select(s => s[0]);

        static readonly Parser<ChessPiece?[]> Row =
            PieceOrDigit.Repeated().Select(row => row.SelectMany(FromFENChar).ToArray());

        static readonly Parser<ChessBoard> Board =
                Row.SeparatedBy(Parsers.Char('/')).Select(rows => new ChessBoard(rows.Reverse()));

        // https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation
        // Piece placement data:
        // Active color: "w" means that White is to move; "b" means that Black is to move.
        // Castling availability: If neither side has the ability to castle, this field uses the character "-". Otherwise, this field contains one or more letters: "K" if White can castle kingside, "Q" if White can castle queenside, "k" if Black can castle kingside, and "q" if Black can castle queenside. A situation that temporarily prevents castling does not prevent the use of this notation.
        // En passant target square: This is a square over which a pawn has just passed while moving two squares; it is given in algebraic notation. If there is no en passant target square, this field uses the character "-". This is recorded regardless of whether there is a pawn in position to capture en passant.[6] An updated version of the spec has since made it so the target square is recorded only if a legal en passant capture is possible, but the old version of the standard is the one most commonly used.[7][8]
        // Halfmove clock: The number of halfmoves since the last capture or pawn advance, used for the fifty-move rule.[9]
        // Fullmove number: The number of the full moves. It starts at 1 and is incremented after Black's move.

        static readonly Parser<CastlingAvailability> Castling =
                Parsers.Alternatives(
                    Parsers.Char('-').Select(_ => CastlingAvailability.None),
                    Parsers.Regex("[kqKQ]{1,4}").Select(CastlingAvailabilities.FromChars)
                );

        static readonly Parser<ChessPieceColor> Active =
                Parsers.Regex("[wb]").Select(s => s == "w" ? ChessPieceColor.White : s == "b" ? ChessPieceColor.Black : throw new InvalidDataException($"invalid active player {s}"));
        public static readonly Parser<char> Rank =
                   Parsers.Regex("[a-h]").Select(c => c[0]);

        public static readonly Parser<short> File =
            Parsers.Regex("[1-8]").Select(c => (short)(c[0] - '0'));

        static readonly Parser<Square?> EnPasant =
                Parsers.Alternatives(
                    Parsers.Char('-').Select(_ => default(Square?)),
                     Parsers.Tuple(Rank, File).Select(t => (Square?)new Square(t.Item1, t.Item2))
                );

        static readonly Parser<int> Number =
            Parsers.Regex("[wb]").TrySelect<int>(Int32.TryParse);

        static readonly Parser<ChessGameState> Game =
                   from board in Board.Token()
                   from active in Active.Token()
                   from avail in Castling.Token()
                   from epTarget in EnPasant // TODO
                   from halfMoveClock in Number.Token().Optional()
                   from fullMoveNumber in Number.Token().Optional()
                   select new ChessGameState(board, avail, active, halfMoveClock, fullMoveNumber);


        static IEnumerable<ChessPiece?> FromFENChar(char ch)
        {
            switch (ch)
            {
                case 'p':
                case 'r':
                case 'b':
                case 'n':
                case 'k':
                case 'q':
                case 'P':
                case 'R':
                case 'B':
                case 'N':
                case 'K':
                case 'Q':
                    yield return ChessPiece.FromChar(ch); break;
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                    var val = (int)(ch - '0');
                    for (int i = 0; i < val; i++) { yield return default; }
                    break;
            }
        }

        // record class BoardRow(ChessPiece[] Pieces)
        // {
        //     BoardRow(IEnumerable<ChessPiece> pieces) : this(pieces.ToArray()) { }

        // }
    }
}
