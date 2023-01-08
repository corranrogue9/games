namespace Fx.Strategy
{
    using Fx.Game;

    public sealed class HeuristicStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Func<TGame, double> heuristic;

        public HeuristicStrategy()
            : this(game => game.Moves.Count())
        {
        }

        public HeuristicStrategy(Func<TGame, double> heuristic)
        {
            Ensure.NotNull(heuristic, nameof(heuristic));

            this.heuristic = heuristic;
        }

        public TMove SelectMove(TGame game)
        {
            Ensure.NotNull(game, nameof(game));

            return game.Moves.Maximum(move => this.heuristic(game.CommitMove(move)));
        }
    }
}
