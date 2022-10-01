namespace Fx.Games.Bobble
{
    using System;

    public sealed class BobbleMove
    {
        public BobbleMove(uint row, uint column, BobbleSize size)
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

        public BobbleSize Size { get; }
    }
}
