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
            "________" +
            "________" +
            "________" +
            "________" +
            "pppppppp" +
            "rnbqkbnr";

        public ChessBoard()
        {
            this.Board = new ChessPiece?[8, 8];
            var pos = (0, 0);
            foreach (var piece in INITIAL)
            {
                this.Board[pos.Item1, pos.Item2] = ChessPiece.SquareFromChar(piece);
                pos = pos.Item2 < 7 ? (pos.Item1, pos.Item2 + 1) : (pos.Item1 + 1, 0);
            }
        }

        public override string ToString()
        {
            TextWriter writer = new StringWriter();

            writer.WriteLine("\x1b[31;90m{0}\x1b[0m", "  a b c d e f g h");
            // rank 8 on top (-> white at bottom)
            for (var rank = 7; rank > -1; rank--)
            {
                writer.Write("\x1b[31;90m{0}\x1b[0m ", rank + 1);
                for (var file = 0; file < 8; file++)
                {
                    writer.Write(Board[rank, file]?.Symbol() ?? '_');
                    writer.Write(' ');
                }
                writer.WriteLine(" \x1b[31;90m{0}\x1b[0m", rank + 1);
            }
            writer.WriteLine("\x1b[31;90m{0}\x1b[0m", "  a b c d e f g h");

            return writer.ToString()!;
        }

        public ChessPiece? this[Square coordinate]
        {
            get { return this.Board[coordinate.y, coordinate.x]; }
        }

        public bool TryGetPiece(Square coordinate, [MaybeNullWhen(false)] out ChessPiece piece)
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
