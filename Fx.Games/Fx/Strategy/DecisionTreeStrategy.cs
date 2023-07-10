namespace Fx.Strategy
{
    using Fx.Game;
    using Fx.Todo;
    using Fx.TreeFactory;

    public sealed class DecisionTreeStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly TPlayer player;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        private readonly ITreeFactory treeFactory;

        public DecisionTreeStrategy(TPlayer player, IEqualityComparer<TPlayer> playerComparer)
            : this(player, playerComparer, Fx.Tree.Node.TreeFactory)
        {
        }

        public DecisionTreeStrategy(TPlayer player, IEqualityComparer<TPlayer> playerComparer, ITreeFactory treeFactory)
        {
            Ensure.NotNull(player, nameof(player));
            Ensure.NotNull(playerComparer, nameof(playerComparer));

            this.player = player;
            this.playerComparer = playerComparer;
            this.treeFactory = treeFactory;
        }

        public TMove SelectMove(TGame game)
        {
            var toWin = game
                .ToTree(this.treeFactory)
                .Decide(this.player, this.playerComparer, this.treeFactory);

            return toWin.Value.Item2;
        }
    }
}
