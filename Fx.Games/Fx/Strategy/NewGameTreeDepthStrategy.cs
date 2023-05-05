namespace Fx.Strategy
{
    using Fx.Game;
    using Fx.Tree;
    using Fx.TreeFactory;
    using Fx.Todo;
    using System.Linq;

    public sealed class NewGameTreeDepthStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Func<TGame, double> selector;

        private readonly ITreeFactory treeFactory;

        private readonly TPlayer player;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        public NewGameTreeDepthStrategy(Func<TGame, double> selector, ITreeFactory treeFactory, TPlayer player, IEqualityComparer<TPlayer> playerComparer)
        {
            /*Ensure.NotNull(selector, nameof(selector));
            Ensure.NotNull(treeFactory, nameof(treeFactory));*/
            Ensure.NotNull(player, nameof(player));
            Ensure.NotNull(playerComparer, nameof(playerComparer));

            /*this.selector = selector;
            this.treeFactory = treeFactory;*/
            this.player = player;
            this.playerComparer = playerComparer;

            this.selector = selector;
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
