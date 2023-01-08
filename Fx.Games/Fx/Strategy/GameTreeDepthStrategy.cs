namespace Fx.Strategy
{
    using Fx.Game;
    using Fx.Tree;
    using Fx.TreeFactory;
    using System.Linq;

    public sealed class GameTreeDepthStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Func<TGame, double> selector;

        private readonly ITreeFactory treeFactory;

        public GameTreeDepthStrategy(ITreeFactory treeFactory)
            : this(game => game.Moves.Count(), treeFactory)
        {
        }

        public GameTreeDepthStrategy(Func<TGame, double> selector, ITreeFactory treeFactory)
        {
            Ensure.NotNull(selector, nameof(selector));
            Ensure.NotNull(treeFactory, nameof(treeFactory));

            this.selector = selector;
            this.treeFactory = treeFactory;

            this.treeFactory = Node.TreeFactory;
        }

        public TMove SelectMove(TGame game)
        {
            return game.Moves.Maximum(move =>
            {
                var testGame = game.CommitMove(move);
                return testGame
                    .ToTree<TGame, TBoard, TMove, TPlayer>(7)
                    .Fold(this.selector, (game, scores) => scores.Max());
            });
        }
    }
}
