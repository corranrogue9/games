using System.Diagnostics.CodeAnalysis;

namespace Fx.Game.Chess
{

    public sealed class ChessBoard
    {
        public ChessPiece?[,] Board { get; }

        public ChessBoard(ChessPiece?[,] board)
        {
            this.Board = board;
        }

        private static readonly string INITIAL =
            "RNBQKBNR" +
            "PPPPPPPP" +
            // "________" +
            "________" +
            "________" +
            "________" +
            "________" +
            // "________" +
            "pppppppp" +
            "rnbqkbnr";

        public ChessBoard()
        {
            this.Board = new ChessPiece?[8, 8];
            var pos = (0, 0);
            foreach (var piece in INITIAL)
            {
                this.Board[pos.Item1, pos.Item2] = ChessPiece.FromChar(piece);
                pos = pos.Item2 < 7 ? (pos.Item1, pos.Item2 + 1) : (pos.Item1 + 1, 0);
            }
        }

        public override string ToString()
        {
            TextWriter writer = new StringWriter();

            writer.WriteLine("\x1b[31;90m{0}\x1b[0m", "  a b c d e f g h");
            for (int i = 7; i > -1; i--)
            {
                writer.Write("\x1b[31;90m{0}\x1b[0m ", i + 1);
                for (var j = 0; j < 8; j++)
                {
                    writer.Write(Board[i, j]?.Symbol() ?? '_');
                    writer.Write(' ');
                }
                writer.WriteLine(" \x1b[31;90m{0}\x1b[0m", i + 1);
            }
            writer.WriteLine("\x1b[31;90m{0}\x1b[0m", "  a b c d e f g h");

            return writer.ToString()!;
        }

        public ChessPiece? this[Coordinate coordinate]
        {
            get { return this.Board[coordinate.y, coordinate.x]; }
        }

        public bool TryGetPiece(Coordinate coordinate, [MaybeNullWhen(false)] out ChessPiece piece)
        {
            var maybePiece = this[coordinate];
            if (maybePiece == null)
            {
                piece = default;
                return false;
            }
            else
            {
                piece = maybePiece.Value;
                return true;
            }
        }
    }
}
