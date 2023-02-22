﻿namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Games;
    using Fx.Games.Gobble;
    using Fx.Games.PegGame;
    using Fx.Games.TicTacToe;
    using Fx.Tree;

    internal static class Extension
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

        public static void Enumerate<T>(this IEnumerable<T> source)
        {
            foreach (var element in source)
            {
            }
        }

        public static T[][] ToArray<T>(this T[,] source)
        {
            var result = new T[source.GetLength(0)][];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = new T[source.GetLength(1)];
                for (int j = 0; j < result[i].Length; ++j)
                {
                    result[i][j] = source[i, j];
                }
            }

            return result;
        }

        public static Func<Void> ToFunc(Action action)
        {
            return () =>
            {
                action();
                return new Void();
            };
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

        internal static T Choose<T>(this IEnumerable<T> source, Func<T, bool> preference, Func<T, bool> fallback)
        {
            return source
                .Aggregate(
                    (0, default(T)),
                    (aggregation, current) => aggregation.Item1 == 2 ? aggregation : preference(current) ? (2, current) : aggregation.Item1 == 1 ? aggregation : fallback(current) ? (1, current) : (0, current))
                .Item2;
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

    public struct Void
    {
    }

    class Program
    {

        static void Main(string[] args)
        {

            var tree = Node.CreateTree("Asdf", "qwer", "1234", "zxcv");

            //// TODO this should be the same as tree3
            //// var tree2 = Node.CreateBinaryTree("Asdf", "qwer", "1234", "zxcv");
            var tree3 = Node.CreateTree("Asdf", Node.CreateTree("qwer1", "zxcv12"), Node.CreateTree("1234567"));
            var lengths = tree3.Select(value => value.Length, Node.TreeFactory);



            var branches = tree3.EnumerateBranches();
            var recreatedTree = Extension.CreateFromBranches(branches, Node.TreeFactory);


            Pegs();
            TicTacToeHumanVsMaximizeMoves();
            Gobble();

            int sku = GetSkuFromArgsOrConsole(args);
            switch (sku)
            {
                case 0: // two player console tic tac toe
                    TwoPlayerConsoleTicTacToe();
                    break;
                case 1:
                    TicTacToeHumanVsMaximizeMoves();
                    break;
                case 2: // tic tac toe with two computer players using random strategy
                    TicTacToeTwoRandom();
                    break;
                default:
                    throw new Exception("bad sku given");
            }

            Console.WriteLine("hit enter to exit");
            Console.ReadLine();
        }

        private sealed class TestStrategy<TGame, TBoard> : IStrategy<TGame, TicTacToeBoard, TicTacToeMove, string> where TGame : IGame<TGame, TicTacToeBoard, TicTacToeMove, string>
        {
            private enum Status
            {
                Win,
                Lose,
                Draw,
                Other,
            }

            public TicTacToeMove SelectMove(TGame game)
            {
                var toWin = game
                    .ToTree()
                    .Select(
                       game => (game, default(TicTacToeMove), game.Outcome?.Winners.Contains("max") == true ? (Status.Win, 1.0) : game.Outcome?.Winners.Any() == false ? (Status.Draw, 0.0) : (Status.Lose, -1.0)),
                       (game, children) =>
                       {
                           var zipped = game.Moves.Zip(children).ToList();
                           if (game.CurrentPlayer == "max")
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
                                   return (game, default(TicTacToeMove), (Status.Win, 1.0));
                               }
                               else if (zipped.All(child => child.Second.Item3.Item1 == Status.Lose))
                               {
                                   return (game, default(TicTacToeMove), (Status.Lose, -1.0));
                               }
                               else if (zipped.All(child => child.Second.Item3.Item1 == Status.Draw))
                               {
                                   return (game, default(TicTacToeMove), (Status.Draw, 0.0));
                               }
                               else
                               {
                                   return (game, default(TicTacToeMove), (Status.Other, children.Average(_ => _.Item3.Item2)));
                               }
                           }
                       },
                       Node.TreeFactory);
                if (!System.IO.File.Exists(@"c:\users\gdebruin\desktop\tictactoe.txt"))
                {
                    Func<(IGame<TGame, TicTacToeBoard, TicTacToeMove, string>, TicTacToeMove?, (Status, double)), string> toString = tuple =>
                    {
                        var stringBuilder = new System.Text.StringBuilder();
                        stringBuilder.Append($"{tuple.Item3.Item2}: ");
                        if (tuple.Item2 != null)
                        {
                            stringBuilder.Append($"({tuple.Item2.Row}, {tuple.Item2.Column}) ");
                        }

                        var array = tuple.Item1.Board.Grid.ToArray();
                        stringBuilder.Append(string.Join("\\", array.Select(row => string.Join('|', row.Select(element => element == TicTacToePiece.Ex ? "X" : element == TicTacToePiece.Oh ? "O" : ".")))));

                        return stringBuilder.ToString();
                    };
                    var toDisplay = toWin
                        .Select(
                            toString,
                            Node.TreeFactory);

                    var formatted = toDisplay.DepthTree(Node.TreeFactory).Merge(toDisplay, (depth, winRate) => $"{new string(' ', depth * 2)}{winRate}", Node.TreeFactory);

                    System.IO.TextWriter writer;
                    ////writer = Console.Out;
                    writer = new System.IO.StreamWriter(@"c:\users\gdebruin\desktop\tictactoe.txt");
                    formatted.Fold(
                        (value) =>
                        {
                            writer.WriteLine(value);
                            return new Void();
                        },
                        (value, children) =>
                        {
                            writer.WriteLine(value);
                            children.Enumerate();
                            return new Void();
                        });
                    writer.Dispose();
                }

                return toWin.Value.Item2;

                /*return game
                    .ToTree()
                    .Fold(
                        game => ((TicTacToeMove?)null, game.Outcome?.Winners.Contains("max") == true ? (true, 1.0) : (false, 0.0)),
                        (game, children) =>
                        {
                            var zipped = game.Moves.Zip(children).ToList();
                            foreach (var child in zipped)
                            {
                                if (child.Second.Item2.Item1)
                                {
                                    return (child.First, (true, 1.0));
                                }
                            }

                            if (game.CurrentPlayer == "max")
                            {
                                var max = zipped.Maximum(child => child.Second.Item2.Item2);
                                return (max.First, max.Second.Item2);
                            }
                            else
                            {
                                return ((TicTacToeMove?)null, (false, children.Average(_ => _.Item2.Item2)));
                            }
                        })
                    .Item1;*/

                /*var count = game.ToTree().LeafCount();

                var winRates = game
                    .ToTree()
                    .Select(
                       game => (game, default(TicTacToeMove), game.Outcome?.Winners.Contains("max") == true ? (true, 1.0) : (false, 0.0)),
                       (game, children) =>
                       {
                           var zipped = game.Moves.Zip(children).ToList();
                           foreach (var child in zipped)
                           {
                               if (child.Second.Item3.Item1)
                               {
                                   return (game, child.First, child.Second.Item3);
                               }
                           }

                           if (game.CurrentPlayer == "max")
                           {
                               var choice = zipped.Maximum(child => child.Second.Item3.Item2);
                               return (game, choice.First, choice.Second.Item3);
                           }
                           else
                           {
                               return (game, default(TicTacToeMove), (false, children.Average(_ => _.Item3.Item2)));
                           }
                       },
                       Node.TreeFactory);

                Func<(IGame<TGame, TicTacToeBoard, TicTacToeMove, string>, TicTacToeMove?, (bool, double)), string> toString = tuple =>
                {
                    var stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.Append($"{tuple.Item3.Item2}: ");
                    if (tuple.Item2 != null)
                    {
                        stringBuilder.Append($"({tuple.Item2.Row}, {tuple.Item2.Column}) ");
                    }

                    var array = tuple.Item1.Board.Grid.ToArray();
                    stringBuilder.Append(string.Join("\\", array.Select(row => string.Join('|', row))));

                    return stringBuilder.ToString();
                };
                var toDisplay = winRates
                    .Select(
                        toString,
                        Node.TreeFactory);

                var formatted = toDisplay.DepthTree(Node.TreeFactory).Merge(toDisplay, (depth, winRate) => $"{new string(' ', depth * 2)}{winRate}", Node.TreeFactory);

                System.IO.TextWriter writer;
                ////writer = Console.Out;
                writer = new System.IO.StreamWriter(@"c:\users\gdebruin\desktop\tictactoe.txt");
                formatted.Fold(
                    (value) =>
                    {
                        writer.WriteLine(value);
                        return new Void();
                    },
                    (value, children) =>
                    {
                        writer.WriteLine(value);
                        children.Enumerate();
                        return new Void();
                    });*/

                /*var winRates = game
                    .ToTree()
                    .Fold(
                        game => (game, default(TicTacToeMove), game.Outcome?.Winners.Contains("max") == true ? (true, 1.0) : (false, 0.0)),
                        (game, children) =>
                        {
                            var zipped = game.Moves.Zip(children).ToList();
                            foreach (var child in zipped)
                            {
                                if (child.Second.Item3.Item1)
                                {
                                    return (game, child.First, child.Second.Item3);
                                }
                            }

                            if (game.CurrentPlayer == "max")
                            {
                                var choice = zipped.Maximum(child => child.Second.Item3.Item2);
                                return (game, choice.First, choice.Second.Item3);
                            }
                            else
                            {
                                return (game, default(TicTacToeMove), (false, children.Average(_ => _.Item3.Item2)));
                            }
                        });*/





                return default(TicTacToeMove);
            }
        }

        private static void Pegs()
        {
            var displayer = new PegGameConsoleDisplayer<string>();
            var computer = "max";
            var game = new PegGame<string>(computer);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<PegGame<string>, PegBoard, PegMove, string>>
                {
                    { computer, new GameTreeDepthStrategy<PegGame<string>, PegBoard, PegMove, string>(null, null, computer, StringComparer.OrdinalIgnoreCase) },
                    ////{ computer, new UserInterfaceStrategy<PegGame<string>, PegBoard, PegMove, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void Gobble()
        {
            var displayer = new GobbleConsoleDisplayer<string>(_ => _);
            var computer = "max";
            var human = "human";
            var game = new Gobble<string>(computer, human);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>>
                {
                            ////{ computer, MaximizeMovesStrategy.Default<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>() },
                            { computer, new DecisionTreeStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(computer, StringComparer.OrdinalIgnoreCase) },
                            ////{ computer, new MonteCarloStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(computer, 0.1, StringComparer.OrdinalIgnoreCase, new Random(0)) },
                            ////{ human, new UserInterfaceStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(displayer) },
                    {human, new RandomStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>() }
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeHumanVsMaximizeMoves()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var computer = "max";
            var human = "human";
            var game = new TicTacToe<string>(computer, human);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                            ////{ computer, MaximizeMovesStrategy.Default<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>() },
                            { computer, new DecisionTreeStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(computer, StringComparer.OrdinalIgnoreCase) },
                            ////{ computer, new MonteCarloStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(computer, 0.1, StringComparer.OrdinalIgnoreCase, new Random(0)) },
                            ////{ human, new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer) },
                            {human, new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>() }
                },
                displayer);

            //// TODO uncomment this to write all of the games with their moves and winrates to a file
            /*var winRates = game
                .ToTree()
                .Select(
                   game => (game, default(TicTacToeMove), game.Outcome?.Winners.Contains("max") == true ? (true, 1.0) : (false, 0.0)),
                   (game, children) =>
                   {
                       var zipped = game.Moves.Zip(children).ToList();
                       if (game.CurrentPlayer == "max")
                       {
                           if (zipped.Where(child => child.Second.Item3.Item1).TryFirst(out var first))
                           {
                               return (game, first.First, first.Second.Item3);
                           }

                           var choice = zipped.Maximum(child => child.Second.Item3.Item2);
                           return (game, choice.First, choice.Second.Item3);
                       }
                       else
                       {
                           if (zipped.All(child => child.Second.Item3.Item1))
                           {
                               return (game, default(TicTacToeMove), (true, 1.0));
                           }
                           else
                           {
                               return (game, default(TicTacToeMove), (false, children.Average(_ => _.Item3.Item2)));
                           }
                       }
                   },
                   Node.TreeFactory);

            Func<(IGame<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>, TicTacToeMove?, (bool, double)), string> toString = tuple =>
            {
                var stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append($"{tuple.Item3.Item2}: ");
                if (tuple.Item2 != null)
                {
                    stringBuilder.Append($"({tuple.Item2.Row}, {tuple.Item2.Column}) ");
                }

                var array = tuple.Item1.Board.Grid.ToArray();
                stringBuilder.Append(string.Join("\\", array.Select(row => string.Join('|', row.Select(element => element == TicTacToePiece.Ex ? "X" : element == TicTacToePiece.Oh ? "O" : ".")))));

                return stringBuilder.ToString();
            };
            var toDisplay = winRates
                .Select(
                    toString,
                    Node.TreeFactory);

            var formatted = toDisplay.DepthTree(Node.TreeFactory).Merge(toDisplay, (depth, winRate) => $"{new string(' ', depth * 2)}{winRate}", Node.TreeFactory);

            System.IO.TextWriter writer;
            ////writer = Console.Out;
            writer = new System.IO.StreamWriter(@"c:\users\gdebruin\desktop\tictactoe.txt");
            formatted.Fold(
                (value) =>
                {
                    writer.WriteLine(value);
                    return new Void();
                },
                (value, children) =>
                {
                    writer.WriteLine(value);
                    children.Enumerate();
                    return new Void();
                });*/




            /*//// TODO is this lazily evaluated?
            var gameTree = game.ToTree();
            var computerWinRate = gameTree.Fold(game => game.Outcome?.Winners.Contains("max") == true ? 1.0 : 0.0, (game, children) => children.Average());
            var humanWinRate = gameTree.Fold(game => game.Outcome?.Winners.Contains("human") == true ? 1.0 : 0.0, (game, children) => children.Average());
            var drawRate = gameTree.Fold(game => game.Outcome?.Winners.Any() == false ? 1.0 : 0.0, (game, children) => children.Average());

            //// TODO is this lazily evaluated?
            var nextMove = gameTree.Fold(
                game => ((TicTacToeMove?)null, game.Outcome?.Winners.Contains("max") == true ? 2 : game.Outcome?.Winners.Any() == false ? 1 : 0),
                (game, children) =>
                {
                    var chosen = game.Moves.Zip(children).Choose(element => element.Second.Item2 == 2, element => element.Second.Item2 == 1);
                    return (chosen.First, chosen.Second.Item2);
                });*/

            var result = driver.Run(game);
        }

        private static void TwoPlayerConsoleTicTacToe()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var consoleStrategy = new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer);
            var chrispre = "chrispre";
            var gdebruin = "gdebruin";
            var game = new TicTacToe<string>(chrispre, gdebruin);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { chrispre, consoleStrategy },
                    { gdebruin, consoleStrategy },
                },
                displayer);
            var result = driver.Run(game);
        }


        private static void TicTacToeTwoRandom()
        {

            const string playerX = "player X";
            const string playerO = "player O";
            var strategyX = new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>();
            var strategyO = new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>();

            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var game = new TicTacToe<string>(playerX, playerO);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { playerX, strategyX },
                    { playerO, strategyO },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static int GetSkuFromArgsOrConsole(string[] args)
        {
            if (args.Length == 1 && int.TryParse(args[0], out var num))
            {
                return num;
            }
            Console.WriteLine("Provide SKU:");
            var sku = int.Parse(Console.ReadLine());
            return sku;
        }
    }
}
