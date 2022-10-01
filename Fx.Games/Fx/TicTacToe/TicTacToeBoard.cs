using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Games.TicTacToe
{
    public sealed class TicTacToeBoard
    {
        public TicTacToeBoard()
            : this(new TicTacToePiece[3, 3])
        {
        }

        public TicTacToeBoard(TicTacToePiece[,] board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            if (board.Rank != 2 && board.GetLength(0) != 3 && board.GetLength(1) != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(board), "A 3x3 board is required");
            }

            this.Grid = board.Clone() as TicTacToePiece[,]; //// TODO does clone work here?
        }

        public TicTacToePiece[,] Grid { get; }
    }
}
