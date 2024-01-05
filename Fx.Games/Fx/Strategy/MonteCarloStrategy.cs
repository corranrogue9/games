namespace Fx.Strategy
{
    using Fx.Game;
    using Fx.Tree;
    using Fx.Todo;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Principal;

    public sealed class MonteCarloStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly TPlayer player;

        private readonly int decisionCount;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        private readonly Random random;

        public MonteCarloStrategy(TPlayer player, int decisionCount, IEqualityComparer<TPlayer> playerComparer, Random random)
        {
            Ensure.NotNull(player, nameof(player));
            Ensure.NotNull(playerComparer, nameof(playerComparer));
            Ensure.NotNull(random, nameof(random));

            this.player = player;
            this.decisionCount = decisionCount;
            this.playerComparer = playerComparer;
            this.random = random;
        }

        public TMove SelectMove(TGame game)
        {
            var outcomes = new Dictionary<int, (int winCount, int sampledCount)>();
            var remainingDecisionsCount = this.decisionCount;
            var moves = game.Moves.ToList();
            while (remainingDecisionsCount > 0)
            {
                var sampledGame = SampleGame(game, moves);
                var win = sampledGame.win ? 1 : 0;
                if (outcomes.TryGetValue(sampledGame.moveIndex, out var counts))
                {
                    outcomes[sampledGame.moveIndex] = (counts.winCount + win, counts.sampledCount + 1);
                }
                else
                {
                    outcomes[sampledGame.moveIndex] = (win, 1);
                }

                remainingDecisionsCount -= sampledGame.numberOfDecisions;
            }

            var bestOutcome = outcomes.MaxBy(outcome => (double)outcome.Value.winCount / outcome.Value.sampledCount);
            return moves[bestOutcome.Key];
        }

        private (int moveIndex, bool win, int numberOfDecisions) SampleGame(TGame game, IReadOnlyList<TMove> moves)
        {
            var initialMoveIndex = random.Next(0, moves.Count);
            var initialMove = moves[initialMoveIndex];
            game = game.CommitMove(initialMove);
            var numberOfDecisions = 1;
            while (game.Outcome == null)
            {
                var nextMoves = game.Moves.ToList();
                var nextMoveIndex = random.Next(0, nextMoves.Count);
                var nextMove = nextMoves[nextMoveIndex];
                game = game.CommitMove(nextMove);
                ++numberOfDecisions;
            }

            return (initialMoveIndex, game.Outcome.Winners.Contains(this.player, this.playerComparer), numberOfDecisions);
        }
    }
}
