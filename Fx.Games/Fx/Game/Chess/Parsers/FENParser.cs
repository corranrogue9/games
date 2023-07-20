using Fx.Game.Chess;

namespace Fx.Game.Chess
{
    // https://www.wikiwand.com/en/Forsyth%E2%80%93Edwards_Notation
    public class FENParser
    {
        public static ChessBoard ParseBoard(string input)
        {
            if (FENParser.Board(input, out var rem, out var move))
            {
                return move;
            }
            throw new FormatException($"parse error at {input}");
        }

        static readonly Parser<char> PieceOrDigit =
             Parsers.Regex("[prbnkqPRBNKQ12345678]").Select(s => s[0]);

        static readonly Parser<ChessPiece?[]> Row =
            PieceOrDigit.Repeated().Select(row => row.SelectMany(FromFENChar).ToArray());

        static readonly Parser<ChessBoard> Board =
                Row.SeparatedBy(Parsers.Char('/')).Select(rows => new ChessBoard(rows.Reverse()));

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
