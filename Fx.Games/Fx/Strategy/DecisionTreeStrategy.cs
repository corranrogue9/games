namespace Fx.Strategy
{
    using ConsoleApplication4;
    using Fx.Game;
    using Fx.Strategy;
    using Fx.Tree;
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

    public sealed class GameTreeDepthStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Func<IGame<TGame, TBoard, TMove, TPlayer>, (IGame<TGame, TBoard, TMove, TPlayer>, TMove, (Status, double))> selector;

        private readonly ITreeFactory treeFactory;

        private readonly TPlayer player;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        public GameTreeDepthStrategy(Func<IGame<TGame, TBoard, TMove, TPlayer>, (IGame<TGame, TBoard, TMove, TPlayer>, TMove, (Status, double))> selector, ITreeFactory treeFactory, TPlayer player, IEqualityComparer<TPlayer> playerComparer)
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

    public enum Status
    {
        Win,
        Lose,
        Draw,
        Other,
    }

    internal static class DecisionTreeExtensions
    {
        public static ITree<(IGame<TGame, TBoard, TMove, TPlayer>, TMove, (Status, double))> Decide<TGame, TBoard, TMove, TPlayer>(
            this ITree<IGame<TGame, TBoard, TMove, TPlayer>> gameTree, 
            TPlayer player, 
            IEqualityComparer<TPlayer> playerComparer) 
            where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return gameTree.Select(
                game => (game, default(TMove), game.Outcome?.Winners.Contains(player, playerComparer) == true ? (Status.Win, 1.0) : game.Outcome?.Winners.Any() == false ? (Status.Draw, 0.0) : (Status.Lose, -1.0)),
                (game, children) =>
                {
                    var zipped = game.Moves.Zip(children).ToList();
                    if (playerComparer.Equals(game.CurrentPlayer, player))
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
        }

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

        internal static ITree<TGame> ToOtherTree<TGame, TMove, TPlayer>(this TGame game, int depth) where TGame : Fx.Game.IGame<TMove, TPlayer, TGame>
        {
            if (game.Outcome == null && depth != 0)
            {
                return Node.CreateTree(game, game.LegalMoves.Select(move => game.CommitMove(move).ToOtherTree<TGame, TMove, TPlayer>(depth - 1)));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }

        internal static ITree<Fx.Game.IGame<TMove, TPlayer, TGame>> ToOtherTree<TGame, TMove, TPlayer>(this Fx.Game.IGame<TMove, TPlayer, TGame> game) where TGame : Fx.Game.IGame<TMove, TPlayer, TGame>
        {
            if (game.Outcome == null)
            {
                return Node.CreateTree(game, game.LegalMoves.Select(move => game.CommitMove(move).ToOtherTree()));
            }
            else
            {
                return Node.CreateTree(game);
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

        internal static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game, int depth) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            if (game.Outcome == null && depth != 0)
            {
                return Node.CreateTree(game, game.Moves.Select(move => game.CommitMove(move).ToTree(depth - 1)));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }

        public static IEnumerable<IEnumerable<T>> EnumerateBranches<T>(this ITree<T> tree)
        {
            return tree.Fold(
                (value) => new[] { new[] { value }.AsEnumerable() }.AsEnumerable(),
                (value, aggregation) => aggregation.SelectMany(child => child.Select(branch => branch.Prepend(value))));
        }

        public static ITree<T> CreateFromBranches<T>(IEnumerable<IEnumerable<T>> branches, ITreeFactory treeFactory)
        {
            if (!branches.Any())
            {
                throw new ArgumentException("TODO");
            }

            var branch = branches.First();
            if (!branch.Any())
            {
                throw new ArgumentException("TODO");
            }

            var value = branch.First();


            var subbranches = branches.Select(b => b.Skip(1)).Where(b => b.Any()).GroupBy(b => b.First());

            return treeFactory.CreateInner(value, subbranches.Select(b => CreateFromBranches(b, treeFactory)));
        }
    }
}
