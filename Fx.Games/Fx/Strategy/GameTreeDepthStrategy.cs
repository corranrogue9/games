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

        private readonly int depth;

        public GameTreeDepthStrategy(ITreeFactory treeFactory)
            : this(game => game.Moves.Count(), treeFactory)
        {
        }

        public GameTreeDepthStrategy(Func<TGame, double> selector, ITreeFactory treeFactory)
            : this(selector, treeFactory, 2)
        {
        }

        public GameTreeDepthStrategy(Func<TGame, double> selector, ITreeFactory treeFactory, int depth)
        {
            Ensure.NotNull(selector, nameof(selector));
            Ensure.NotNull(treeFactory, nameof(treeFactory));

            this.selector = selector;
            this.treeFactory = treeFactory;

            ////this.treeFactory = Node.TreeFactory;
            this.depth = depth;
        }

        public TMove SelectMove(TGame game)
        {
            return game.Moves.Maximum(move =>
            {
                var testGame = game.CommitMove(move);
                var score = testGame
                    .ToTree<TGame, TBoard, TMove, TPlayer>(this.depth)
                    .DepthTree(this.treeFactory)
                    .Select(tuple => (tuple.Depth, this.selector(tuple.Value)), this.treeFactory)
                    //// TODO max is defintely bad; chess, for example, will always try to trade queens because is sees that as the highest upside move; minimize oppoent's max, or average, or something else would be better
                    .Fold(tuple => tuple.Item2 / tuple.Depth, (game, scores) => scores.Max()); //// TODO is there a cheaper way to accommodate picking the shortest path for tied scores? i think that this algorithm might not always pick the highest score...
                return score;
            });
        }
    }
}
