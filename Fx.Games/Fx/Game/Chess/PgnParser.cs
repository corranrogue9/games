namespace Fx.Games.Chess
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using Fx.Game.Chess;

    public class PgnParser
    {
        public static (ChessPiece?, bool, (int, int)) ParseMove(string input)
        {
            if (Move(input, out var rem, out var move))
            {
                System.Console.WriteLine("remainder: '{0}'", rem.ToString());
                return move;
            }
            throw new FormatException($"parse error at {input}");
        }

        private static readonly Parser<int> Index = Parsers.Terminated<int, string>(Parsers.Terminated<int, char>(Parsers.Number, Parsers.Char('.')), Parsers.Whitespace);

        public static readonly Parser<int> Rank = Parsers.Regex("[a-h]").Select<string, int>(c => (int)(c[0] - 'a'));

        public static readonly Parser<int> File = Parsers.Regex("[1-8]").Select<string, int>(c => (int)(c[0] - '0'));

        public static readonly Parser<(int, int)> Coord = Parsers.Tuple(Rank, File);

        public static readonly Parser<ChessPiece> Piece = Parsers.Regex("[kqnbrpKQNBRP]").Select<string, ChessPiece>(s => ChessPiece.FromChar(s[0]));

        public static readonly Parser<bool> Capture = Parsers.Char('x').Optional().Select(x => x != null);

        private static readonly Parser<(ChessPiece?, bool, (int, int))> Move =
            from p in Piece.Optional()
            from x in Capture
            from c in Coord
            select (p, x, c);

        // internal static Parser<IReadOnlyList<(ChessPiece?, bool, (int, int))>> Moves = Move.Many();
    }

}