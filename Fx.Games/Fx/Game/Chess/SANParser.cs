namespace Fx.Games.Chess
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using Fx.Game;
    using Fx.Game.Chess;


    public enum SANPiece { King, Queen, Rook, Bishop, Knight, Pawn }

    public record struct SANMove(SANPiece Piece, (int, int)? Start, bool Take, (int, int) Coord, bool Check)
    { }


    // https://en.wikipedia.org/wiki/Algebraic_notation_(chess)
    public class SANParser
    {
        public static SANMove ParseMove(string input)
        {
            if (SANParser.Move(input, out var rem, out var move))
            {
                return move;
            }
            throw new FormatException($"parse error at {input}");
        }

        // private static readonly Parser<int> Index = Parsers.Terminated<int, string>(Parsers.Terminated<int, char>(Parsers.Number, Parsers.Char('.')), Parsers.Whitespace);

        public static readonly Parser<int> Rank = Parsers.Regex("[a-h]").Select<string, int>(c => (int)(c[0] - 'a'));

        public static readonly Parser<int> File = Parsers.Regex("[1-8]").Select<string, int>(c => (int)(c[0] - '0'));

        public static readonly Parser<(int, int)> Coord = Parsers.Alternatives(
            Parsers.Tuple(Rank, File),
            Rank.Select(r => (r, -1)),
            File.Select(f => (-1, f))
        );

        private static SANPiece PieceFromLetter(string str)
        {
            return str switch
            {
                "K" => SANPiece.King,
                "Q" => SANPiece.Queen,
                "N" => SANPiece.Knight,
                "B" => SANPiece.Bishop,
                "R" => SANPiece.Rook,
                "" => SANPiece.Pawn,
                _ => throw new InvalidDataException("not a leagan SAN piece name/char")
            };
        }

        public static readonly Parser<SANPiece> Piece =
            Parsers
                .Regex("[KQNBR]?")
                .Select(s => PieceFromLetter(s));

        public static readonly Parser<bool> MaybeCapture = Parsers.Char('x').Optional().Select(x => x != null);
        public static readonly Parser<bool> MaybeCheck = Parsers.Char('+').Optional().Select(x => x != null);

        private static readonly Parser<SANMove> MoveWithoutStart = (
            from p in Piece
            from s in Coord
            from x in MaybeCapture
            from c in Coord
            from k in MaybeCheck
            select new SANMove(p, s, x, c, k)
        ).EOI();

        private static readonly Parser<SANMove> MoveWithStart = (
            from p in Piece
            from x in MaybeCapture
            from c in Coord
            from k in MaybeCheck
            select new SANMove(p, null, x, c, k)
        ).EOI();

        static readonly Parser<SANMove> Move = Parsers.Alternatives(
            MoveWithoutStart,
            MoveWithStart,
            Parsers.String("O-O").Select(_ => new SANMove(SANPiece.Rook, (0, 7), false, (0, 5), false)),
            Parsers.String("O-O-O").Select(_ => new SANMove(SANPiece.Rook, (0, 0), false, (0, 3), false))
        );

        // internal static Parser<IReadOnlyList<(ChessPiece?, bool, (int, int))>> Moves = Move.Many();
    }

}