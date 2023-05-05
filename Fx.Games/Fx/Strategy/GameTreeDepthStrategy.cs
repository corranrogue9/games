namespace Fx.Strategy
{
    using Fx.Game;
    using Fx.Tree;
    using Fx.TreeFactory;
    using Fx.Todo;
    using System.Linq;

    public sealed class GameTreeDepthStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Func<IGame<TGame, TBoard, TMove, TPlayer>, (IGame<TGame, TBoard, TMove, TPlayer>, TMove, (DecisionTreeStatus, double))> selector;

        private readonly ITreeFactory treeFactory;

        private readonly TPlayer player;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        public GameTreeDepthStrategy(Func<IGame<TGame, TBoard, TMove, TPlayer>, (IGame<TGame, TBoard, TMove, TPlayer>, TMove, (DecisionTreeStatus, double))> selector, ITreeFactory treeFactory, TPlayer player, IEqualityComparer<TPlayer> playerComparer)
        {
            /*Ensure.NotNull(selector, nameof(selector));
            Ensure.NotNull(treeFactory, nameof(treeFactory));*/
            Ensure.NotNull(player, nameof(player));
            Ensure.NotNull(playerComparer, nameof(playerComparer));

            /*this.selector = selector;
            this.treeFactory = treeFactory;*/
            this.player = player;
            this.playerComparer = playerComparer;

            this.selector = game => game.ToTree().Decide(this.player, this.playerComparer).Value;
            this.treeFactory = Node.TreeFactory;
        }

        public TMove SelectMove(TGame game)
        {
            var gameTree = game
                .ToTree(3);
            var selectedGameTree = gameTree
                .Select(this.selector, this.treeFactory); //// TODO the signature of the selector is wrong; the stragety should take a "score" and make the correct decision from there, the selector should just give us the score

            return selectedGameTree.Value.Item2;
        }
    }
}
