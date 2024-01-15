namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;

    using Fx.Displayer;
    using Fx.Driver;
    using Fx.Game;
    using Fx.Strategy;
    using Fx.Todo;
    using Fx.Tree;

    class Program
    {
        private static readonly IReadOnlyList<(string, Action)> games = new (string, Action)[]
        {
            (nameof(TicTacToeHumanVsHuman), TicTacToeHumanVsHuman),
            (nameof(TicTacToeHumanVsDecisionTree), TicTacToeHumanVsDecisionTree),
            (nameof(TicTacToeDecisionTreeVsHuman), TicTacToeDecisionTreeVsHuman),
            (nameof(TicTacToeTwoRandom), TicTacToeTwoRandom),
            (nameof(TicTacToeDecisionTreeVsRandom), TicTacToeDecisionTreeVsRandom),
            (nameof(TicTacToeMonteCarloVsRandom), TicTacToeMonteCarloVsRandom),
            (nameof(TicTacToeMonteCarloVsDecisionTree), TicTacToeMonteCarloVsDecisionTree),
            (nameof(PegsHuman), PegsHuman),
            (nameof(PegsDecisionTree), PegsDecisionTree),
            (nameof(PegsMonteCarlo), PegsMonteCarlo),
            (nameof(GobbleHumanVsHuman), GobbleHumanVsHuman),
            (nameof(GobbleHumanVsRandom), GobbleHumanVsRandom),
            (nameof(GobbleNumberOfMovesHeuristcVsRandom), GobbleNumberOfMovesHeuristcVsRandom),
            (nameof(GobbleMonteCarloVsRandom), GobbleMonteCarloVsRandom),
            (nameof(TwosHuman), TwosHuman),
            (nameof(TwosSevenMovesHeuristic12), TwosSevenMovesHeuristic12),
            (nameof(TwosOneMoveHeuristic12), TwosOneMoveHeuristic12),
        };

        static void Main(string[] args)
        {
            for (int i = 0; true; ++i)
            {
                var sku = GetSkuFromArgsOrConsole(args, i);
                games[sku].Item2();
                Console.WriteLine();
            }
        }

        private static int GetSkuFromArgsOrConsole(string[] args, int arg)
        {
            if (args.Length > arg && int.TryParse(args[arg], out var num))
            {
                return num;
            }

            Console.WriteLine("Available games:");
            for (int i = 0; i < games.Count; ++i)
            {
                Console.WriteLine($"{i}: {games[i].Item1}");
            }

            do
            {
                Console.WriteLine();
                Console.WriteLine("Provide SKU:");
                if (int.TryParse(Console.ReadLine(), out var sku) && sku >= 0 && sku < games.Count)
                {
                    return sku;
                }
            }
            while (true);
        }

        private static void TicTacToeHumanVsHuman()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var consoleStrategy = new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer);
            var exes = "exes";
            var ohs = "ohs";
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { exes, consoleStrategy },
                    { ohs, consoleStrategy },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeHumanVsDecisionTree()
        {
            TicTacToeHumanVsDecisionTree(1);
        }

        private static void TicTacToeDecisionTreeVsHuman()
        {
            TicTacToeHumanVsDecisionTree(0);
        }

        private static void TicTacToeHumanVsDecisionTree(int first)
        {
            var exes = "exes";
            var ohs = "ohs";
            var players = new[] { exes, ohs };
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                            { players[first % 2], new DecisionTreeStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(players[first % 2], StringComparer.OrdinalIgnoreCase) },
                            { players[(first + 1) % 2], new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeTwoRandom()
        {
            var exes = "exes";
            var ohs = "ohs";
            var strategyX = new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>();
            var strategyO = new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>();

            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { exes, strategyX },
                    { ohs, strategyO },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeDecisionTreeVsRandom()
        {
            var exes = "exes";
            var ohs = "ohs";
            var players = new[] { exes, ohs };
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { exes, new DecisionTreeStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(exes, StringComparer.OrdinalIgnoreCase) },
                    { ohs, new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>() },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeMonteCarloVsRandom()
        {
            var exes = "exes";
            var ohs = "ohs";
            var players = new[] { exes, ohs };
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var random = new Random();
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { exes, new MonteCarloStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(exes, 1000000, StringComparer.Ordinal, random) },
                    { ohs, new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>() },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeMonteCarloVsDecisionTree()
        {
            var exes = "exes";
            var ohs = "ohs";
            var players = new[] { exes, ohs };
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var random = new Random();
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { exes, new MonteCarloStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(exes, 1000000, StringComparer.Ordinal, random) },
                    { ohs, new DecisionTreeStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(ohs, StringComparer.OrdinalIgnoreCase) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsHuman()
        {
            var displayer = new PegGameConsoleDisplayer<string>();
            var player = "player";
            var game = new PegGame<string>(player);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<PegGame<string>, PegBoard, PegMove, string>>
                {
                    { player, new UserInterfaceStrategy<PegGame<string>, PegBoard, PegMove, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsDecisionTree()
        {
            var displayer = new PegGameConsoleDisplayer<string>();
            var computer = "computer";
            var game = new PegGame<string>(computer);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<PegGame<string>, PegBoard, PegMove, string>>
                {
                    { computer, new GameTreeDepthStrategy<PegGame<string>, PegBoard, PegMove, string>(game => game.ToTree().Decide(computer, StringComparer.OrdinalIgnoreCase).Value.Item3.Item2, Node.TreeFactory) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsMonteCarlo()
        {
            var displayer = new PegGameConsoleDisplayer<string>();
            var computer = "computer";
            var random = new Random();
            var game = new PegGame<string>(computer);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<PegGame<string>, PegBoard, PegMove, string>>
                {
                    { computer, new MonteCarloStrategy<PegGame<string>, PegBoard, PegMove, string>(computer, 100000, StringComparer.OrdinalIgnoreCase, random) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void GobbleHumanVsHuman()
        {
            var displayer = new GobbleConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";
            var game = new Gobble<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>>
                {
                    { exes, new UserInterfaceStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(displayer) },
                    { ohs, new UserInterfaceStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void GobbleHumanVsRandom()
        {
            var displayer = new GobbleConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";
            var game = new Gobble<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>>
                {
                    { exes, new UserInterfaceStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(displayer) },
                    { ohs, new RandomStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>() }
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void GobbleNumberOfMovesHeuristcVsRandom()
        {
            var displayer = new GobbleConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";
            var game = new Gobble<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>>
                {
                    { exes, new HeuristicStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>() },
                    { ohs, new RandomStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>() }
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void GobbleMonteCarloVsRandom()
        {
            var displayer = new GobbleConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";
            var random = new Random();
            var game = new Gobble<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>>
                {
                    { exes, new MonteCarloStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(exes, 1000000, StringComparer.OrdinalIgnoreCase, random) },
                    { ohs, new RandomStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>() }
                },
                displayer);
            var result = driver.Run(game);
        }

        public static void TwosHuman()
        {
            var displayer = new TwosConsoleDisplayer<string>(_ => _);
            var player = "player";
            var game = new Twos<string>(player, new Random(), 12);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Twos<string>, int[][], TwosDirection, string>>
                {
                    { player, new UserInterfaceStrategy<Twos<string>, int[][], TwosDirection, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        public static void TwosSevenMovesHeuristic12()
        {
            var displayer = new TwosConsoleDisplayer<string>(_ => _);
            var computer = "computer";
            var game = new Twos<string>(computer, new Random(), 12);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Twos<string>, int[][], TwosDirection, string>>
                {
                    { computer, new GameTreeDepthStrategy<Twos<string>, int[][], TwosDirection, string>(TwosHeuristics.Heuristic12, Node.TreeFactory)},
                },
                displayer);
            var result = driver.Run(game);
        }

        public static void TwosOneMoveHeuristic12()
        {
            var displayer = new TwosConsoleDisplayer<string>(_ => _);
            var computer = "computer";
            var game = new Twos<string>(computer, new Random(), 12);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Twos<string>, int[][], TwosDirection, string>>
                {
                    { computer, new HeuristicStrategy<Twos<string>, int[][], TwosDirection, string>(TwosHeuristics.Heuristic12)},
                },
                displayer);
            var result = driver.Run(game);
        }
    }
}
