namespace Fx.Game
{
    public sealed class CheckerMove
    {
        public CheckerMove(Position initial, Position final)
        {
            this.Initial = initial;
            this.Final = final;
        }

        public Position Initial { get; }

        public Position Final { get; }
    }
}
