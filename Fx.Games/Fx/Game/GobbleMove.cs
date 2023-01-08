namespace Fx.Game
{
    using System;

    public sealed class GobbleMove
    {
        public GobbleMove(uint row, uint column, GobbleSize size)
        {
            if (row > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "must be a value in [0-2]");
            }

            if (column > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "must be a value in [0-2]");
            }

            // TODO Ensure.EnumExists(size)

            this.Row = row;
            this.Column = column;
            this.Size = size;
        }

        public uint Row { get; }

        public uint Column { get; }

        public GobbleSize Size { get; }
    }
}
