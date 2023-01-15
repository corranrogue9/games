namespace Fx.Games
{
    using Fx.Games.TicTacToe;
    using Fx.Tree;
    using System.Linq;

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
                .Select(
                    game => (game, default(TMove), game.Outcome?.Winners.Contains(this.player, this.playerComparer) == true ? (Status.Win, 1.0) : game.Outcome?.Winners.Any() == false ? (Status.Draw, 0.0) : (Status.Lose, -1.0)),
                    (game, children) =>
                    {
                        var zipped = game.Moves.Zip(children).ToList();
                        if (this.playerComparer.Equals(game.CurrentPlayer, this.player))
                        {
                            if (zipped.Where(child => child.Second.Item3.Item1 == Status.Win).TryFirst(out var first))
                            {
                                return (game, first.First, first.Second.Item3);
                            }

                            //// TODO you could choose to pick a draw here instaed of maximum for the "try not to lose" option
                            var choice = zipped.Maximum(child => child.Second.Item3.Item2);
                            return (game, choice.First, choice.Second.Item3);
                        }
                        else
                        {
                            if (zipped.All(child => child.Second.Item3.Item1 == Status.Win))
                            {
                                return (game, default(TMove), (Status.Win, 1.0));
                            }
                            else if (zipped.All(child => child.Second.Item3.Item1 == Status.Lose))
                            {
                                return (game, default(TMove), (Status.Lose, -1.0));
                            }
                            else if (zipped.All(child => child.Second.Item3.Item1 == Status.Draw))
                            {
                                return (game, default(TMove), (Status.Draw, 0.0));
                            }
                            else
                            {
                                return (game, default(TMove), (Status.Other, children.Average(_ => _.Item3.Item2)));
                            }
                        }
                    },
                    Node.TreeFactory);

            return toWin.Value.Item2;
        }

        private enum Status
        {
            Win,
            Lose,
            Draw,
            Other,
        }
    }

    internal static class DecisionTreeExtensions
    {
        public static bool TryFirst<T>(this IEnumerable<T> source, out T value)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    value = enumerator.Current;
                    return true;
                }

                value = default;
                return false;
            }
        }

        internal static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            if (game.Outcome == null)
            {
                return Node.CreateTree(game, game.Moves.Select(move => game.CommitMove(move).ToTree()));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }
    }
}
