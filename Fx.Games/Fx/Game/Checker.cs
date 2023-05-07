namespace Fx.Game
{
    public sealed class Checker
    {
        private Checker(bool black, bool queen)
        {
            this.IsBlack = black;
            this.IsQueen = queen;
        }

        public static Checker BlackQueen { get; } = new Checker(true, true);

        public static Checker Black { get; } = new Checker(true, false);

        public static Checker WhiteQueen { get; } = new Checker(false, true);

        public static Checker White { get; } = new Checker(false, false);

        public bool IsBlack { get; }

        public bool IsQueen { get; }
    }
}
