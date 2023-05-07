namespace Fx.Game
{
    public sealed class Position
    {
        public Position(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public int Row { get; }

        public int Column { get; }
    }
}
