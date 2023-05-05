namespace Fx.Game
{
    public sealed class PegMove
    {
        public PegMove((int, int) start, (int, int) end)
        {
            Start = start;
            End = end;
        }

        public (int, int) Start { get; }

        public (int, int) End { get; }
    }
}
