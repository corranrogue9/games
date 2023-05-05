namespace Fx.Strategy
{
    using Fx.Game;
    using Fx.Tree;
    using Fx.Todo;
    using System.Linq;

    public sealed class GarrettGameTreeDepthStrategy<TGame, TMove, TPlayer> : IStrategy<TMove, TPlayer, TGame> where TGame : Fx.Game.IGame<TMove, TPlayer, TGame>
    {
        private readonly Func<TGame, double> selector;

        private readonly ITreeFactory treeFactory;

        private readonly TPlayer player;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        public GarrettGameTreeDepthStrategy(Func<TGame, double> selector, ITreeFactory treeFactory, TPlayer player, IEqualityComparer<TPlayer> playerComparer)
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
            return game.LegalMoves.Maximum(move =>
            {
                var testGame = game.CommitMove(move);
                return testGame
                    .ToOtherTree<TGame, TMove, TPlayer>(7)
                    .Fold(this.selector, (game, scores) => scores.Max());
            });
        }

        public TGame SelectMove(TPlayer game)
        {
            throw new NotImplementedException();
        }
    }
}
