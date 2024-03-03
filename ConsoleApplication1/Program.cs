namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using Fx.Displayer;
    using Fx.Driver;
    using Fx.Game;
    using Fx.Game.Chess;
    using Fx.Strategy;
    using Fx.Todo;
    using Fx.Tree;

    partial class Program
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
            (nameof(LonghornHumanVsRandom), LonghornHumanVsRandom),
        };

        static void Main(string[] args)
        {
            ////ConsoleApplication2.ListExt.DoWork();

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

            var seed = -2056050046; //// Environment.TickCount;
            var rng = new Random(seed);
            var displayer = new ChessConsoleDisplayer<string>();
            var tree = "tree";
            var random = "random";
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>>
                {
                    ////{ computer, new RandomStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>() },
                    ////{ random, new RandomStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>(rng) },
                    { random, new GameTreeDepthStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>(game => ChessScore(game, tree), Node.TreeFactory, 2) },
                    ////{ tree, new GameTreeDepthStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>(game => ChessScore(game, tree), Node.TreeFactory, 3) },
                    ////{ "test", new DecisionTreeStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>("test", StringComparer.OrdinalIgnoreCase) },
                    { tree, new FixedCountStrategy<Fx.Game.Chess.ChessGame<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>(100, rng, tree, StringComparer.OrdinalIgnoreCase, EqualityComparer<Fx.Game.Chess.ChessMove>.Default, EqualityComparer<Fx.Game.Chess.ChessGame<string>>.Default) },
                    ////{ tree, new ChessStrategy<string>(tree, StringComparer.OrdinalIgnoreCase) },
                    // { human, new UserInterfaceStrategy<Fx.Game.Chess.Chess<string>, Fx.Game.Chess.ChessGameState, Fx.Game.Chess.ChessMove, string>(displayer) },
                },
                displayer);
            var game = new Fx.Game.Chess.ChessGame<string>(random, tree);
            var result = driver.Run(game);
        }

        private static void LonghornHumanVsRandom()
        {
            var displayer = new LonghornConsoleDisplay<string>(_ => _);
            ////var seed = 581316343;
            var seed = Environment.TickCount;
            var random = new Random(seed);
            var exes = "player1";
            var ohs = "player2";
            var game = new Longhorn<string>(exes, ohs, random);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Longhorn<string>, LonghornBoard<string>, LonghornMove, string>>
                {
                    { exes, UserInterfaceStrategy.Create(displayer) },
                    ////{ exes, game.RandomStrategy() },
                    ////{ exes, new MonteCarloStrategy2<Longhorn<string>, LonghornBoard<string>, LonghornMove, string>(exes, 100000, StringComparer.OrdinalIgnoreCase, random) },
                    ////{ ohs, game.RandomStrategy() },
                    ////{ ohs, UserInterfaceStrategy.Create(displayer) },
                    { ohs, new MonteCarloStrategy2<Longhorn<string>, LonghornBoard<string>, LonghornMove, string>(ohs, 100000, StringComparer.OrdinalIgnoreCase, random) },
                    ////{ ohs, new DecisionTreeStrategy<Longhorn<string>, LonghornBoard<string>, LonghornMove, string>(ohs, StringComparer.OrdinalIgnoreCase) },
                },
                displayer);
            Console.WriteLine(seed);
            var result = driver.Run(game);
            Console.WriteLine(seed);
        }

        public sealed class ChessComparer<TPlayer> : IEqualityComparer<ChessGame<TPlayer>>
        {
            public bool Equals(ChessGame<TPlayer>? x, ChessGame<TPlayer>? y)
            {
                return x.Equals(y);
            }

            public int GetHashCode([DisallowNull] ChessGame<TPlayer> obj)
            {
                var hashcode = 0;
                for (int i = 0; i < 8; ++i)
                {
                    for (int j = 0; j < 8; ++j)
                    {
                        hashcode ^= obj.Board.Board.Board[i, j].GetHashCode();
                    }
                }

                return hashcode;
            }
        }

        public sealed class FixedCountStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            private readonly int count;

            private readonly Random random;

            private readonly TPlayer player; //// TODO should this be a property on istrategy?

            private readonly IEqualityComparer<TPlayer> playerComparer;

            private readonly IEqualityComparer<TMove> moveComparer;

            private readonly IEqualityComparer<TGame> gameComparer;

            private sealed class NewComparer : IEqualityComparer<(TGame, TMove)>
            {
                private readonly IEqualityComparer<TMove> moveComparer;

                private readonly IEqualityComparer<TGame> gameComparer;

                public NewComparer(IEqualityComparer<TMove> moveComparer, IEqualityComparer<TGame> gameComparer)
                {
                    this.moveComparer = moveComparer;
                    this.gameComparer = gameComparer;
                }

                public bool Equals((TGame, TMove) x, (TGame, TMove) y)
                {
                    return this.gameComparer.Equals(x.Item1, y.Item1) && this.moveComparer.Equals(x.Item2, y.Item2);
                }

                public int GetHashCode([DisallowNull] (TGame, TMove) obj)
                {
                    return this.gameComparer.GetHashCode(obj.Item1) ^ this.moveComparer.GetHashCode(obj.Item2);
                }
            }

            public FixedCountStrategy(int count, Random random, TPlayer player, IEqualityComparer<TPlayer> playerComparer, IEqualityComparer<TMove> moveComparer, IEqualityComparer<TGame> gameComparer)
            {
                this.count = count;
                this.random = random;
                this.player = player;
                this.playerComparer = playerComparer;
                this.moveComparer = moveComparer;
                this.gameComparer = gameComparer;
            }

            public TMove SelectMove(TGame game)
            {
                if (game.Outcome != null)
                {
                    throw new InvalidOperationException("TODO");
                }

                var oldGame = game;
                var moveScores = new Dictionary<TMove, List<WinnabilityScore<TGame, TBoard, TMove, TPlayer>>>(this.moveComparer);
                ////var selectedMoves = new Dictionary<TGame, List<TMove>>(this.gameComparer);
                for (int i = 0; i < this.count; ++i)
                {
                    TMove originalMove = random.Choose(game.Moves.ToList());
                    var move = originalMove;
                    while (game.Outcome == null)
                    {
                        /*if (!selectedMoves.TryGetValue(game, out var previouslySelectedMoves))
                        {
                            previouslySelectedMoves = new List<TMove>();
                            selectedMoves[game] = previouslySelectedMoves;
                        }*/

                        var moves = game.Moves;
                        ////var elligibleMoves = moves.Where(move => !previouslySelectedMoves.Contains(move, this.moveComparer)).ToList();
                        var elligibleMoves = moves.ToList();

                        move = random.Choose(elligibleMoves);
                        ////previouslySelectedMoves.Add(move);
                        game = game.CommitMove(move);
                    }

                    var score = ComputeScore2(game, this.player, this.playerComparer);
                    if (!moveScores.TryGetValue(originalMove, out var scores))
                    {
                        scores = new List<WinnabilityScore<TGame, TBoard, TMove, TPlayer>>();
                        moveScores[originalMove] = scores;
                    }

                    scores.Add(score);
                    game = oldGame;
                }

                var val = moveScores.Maximum(kvp => kvp.Value.Average(score => score.Score.Score)).Key;
                return val;
            }
        }

        public static WinnabilityScore<TGame, TBoard, TMove, TPlayer> ComputeScore2<TGame, TBoard, TMove, TPlayer>(IGame<TGame, TBoard, TMove, TPlayer> game, TPlayer player, IEqualityComparer<TPlayer> playerComparer) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new WinnabilityScore<TGame, TBoard, TMove, TPlayer>()
            {
                Game = game,
                Move = default(TMove),
                Score =
                    game.Outcome?.Winners.Contains(player, playerComparer) == true ? new WinnabilityScore<TGame, TBoard, TMove, TPlayer>.SafeScore() { Status = DecisionTreeStatus.Win, Score = 1.0 } :
                    game.Outcome?.Winners.Any() == false ? new WinnabilityScore<TGame, TBoard, TMove, TPlayer>.SafeScore() { Status = DecisionTreeStatus.Draw, Score = 0.0 } :
                    new WinnabilityScore<TGame, TBoard, TMove, TPlayer>.SafeScore() { Status = DecisionTreeStatus.Lose, Score = -1.0 },
            };
        }

        public sealed class WinnabilityScore<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            public IGame<TGame, TBoard, TMove, TPlayer> Game { get; set; }

            public TMove Move { get; set; }

            public SafeScore Score { get; set; }

            public sealed class SafeScore
            {
                public DecisionTreeStatus Status { get; set; }

                public double Score { get; set; }
            }
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
                    game => ChessScore(game, player),
                    Node.TreeFactory,
                    2);
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
                Console.WriteLine($"{game.Outcome.Winners.First()} won the game! The other player is clearly a loser!");
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
