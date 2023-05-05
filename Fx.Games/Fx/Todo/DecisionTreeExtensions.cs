﻿namespace Fx.Todo
{
    using Fx.Game;
    using Fx.Strategy;
    using Fx.Tree;
    using Fx.TreeFactory;
    using System.Linq;

    public static class DecisionTreeExtensions
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
    }
}
