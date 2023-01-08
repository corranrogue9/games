namespace Fx.Game
{
    public sealed class PegBoard
    {
        public PegBoard(int height, IEnumerable<(int, int)> blanks)
        {
            Triangle = new Peg[height][];
            for (int i = 0; i < height; ++i)
            {
                Triangle[i] = new Peg[i + 1];
            }

            foreach (var blank in blanks)
            {
                Triangle[blank.Item1][blank.Item2] = Peg.Empty;
            }
        }

        public Peg[][] Triangle { get; }
    }
}
