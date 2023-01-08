namespace Fx.Strategy
{
    using Fx.Game;
    using Fx.Todo;

    public sealed class DecisionTreeStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly TPlayer player;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        public DecisionTreeStrategy(TPlayer player, IEqualityComparer<TPlayer> playerComparer)
        {
            Ensure.NotNull(player, nameof(player));
            Ensure.NotNull(playerComparer, nameof(playerComparer));

            this.player = player;
            this.playerComparer = playerComparer;
        }

        public TMove SelectMove(TGame game)
        {
            var toWin = game
                .ToTree()
                .Decide(this.player, this.playerComparer);

            return toWin.Value.Item2;
        }
    }
}
