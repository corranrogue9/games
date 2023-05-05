namespace Fx.Todo
{
    using Fx.Game;
    using Fx.Strategy;
    using Fx.Tree;
    using System.Linq;

    internal static class DecisionTreeExtensions
    {
        public static ITree<(IGame<TGame, TBoard, TMove, TPlayer>, TMove, (DecisionTreeStatus, double))> Decide<TGame, TBoard, TMove, TPlayer>(
            this ITree<IGame<TGame, TBoard, TMove, TPlayer>> gameTree, 
            TPlayer player, 
            IEqualityComparer<TPlayer> playerComparer) 
            where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return gameTree.Select(
                game => (game, default(TMove), game.Outcome?.Winners.Contains(player, playerComparer) == true ? (DecisionTreeStatus.Win, 1.0) : game.Outcome?.Winners.Any() == false ? (DecisionTreeStatus.Draw, 0.0) : (DecisionTreeStatus.Lose, -1.0)),
                (game, children) =>
                {
                    var zipped = game.Moves.Zip(children).ToList();
                    if (playerComparer.Equals(game.CurrentPlayer, player))
                    {
                        if (zipped.Where(child => child.Second.Item3.Item1 == DecisionTreeStatus.Win).TryFirst(out var first))
                        {
                            return (game, first.First, first.Second.Item3);
                        }

                        //// TODO you could choose to pick a draw here instaed of maximum for the "try not to lose" option
                        var choice = zipped.Maximum(child => child.Second.Item3.Item2);
                        return (game, choice.First, choice.Second.Item3);
                    }
                    else
                    {
                        if (zipped.All(child => child.Second.Item3.Item1 == DecisionTreeStatus.Win))
                        {
                            return (game, default(TMove), (DecisionTreeStatus.Win, 1.0));
                        }
                        else if (zipped.All(child => child.Second.Item3.Item1 == DecisionTreeStatus.Lose))
                        {
                            return (game, default(TMove), (DecisionTreeStatus.Lose, -1.0));
                        }
                        else if (zipped.All(child => child.Second.Item3.Item1 == DecisionTreeStatus.Draw))
                        {
                            return (game, default(TMove), (DecisionTreeStatus.Draw, 0.0));
                        }
                        else
                        {
                            return (game, default(TMove), (DecisionTreeStatus.Other, children.Average(_ => _.Item3.Item2)));
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
