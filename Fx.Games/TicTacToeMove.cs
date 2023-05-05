namespace Fx.Game.TicTacToe
{
    using System;

    public sealed class TicTacToeMove
    {
        public TicTacToeMove(uint row, uint column)
        {
            if (row > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "must be a value in [0-2]");
            }

            if (column > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "must be a value in [0-2]");
            }

            this.Row = row;
            this.Column = column;
        }

        public uint Row { get; }

        public uint Column { get; }
    }
}
