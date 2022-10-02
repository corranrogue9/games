using System;

namespace Fx.Games.Gobble
{

    public sealed class GobbleBoard
    {
        public GobbleBoard()
            : this(new Nullable<GobblePiece>[3, 3])
        {
        }

        public GobbleBoard(Nullable<GobblePiece>[,] board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            if (board.Rank != 2 && board.GetLength(0) != 3 && board.GetLength(1) != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(board), "A 3x3 board is required");
            }

            this.Grid = board.Clone() as Nullable<GobblePiece>[,]; //// TODO does clone work here?
        }

        public Nullable<GobblePiece>[,] Grid { get; }
    }
}
