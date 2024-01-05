namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;
    using Fx.Displayer;
    using Fx.Driver;
    using Fx.Game;
    using Fx.Game.Chess;
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
            (nameof(PegsHuman), PegsHuman),
            (nameof(PegsDecisionTree), PegsDecisionTree),
            (nameof(GobbleHumanVsHuman), GobbleHumanVsHuman),
            (nameof(GobbleHumanVsRandom), GobbleHumanVsRandom),
            (nameof(GobbleNumberOfMovesHeuristcVsRandom), GobbleNumberOfMovesHeuristcVsRandom),
            (nameof(TwosHuman), TwosHuman),
            (nameof(TwosSevenMovesHeuristic12), TwosSevenMovesHeuristic12),
            (nameof(TwosOneMoveHeuristic12), TwosOneMoveHeuristic12),
            (nameof(Chess), Chess),
        };

        static void Main(string[] args)
        {
            /*var b = new Fx.Game.Chess.Chess<string>("W", "B");

            System.Console.WriteLine(b.Board.Board);
            foreach (var m in b.Moves)
            {
                System.Console.WriteLine(m);
            }

            Console.WriteLine("end of moves");
            Console.ReadLine();
            return;*/



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

            var seed = Environment.TickCount;
            var random = new Random(seed);
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { exes, strategyX },
                    ////{ ohs, strategyO },
                    {ohs, new MonteCarloStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(ohs, 10000, StringComparer.OrdinalIgnoreCase, random) }
                },
                displayer);
            var result = driver.Run(game);
            Console.WriteLine(seed);
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
            var seed = Environment.TickCount;
            var random = new Random(seed);
            var game = new Gobble<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>>
                {
                    ////{ exes, new UserInterfaceStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(displayer) },
                    { exes, new MonteCarloStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(exes, 10000, StringComparer.OrdinalIgnoreCase, random) },
                    { ohs, new RandomStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>() }
                },
                displayer);
            var result = driver.Run(game);
            Console.WriteLine(seed);
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

        private static double ChessScore<TPlayer>(ChessGame<TPlayer> game, TPlayer player)
        {
            var scores = new Dictionary<ChessPieceKind, int>()
            {
                { ChessPieceKind.King, 0 },
                { ChessPieceKind.Pawn, 1},
                { ChessPieceKind.Knight, 3},
                { ChessPieceKind.Bishop, 3},
                { ChessPieceKind.Rook ,5},
                { ChessPieceKind.Queen, 9},
            };
            return game
                .Board
                .Board
                .Board
                .Cast<ChessPiece?>()
                .Where(piece => piece.HasValue)
                .Select(piece => piece.Value)
                .Sum(piece => scores[piece.Kind] * ((piece.Color == game.PlayerColors[player]) ? 1 : -1));
        }

        public static void Chess()
        {
            //// TODO comparers and tests, kind of like the driver stuff

            ////var seed = -2056050046; //// Environment.TickCount;
            var seed = Environment.TickCount;
            var rng = new Random(seed);
            var displayer = new ChessConsoleDisplayer<string>();
            var tree = "tree";
            var random = "random";
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>>
                {
                    ////{ computer, new RandomStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>() },
                    { random, new RandomStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>(rng) },
                    ////{ tree, new GameTreeDepthStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>(game => ChessScore(game, tree), Node.TreeFactory) }
                    ////{ tree, new ChessStrategy<string>(tree, StringComparer.OrdinalIgnoreCase) },
                    { tree, new MonteCarloStrategy<ChessGame<string>, ChessGameState, Fx.Game.Chess.ChessMove, string>(tree, 20000, StringComparer.OrdinalIgnoreCase, rng) },
                    // { human, new UserInterfaceStrategy<Fx.Game.Chess.Chess<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>(displayer) },
                },
                displayer);
            var game = new Fx.Game.Chess.ChessGame<string>(random, tree);
            var result = driver.Run(game);
            Console.WriteLine(seed);
        }

        private sealed class ChessStrategy<TPlayer> : IStrategy<Fx.Game.Chess.ChessGame<TPlayer>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, TPlayer>
        {
            private readonly MaximizeMovesStrategy<Fx.Game.Chess.ChessGame<TPlayer>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, TPlayer> maximizeMovesStrategy;

            private readonly GameTreeDepthStrategy<Fx.Game.Chess.ChessGame<TPlayer>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, TPlayer> gameTreeStrategy;

            private readonly DecisionTreeStrategy<Fx.Game.Chess.ChessGame<TPlayer>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, TPlayer> decisionTreeStrategy;

            public ChessStrategy(TPlayer player, IEqualityComparer<TPlayer> playerComparer)
            {
                this.maximizeMovesStrategy = MaximizeMovesStrategy.Default<Fx.Game.Chess.ChessGame<TPlayer>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, TPlayer>();
                this.gameTreeStrategy = new GameTreeDepthStrategy<ChessGame<TPlayer>, ChessGameState, Fx.Game.Chess.ChessMove, TPlayer>(
                    game =>  ChessScore(game, player),
                    Node.TreeFactory);
                this.decisionTreeStrategy = new DecisionTreeStrategy<ChessGame<TPlayer>, ChessGameState, Fx.Game.Chess.ChessMove, TPlayer>(player, playerComparer);

            }

            public Fx.Game.Chess.ChessMove SelectMove(ChessGame<TPlayer> game)
            {
                /*if (game.Board.HalfMoveCount < 10)
                {
                    return this.maximizeMovesStrategy.SelectMove(game);
                }
                else*/
                //// TODO hard-coded piece color
                ////if (game.Board.Board.Board.Cast<ChessPiece?>().Where(piece => piece?.Color == ChessPieceColor.White && piece?.Kind != ChessPieceKind.King).Any())
                {
                    return this.gameTreeStrategy.SelectMove(game);
                }
                /*else
                {
                    return this.decisionTreeStrategy.SelectMove(game);
                }*/
            }
        }

        private sealed class ChessConsoleDisplayer<TPlayer> : IDisplayer<Fx.Game.Chess.ChessGame<TPlayer>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, TPlayer>
        {
            public void DisplayBoard(ChessGame<TPlayer> game)
            {
                Console.WriteLine(game.Board.Board.ToString());
            }

            public void DisplayMoves(ChessGame<TPlayer> game)
            {
                Console.WriteLine($"{game.CurrentPlayer}, please select a move (row, column):");
                int i = 0;
                foreach (var move in game.Moves)
                {
                    Console.WriteLine($"{i++}: {move}");
                }

                Console.WriteLine();
            }

            public void DisplayOutcome(ChessGame<TPlayer> game)
            {
                Console.WriteLine($"{game.Outcome.Winners.FirstOrDefault()} won the game! The other player is clearly a loser!");
            }

            public Fx.Game.Chess.ChessMove ReadMoveSelection(ChessGame<TPlayer> game)
            {
                var moves = game.Moves.ToList();
                while (true)
                {
                    var input = Console.ReadLine();
                    if (!int.TryParse(input, out var selectedMove) || selectedMove >= moves.Count)
                    {
                        Console.WriteLine($"The input '{input}' was not the index of a legal move");
                        continue;
                    }

                    var move = moves[selectedMove];
                    return move;
                }
            }
        }
    }
}
