namespace Fx.Strategy
{
    using Fx.Game;
    using Fx.Tree;
    using Fx.Todo;
    using System.Linq;

    public sealed class MonteCarloStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly TPlayer player;

        private readonly double sampleRate;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        private readonly Random random;

        public MonteCarloStrategy(TPlayer player, double sampleRate, IEqualityComparer<TPlayer> playerComparer, Random random)
        {
            Ensure.NotNull(player, nameof(player));
            Ensure.NotNull(playerComparer, nameof(playerComparer));
            Ensure.NotNull(random, nameof(random));

            this.player = player;
            this.sampleRate = sampleRate;
            this.playerComparer = playerComparer;
            this.random = random;
        }

        public TMove SelectMove(TGame game)
        {
            var tree = game.ToTree();
            var branches = tree.EnumerateBranches();
            var prunedBranches = branches.Where(branch => this.random.NextDouble() < this.sampleRate);
            var prunedTree = DecisionTreeExtensions.CreateFromBranches(prunedBranches, Node.TreeFactory);

            var decisionTree = prunedTree.Decide(this.player, this.playerComparer);
            return decisionTree.Value.Item2;
        }

        private enum Status
        {
            Win,
            Lose,
            Draw,
            Other,
        }
    }
}
