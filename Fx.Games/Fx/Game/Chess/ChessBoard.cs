using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Fx.Game.Chess
{

    public sealed class ChessBoard
    {
        public ChessPiece?[,] Board { get; }

        public ChessBoard(ChessPiece?[,] board)
        {
            this.Board = board;
        }

        public ChessBoard(IEnumerable<IEnumerable<ChessPiece?>> pieces)
        {
            this.Board = new ChessPiece?[8, 8];
            var i = 0;
            foreach (var row in pieces)
            {
                var j = 0;
                foreach (var piece in row)
                {
                    this.Board[i, j] = piece;
                    j += 1;
                }
                i += 1;
            }
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

            writer.WriteLine("\x1b[90m{0}\x1b[0m", "  a b c d e f g h");
            // rank 8 on top (-> white at bottom)
            for (var rank = 7; rank > -1; rank--)
            {
                writer.Write("\x1b[90m{0}\x1b[0m ", rank + 1);
                for (var file = 0; file < 8; file++)
                {
                    var piece = Board[rank, file];
                    // no color
                    // writer.Write(Board[rank, file]?.Symbol() ?? '_');
                    // with contrasting colors
                    var color = piece == null ? 37 : piece?.Color == ChessPieceColor.White ? 33 : 31;
                    writer.Write("\x1b[{0}m{1}\x1b[0m", color, Board[rank, file]?.Symbol() ?? '_');
                    writer.Write(' ');
                }
                writer.WriteLine(" \x1b[90m{0}\x1b[0m", rank + 1);
            }
            writer.WriteLine("\x1b[90m{0}\x1b[0m", "  a b c d e f g h");

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
