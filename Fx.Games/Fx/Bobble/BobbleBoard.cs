using System;

namespace Fx.Games.Bobble
{

        public sealed class BobbleBoard
    {
        public BobbleBoard()
            : this(new Nullable<BobblePiece>[3, 3])
        {
        }

        public BobbleBoard(Nullable<BobblePiece>[,] board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            if (board.Rank != 2 && board.GetLength(0) != 3 && board.GetLength(1) != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(board), "A 3x3 board is required");
            }

            this.Grid = board.Clone() as Nullable<BobblePiece>[,]; //// TODO does clone work here?
        }

        public Nullable<BobblePiece>[,] Grid { get; }
    }
}
