namespace Fx.Game.Chess
{



    // https://en.wikipedia.org/wiki/Algebraic_notation_(chess)
    public class SANParser
    {
        public static SANMove ParseHalfMove(string input)
        {
            if (SANParser.HalfMove(input, out var rem, out var move))
            {
                return move;
            }
            throw new FormatException($"parse error at {input}");
        }

        /// <summary>
        /// parse the list of SAN (standard algebraic notation) moves
        /// e.g.: "1. e4 e6 2. d4 b6"
        /// </summary>
        /// <returns>a list of SAN moves together with the half move number
        /// </returns>
        /// <param name="input"></param>
        public static IEnumerable<(int, SANMove)> ParseMoves(string input)
        {
            if (SANParser.Moves(input, out var rem, out var moves))
            {
                return moves.SelectMany(ToHalfMoves);
            }
            throw new FormatException($"parse error at {input}");

            static IEnumerable<(int, SANMove)> ToHalfMoves((int, SANMove, SANMove) round)
            {
                yield return (round.Item1 * 2 - 1, round.Item2);
                yield return (round.Item1 * 2, round.Item3);
            }
        }


        private static readonly Parser<int> Index =
            Parsers.Terminated(Parsers.Number, Parsers.Char('.'));

        public static readonly Parser<char> Rank =
            Parsers.Regex("[a-h]").Select(c => c[0]);

        public static readonly Parser<short> File =
            Parsers.Regex("[1-8]").Select(c => (short)(c[0] - '0'));

        public static readonly Parser<(char, short)> Coord = Parsers.Alternatives<(char, short)>(
            Parsers.Tuple(Rank, File),
            File.Select(f => ((char)0, f)),
            Rank.Select(r => (r, (short)0))
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

        private static readonly Parser<SANMove> MoveWithStart =
            from p in Piece
            from s in Coord
            from x in MaybeCapture
            from c in Coord
            from k in MaybeCheck
            select new SANMove(p, s, x, c, k);


        private static readonly Parser<SANMove> MoveWithoutStart =
            from p in Piece
            from x in MaybeCapture
            from c in Coord
            from k in MaybeCheck
            select new SANMove(p, null, x, c, k);

        static readonly Parser<SANMove> HalfMove = Parsers.Alternatives(
            // with start needs to come first to greedely parse things like "be1"
            MoveWithStart,
            MoveWithoutStart,
            Parsers.String("O-O").Select(_ => SANMove.KingSideCastling),
            Parsers.String("O-O-O").Select(_ => SANMove.KingSideCastling)
        );

        public static readonly Parser<(int, SANMove, SANMove)> FullMove = Parsers.Tuple(
           Index.Token(),
           HalfMove.Token(),
           HalfMove.Token()
       );

        public static readonly Parser<IReadOnlyList<(int, SANMove, SANMove)>> Moves = FullMove.Many();
    }
}