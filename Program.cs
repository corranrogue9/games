using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Program
    {
        private static double MontyCarloSimulation<TGame, TMove, TPlayer>(TGame game, Func<Outcome<TPlayer>, bool> predicate, int simulations, Random random)
            where TGame : IGame<TMove, TPlayer, TGame>
        {
            simulations = 50;
            var moveCount = 10;

            int wins = 0;
            int losses = 0;
            for (int i = 0; i < simulations; ++i)
            {
                var playedGame = game;
                for (int j = 0; j < moveCount && playedGame.Outcome == null; ++j)
                {
                    var moves = playedGame.LegalMoves.ToList();
                    playedGame = playedGame.CommitMove(moves[random.Next(moves.Count)]);
                }

                var outcome = playedGame.Outcome;
                if (outcome != null)
                {
                    if (predicate(playedGame.Outcome))
                    {
                        ++wins;
                    }
                    else
                    {
                        ++losses;
                    }
                }
            }

            if (wins > 0)
            {
                return 1.0;
            }
            
            return (simulations - losses) / (double)simulations;
        }

        private sealed class BrettStrategy : IStrategy<Direction, string, Twos<string>>
        {
            private readonly ConsoleStrategy<Direction, string, Twos<string>> consoleStrategy;

            private bool manual;

            private bool down;

            public BrettStrategy(ConsoleStrategy<Direction, string, Twos<string>> consoleStrategy)
            {
                this.consoleStrategy = consoleStrategy;

                this.manual = false;
                this.down = false;
            }

            public Direction SelectMove(Twos<string> game)
            {
                var sleepTime = 100;
                if (this.manual)
                {
                    Console.WriteLine("you are now on manual mode; enter 5 to resume automated mode");
                    try
                    {
                        return this.consoleStrategy.SelectMove(game);
                    }
                    catch
                    {
                        this.manual = false;
                    }
                }

                if (this.down && game.LegalMoves.Contains(Direction.Down))
                {
                    this.down = false;
                    return Direction.Down;
                }

                if (game.Board[3][3] == game.Board[3][2] && game.Board[3][3] != 0)
                {
                    System.Threading.Thread.Sleep(sleepTime);
                    return Direction.Right;
                }

                if (game.Board[3][3] == game.Board[2][3] && game.Board[3][3] != 0)
                {
                    System.Threading.Thread.Sleep(sleepTime);
                    return Direction.Down;
                }

                if (game.LegalMoves.Contains(Direction.Right))
                {
                    System.Threading.Thread.Sleep(sleepTime);
                    return Direction.Right;
                }
                else if (game.LegalMoves.Contains(Direction.Down))
                {
                    System.Threading.Thread.Sleep(sleepTime);
                    return Direction.Down;
                }

                var bottomRight = game.Board[3][3];
                if (game.LegalMoves.Contains(Direction.Up))
                {
                    var newGame = game.CommitMove(Direction.Up);
                    if (newGame.Board[3][3] == bottomRight)
                    {
                        System.Threading.Thread.Sleep(sleepTime);
                        return Direction.Up;
                    }
                }

                if (game.LegalMoves.Contains(Direction.Up))
                {
                    this.down = true;
                    return Direction.Up;
                }
                
                this.manual = true;
                return this.SelectMove(game);
            }
        }

        static void Main(string[] args)
        {
            //// TODO do other display mechanism to formalize "board state"
            //// TODO do game of amazons to fix decision tree with monte carlo simulation
            //// TODO do more games
            Func<Direction, string> displayMove = (move) =>
            {
                return move.ToString();
            };
            var ticks = 551040140;//// Environment.TickCount;
            ////var ticks = 1071300156;
            Console.WriteLine(ticks);
            var random = new Random(ticks);
            var consolestat = new ConsoleStrategy<Direction, string, Twos<string>>(displayMove);
            var results = new GameRunner<Direction, string, Twos<string>>(
                new Twos<string>("brett", new Random(ticks), 12),
                new Dictionary<string, IStrategy<Direction, string, Twos<string>>>
                {
                    ////{ "gdebruin", new ConsoleStrategy<Direction, string, Twos<string>>(displayMove)},
                    { "brett", new BrettStrategy(consolestat) },
                    ////{ "gdebruin", new DecisionTree<Direction, string, Twos<string>>(outcome => outcome.Winners.Contains("gdebruin")) },
                    /*{
                        "gdebruin",
                        new DecisionTreeImp<Direction, string, Twos<string>>(
                            outcome => outcome.Winners.Contains("gdebruin"),
                            2, 
                            game => MontyCarloSimulation<Twos<string>, Direction, string>(game, outcome => outcome.Winners.Contains("gdebruin"), 100000, random))
                    },*/
                    ////{ "gdebruin", new RandomStrategy<Direction, string, Twos<string>>(new Random(ticks)) },
                }).Start();

            foreach (var loser in results.Losers)
            {
                Console.WriteLine($"{loser} loses...");
            }

            foreach (var winner in results.Winners)
            {
                Console.WriteLine($"{winner} wins!");
            }

            Console.ReadLine();

            /*Func<PegMove, string> displayMove = (move) =>
            {
                return $"Move peg at ({move.From.Row}, {move.From.Column}) to ({move.To.Row},{move.To.Column})";
            };
            var results = new GameRunner<PegMove, string, PegGame<string>>(
                new PegGame<string>("gdebruin"),
                new Dictionary<string, IStrategy<PegMove, string, PegGame<string>>>
                {
                    ////{ "gdebruin", new ConsoleStrategy<PegMove, string, PegGame<string>>(displayMove)}
                    { "gdebruin", new DecisionTree<PegMove, string, PegGame<string>>(outcome => outcome.Winners.Contains("gdebruin")) },
                }).Start();

            foreach (var loser in results.Losers)
            {
                Console.WriteLine($"{loser} loses...");
            }

            foreach (var winner in results.Winners)
            {
                Console.WriteLine($"{winner} wins!");
            }

            Console.ReadLine();*/

            /*Func<Placement, string> displayMove = (placement) =>
            {
                return $"Place piece at ({placement.Row}, {placement.Column})";
            };
            var results = new GameRunner<Placement, string, TicTacToe<string>>(
                new TicTacToe<string>("gdebruin", "player 1"),
                new Dictionary<string, IStrategy<Placement, string, TicTacToe<string>>>
                {
                    { "gdebruin", new ConsoleStrategy<Placement, string, TicTacToe<string>>(displayMove) },
                    { "decision tree", new DecisionTree<Placement, string, TicTacToe<string>>(outcome => outcome.Winners.Contains("decision tree")) },
                    { "computer", new RandomStrategy<Placement, string, TicTacToe<string>>(new Random()) },
                    { "player 1", new DecisionTree<Placement, string, TicTacToe<string>>(outcome => outcome.Winners.Contains("player 1") || outcome.Draws.Contains("player 1")) },
                    { "player 2", new DecisionTree<Placement, string, TicTacToe<string>>(outcome => outcome.Winners.Contains("player 2") || outcome.Draws.Contains("player 2")) },
                }).Start();

            foreach (var loser in results.Losers)
            {
                Console.WriteLine($"{loser} loses...");
            }

            foreach (var winner in results.Winners)
            {
                Console.WriteLine($"{winner} wins!");
            }

            foreach (var drawer in results.Draws)
            {
                Console.WriteLine($"{drawer} drew the game.");
            }

            Console.ReadLine();*/

            /*Func<Placement, string> displayMove = (placement) =>
            {
                return $"Place piece at ({placement.Row}, {placement.Column})";
            };
            var results = new GameRunner<Placement, string>(
                new TicTacToe<string>("gdebruin", "computer"),
                new Dictionary<string, IStrategy<Placement, string>>
                {
                    { "gdebruin", new ConsoleStrategy<Placement, string>(displayMove) },
                    { "computer", new RandomStrategy<Placement, string>(new Random()) },
                }).Start();

            foreach (var loser in results.Losers)
            {
                Console.WriteLine($"{loser} loses...");
            }

            foreach (var winner in results.Winners)
            {
                Console.WriteLine($"{winner} wins!");
            }

            foreach (var drawer in results.Draws)
            {
                Console.WriteLine($"{drawer} drew the game.");
            }

            Console.ReadLine();*/

            /*Func<PegMove, string> displayMove = (move) =>
            {
                return $"Move peg at ({move.From.Row}, {move.From.Column}) to ({move.To.Row},{move.To.Column})";
            };
            var results = new GameRunner<PegMove, string>(
                new PegGame<string>("gdebruin"),
                new Dictionary<string, IStrategy<PegMove>>
                {
                    ////{ "gdebruin", new ConsoleStrategy<PegMove>(displayMove)}
                    { "gdebruin", new RandomStrategy<PegMove>(new Random(238)) }
                }).Start();

            foreach (var loser in results.Losers)
            {
                Console.WriteLine($"{loser} loses...");
            }

            foreach (var winner in results.Winners)
            {
                Console.WriteLine($"{winner} wins!");
            }

            Console.ReadLine();*/
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    public sealed class Twos<TPlayer> : IGameWithHiddenInformation<Direction, TPlayer, Twos<TPlayer>>
    {
        private readonly TPlayer player;

        private readonly int[][] board;

        private readonly Random random;

        private readonly int win;

        public Twos(TPlayer player, Random random, int win = 11)
        {
            this.win = win;

            this.player = player;
            this.random = random;

            this.board = new int[4][];
            for (int i = 0; i < this.board.Length; ++i)
            {
                this.board[i] = new int[4];
            }

            for (int i = 0; i < 2; ++i)
            {
                while (true)
                {
                    //// TODO you can probably do better
                    var row = this.random.Next(0, this.board.Length);
                    var column = this.random.Next(0, this.board[row].Length);
                    if (this.board[row][column] == 0)
                    {
                        if (this.random.Next(0, 10) == 0)
                        {
                            this.board[row][column] = 2;
                        }
                        else
                        {
                            this.board[row][column] = 1;
                        }

                        break;
                    }
                }
            }
        }

        private Twos(TPlayer player, int[][] board, Random random, int win)
        {
            this.win = win;

            this.player = player;
            this.random = random;

            this.board = new int[board.Length][];
            for (int i = 0; i < this.board.Length; ++i)
            {
                this.board[i] = board[i].Clone() as int[];
            }

            /*this.board = new int[board.Length][];
            for (int i = 0; i < this.board.Length; ++i)
            {
                this.board[i] = new int[board[i].Length];
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    this.board[i][j] = board[i][j];
                }
            }*/
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.player;
            }
        }
        
        public int Max
        {
            get
            {
                return this.board.Max(row => row.Max());
            }
        }


        private bool Up()
        {
            var up = false;
            for (int i = this.board.Length - 1; i >= 1; --i)
            {
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    if (this.board[i][j] != 0)
                    {
                        for (int k = i - 1; k >= 0; --k)
                        {
                            if (this.board[k][j] == 0 || this.board[k][j] == this.board[i][j])
                            {
                                return true;
                            }

                            if (board[k][j] != 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool Down()
        {
            var down = false;
            for (int i = 0; i < this.board.Length - 1; ++i)
            {
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    if (this.board[i][j] != 0)
                    {
                        for (int k = i + 1; k < this.board.Length; ++k)
                        {
                            if (this.board[k][j] == 0 || this.board[k][j] == this.board[i][j])
                            {
                                return true;
                            }

                            if (board[k][j] != 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool Left()
        {
            var left = false;
            for (int i = 0; i < this.board.Length; ++i)
            {
                for (int j = this.board[i].Length - 1; j >= 1; --j)
                {
                    if (this.board[i][j] != 0)
                    {
                        for (int k = j - 1; k >= 0; --k)
                        {
                            if (this.board[i][k] == 0 || this.board[i][k] == this.board[i][j])
                            {
                                return true;
                            }

                            if (board[i][k] != 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool Right()
        {
            var right = false;
            for (int i = 0; i < this.board.Length; ++i)
            {
                for (int j = 0; j < this.board[i].Length - 1; ++j)
                {
                    if (this.board[i][j] != 0)
                    {
                        for (int k = j + 1; k < this.board[i].Length; ++k)
                        {
                            if (this.board[i][k] == 0 || this.board[i][k] == this.board[i][j])
                            {
                                return true;
                            }

                            if (board[i][k] != 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public IEnumerable<Direction> LegalMoves
        {
            get
            {
                if (Up())
                {
                    yield return Direction.Up;
                }

                if (Down())
                {
                    yield return Direction.Down;
                }

                if (Left())
                {
                    yield return Direction.Left;
                }

                if (Right())
                {
                    yield return Direction.Right;
                }
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                for (int i = 0; i < this.board.Length; ++i)
                {
                    for (int j = 0; j < this.board[i].Length; ++j)
                    {
                        if (this.board[i][j] == this.win)
                        {
                            return new Outcome<TPlayer>(new[] { this.player }, Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
                        }
                    }
                }

                if (!this.LegalMoves.Any())
                {
                    return new Outcome<TPlayer>(Enumerable.Empty<TPlayer>(), new[] { this.player }, Enumerable.Empty<TPlayer>());
                }

                return null;
            }
        }

        public Twos<TPlayer> CommitMove(Direction move)
        {
            var newGames = ExploreMove(move).ToList();
            return newGames[this.random.Next(0, newGames.Count)];
        }

        public int[][] Board
        {
            get
            {
                return this.board;
            }
        }

        public void Display()
        {
            for (int i = 0; i < this.board.Length; ++i)
            {
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    if (this.board[i][j] == 0)
                    {
                        Console.Write("0 ");
                    }
                    else
                    {
                        Console.Write($"{1 << this.board[i][j]} ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public IEnumerable<Twos<TPlayer>> ExploreMove(Direction move)
        {
            var newGame = new Twos<TPlayer>(this.player, this.board, this.random, this.win);
            if (move == Direction.Up)
            {
                for (int j = 0; j < newGame.board.Length; ++j)
                {
                    for (int i = 0; i < newGame.board.Length; ++i)
                    {
                        if (newGame.board[i][j] == 0)
                        {
                            for (int k = i + 1; k < newGame.board.Length; ++k)
                            {
                                if (newGame.board[k][j] != 0)
                                {
                                    newGame.board[i][j] = newGame.board[k][j];
                                    newGame.board[k][j] = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < newGame.board.Length; ++j)
                {
                    for (int i = 0; i < newGame.board.Length - 1; ++i)
                    {
                        if (newGame.board[i][j] != 0 && newGame.board[i + 1][j] == newGame.board[i][j])
                        {
                            newGame.board[i][j]++;
                            newGame.board[i + 1][j] = 0;
                            for (int k = i + 1; k < newGame.board.Length - 1; ++k)
                            {
                                newGame.board[k][j] = newGame.board[k + 1][j];
                                newGame.board[k + 1][j] = 0;
                            }
                        }
                    }
                }
            }
            else if (move == Direction.Down)
            {
                for (int j = 0; j < newGame.board.Length; ++j)
                {
                    for (int i = newGame.board.Length - 1; i >= 0; --i)
                    {
                        if (newGame.board[i][j] == 0)
                        {
                            for (int k = i - 1; k >= 0; --k)
                            {
                                if (newGame.board[k][j] != 0)
                                {
                                    newGame.board[i][j] = newGame.board[k][j];
                                    newGame.board[k][j] = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < newGame.board.Length; ++j)
                {
                    for (int i = newGame.board.Length - 1; i >= 1; --i)
                    {
                        if (newGame.board[i][j] != 0 && newGame.board[i - 1][j] == newGame.board[i][j])
                        {
                            newGame.board[i][j]++;
                            newGame.board[i - 1][j] = 0;
                            for (int k = i - 1; k >= 1; --k)
                            {
                                newGame.board[k][j] = newGame.board[k - 1][j];
                                newGame.board[k - 1][j] = 0;
                            }
                        }
                    }
                }
            }
            else if (move == Direction.Left)
            {
                for (int i = 0; i < newGame.board.Length; ++i)
                {
                    for (int j = 0; j < newGame.board.Length; ++j)
                    {
                        if (newGame.board[i][j] == 0)
                        {
                            for (int k = j + 1; k < newGame.board.Length; ++k)
                            {
                                if (newGame.board[i][k] != 0)
                                {
                                    newGame.board[i][j] = newGame.board[i][k];
                                    newGame.board[i][k] = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < newGame.board.Length; ++i)
                {
                    for (int j = 0; j < newGame.board.Length - 1; ++j)
                    {
                        if (newGame.board[i][j] != 0 && newGame.board[i][j + 1] == newGame.board[i][j])
                        {
                            newGame.board[i][j]++;
                            newGame.board[i][j + 1] = 0;
                            for (int k = j + 1; k < newGame.board.Length - 1; ++k)
                            {
                                newGame.board[i][k] = newGame.board[i][k + 1];
                                newGame.board[i][k + 1] = 0;
                            }
                        }
                    }
                }
            }
            else if (move == Direction.Right)
            {
                for (int i = 0; i < newGame.board.Length; ++i)
                {
                    for (int j = newGame.board.Length - 1; j >= 0; --j)
                    {
                        if (newGame.board[i][j] == 0)
                        {
                            for (int k = j - 1; k >= 0; --k)
                            {
                                if (newGame.board[i][k] != 0)
                                {
                                    newGame.board[i][j] = newGame.board[i][k];
                                    newGame.board[i][k] = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < newGame.board.Length; ++i)
                {
                    for (int j = newGame.board.Length - 1; j >= 1; --j)
                    {
                        if (newGame.board[i][j] != 0 && newGame.board[i][j - 1] == newGame.board[i][j])
                        {
                            newGame.board[i][j]++;
                            newGame.board[i][j - 1] = 0;
                            for (int k = j - 1; k >= 1; --k)
                            {
                                newGame.board[i][k] = newGame.board[i][k - 1];
                                newGame.board[i][k - 1] = 0;
                            }
                        }
                    }
                }
            }

            var emptySlot = false;
            for (int i = 0; i < newGame.board.Length; ++i)
            {
                for (int j = 0; j < newGame.board[i].Length; ++j)
                {
                    if (newGame.board[i][j] == 0)
                    {
                        /*for (int k = 0; k < 10; ++k)
                        {
                            var explore = new Twos<TPlayer>(newGame.player, newGame.board, newGame.random, this.win);
                            if (k == 0)
                            {
                                explore.board[i][j] = 2;
                            }
                            else
                            {
                                explore.board[i][j] = 1;
                            }

                            emptySlot = true;
                            yield return explore;
                        }*/
                        var explore = new Twos<TPlayer>(newGame.player, newGame.board, newGame.random, this.win);
                        explore.board[i][j] = 1;

                        emptySlot = true;
                        yield return explore;
                    }
                }
            }

            if (!emptySlot)
            {
                yield return newGame;
            }
        }
    }

    public interface IGameWithHiddenInformation<TMove, TPlayer, TGame> : IGame<TMove, TPlayer, TGame> where TGame : IGameWithHiddenInformation<TMove, TPlayer, TGame>
    {
        IEnumerable<TGame> ExploreMove(TMove move);
    }

    public sealed class DecisionTreeImp<TMove, TPlayer, TGame> : IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        private readonly Func<Outcome<TPlayer>, bool> predicate;

        private readonly int depth;

        private readonly Func<TGame, double> delegateStrategy;

        public DecisionTreeImp(Func<Outcome<TPlayer>, bool> predicate, int depth, Func<TGame, double> delegateStrategy)
        {
            this.predicate = predicate;
            this.depth = depth;
            this.delegateStrategy = delegateStrategy;
        }

        public TMove SelectMove(TGame game)
        {
            var legalMoves = game.LegalMoves.ToList();
            var maxIndex = 0;
            var maxWeight = ComputeWeight(game.CommitMove(legalMoves[0]), 0);
            for (int i = 1; i < legalMoves.Count; ++i)
            {
                var newWeight = ComputeWeight(game.CommitMove(legalMoves[i]), 0);
                if (newWeight > maxWeight)
                {
                    maxIndex = i;
                    maxWeight = newWeight;
                }
            }

            return legalMoves[maxIndex];
        }

        private double ComputeWeight(TGame game, int depth)
        {
            if (depth == this.depth)
            {
                var result = this.delegateStrategy(game);
                return result;
            }

            var outcome = game.Outcome;
            if (outcome != null)
            {
                if (this.predicate(outcome))
                {
                    return 1.0;
                }
                else
                {
                    return 0.0;
                }
            }

            var average = 0.0;
            int count = 0;
            foreach (var move in game.LegalMoves)
            {
                average += ComputeWeight(game.CommitMove(move), depth + 1);
                ++count;
            }

            return average / count;
        }
    }

    public sealed class DecisionTree<TMove, TPlayer, TGame> : IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        private readonly Func<Outcome<TPlayer>, bool> predicate;

        public DecisionTree(Func<Outcome<TPlayer>, bool> predicate)
        {
            this.predicate = predicate;
        }

        public TMove SelectMove(TGame game)
        {
            var legalMoves = game.LegalMoves.ToList();
            var maxIndex = 0;
            var maxWeight = ComputeWeight(game.CommitMove(legalMoves[0]));
            for (int i = 1; i < legalMoves.Count; ++i)
            {
                var newWeight = ComputeWeight(game.CommitMove(legalMoves[i]));
                if (newWeight > maxWeight)
                {
                    maxIndex = i;
                    maxWeight = newWeight;
                }
            }

            return legalMoves[maxIndex];
        }

        private double ComputeWeight(TGame game)
        {
            var outcome = game.Outcome;
            if (outcome != null)
            {
                if (this.predicate(outcome))
                {
                    return 1.0;
                }
                else
                {
                    return 0.0;
                }
            }

            var average = 0.0;
            int count = 0;
            foreach (var move in game.LegalMoves)
            {
                average += ComputeWeight(game.CommitMove(move));
                ++count;
            }

            return average / count;
        }
    }

    public sealed class Placement
    {
        public Placement(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public int Row { get; }

        public int Column { get; }
    }

    public sealed class TicTacToe<TPlayer> : IGame<Placement, TPlayer, TicTacToe<TPlayer>>
    {
        private readonly TPlayer[] players;

        private readonly Piece[][] board;

        private int currentPlayerIndex;

        public TicTacToe(TPlayer firstPlayer, TPlayer secondPlayer)
        {
            this.players = new TPlayer[2];
            this.players[0] = firstPlayer;
            this.players[1] = secondPlayer;

            this.board = new Piece[3][];
            for (int i = 0; i < this.board.Length; ++i)
            {
                this.board[i] = new Piece[3];
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    this.board[i][j] = Piece.Empty;
                }
            }

            this.currentPlayerIndex = 0;
        }

        private TicTacToe(TPlayer[] players, int currentPlayerIndex, Piece[][] board)
        {
            this.players = players;
            this.currentPlayerIndex = currentPlayerIndex;
            this.board = board;
        }

        public void Display()
        {
            for (int i = 0; i < this.board.Length; ++i)
            {
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    if (this.board[i][j] == Piece.Empty)
                    {
                        Console.Write("_ ");
                    }
                    else if (this.board[i][j] == Piece.X)
                    {
                        Console.Write("X ");
                    }
                    else if (this.board[i][j] == Piece.O)
                    {
                        Console.Write("O ");
                    }
                }
                
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.players[this.currentPlayerIndex];
            }
        }

        public IEnumerable<Placement> LegalMoves
        {
            get
            {
                for (int i = 0; i < this.board.Length; ++i)
                {
                    for (int j = 0; j < this.board[i].Length; ++j)
                    {
                        if (this.board[i][j] == Piece.Empty)
                        {
                            yield return new Placement(i, j);
                        }
                    }
                }
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                Piece? winner = null;
                if (this.board[0][0] == this.board[0][1] && this.board[0][1] == this.board[0][2])
                {
                    if (this.board[0][0] != Piece.Empty)
                    {
                        winner = this.board[0][0];
                    }
                }

                if (this.board[1][0] == this.board[1][1] && this.board[1][1] == this.board[1][2])
                {
                    if (this.board[1][0] != Piece.Empty)
                    {
                        winner = this.board[1][0];
                    }
                }

                if (this.board[2][0] == this.board[2][1] && this.board[2][1] == this.board[2][2])
                {
                    if (this.board[2][0] != Piece.Empty)
                    {
                        winner = this.board[2][0];
                    }
                }

                if (this.board[0][0] == this.board[1][0] && this.board[1][0] == this.board[2][0])
                {
                    if (this.board[0][0] != Piece.Empty)
                    {
                        winner = this.board[0][0];
                    }
                }

                if (this.board[0][1] == this.board[1][1] && this.board[1][1] == this.board[2][1])
                {
                    if (this.board[0][1] != Piece.Empty)
                    {
                        winner = this.board[0][1];
                    }
                }

                if (this.board[0][2] == this.board[1][2] && this.board[1][2] == this.board[2][2])
                {
                    if (this.board[0][2] != Piece.Empty)
                    {
                        winner = this.board[0][2];
                    }
                }

                if (this.board[0][0] == this.board[1][1] && this.board[1][1] == this.board[2][2])
                {
                    if (this.board[0][0] != Piece.Empty)
                    {
                        winner = this.board[0][0];
                    }
                }

                if (this.board[0][2] == this.board[1][1] && this.board[1][1] == this.board[2][0])
                {
                    if (this.board[0][2] != Piece.Empty)
                    {
                        winner = this.board[0][2];
                    }
                }

                if (winner == null)
                {
                    var full = true;
                    for (int i = 0; i < this.board.Length; ++i)
                    {
                        for (int j = 0; j < this.board[i].Length; ++j)
                        {
                            if (this.board[i][j] == Piece.Empty)
                            {
                                full = false;
                            }
                        }
                    }

                    if (full)
                    {
                        return new Outcome<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), this.players.Clone() as TPlayer[]);
                    }
                    else
                    {
                        return null;
                    }
                }

                int winnerIndex;
                if (winner == Piece.X)
                {
                    winnerIndex = 0;
                }
                else
                {
                    winnerIndex = 1;
                }

                return new Outcome<TPlayer>(new[] { this.players[winnerIndex] }, new[] { this.players[(winnerIndex + 1) % 2] }, Enumerable.Empty<TPlayer>());
            }
        }

        public TicTacToe<TPlayer> CommitMove(Placement move)
        {
            Piece piece;
            if (this.currentPlayerIndex == 0)
            {
                piece = Piece.X;
            }
            else
            {
                piece = Piece.O;
            }

            var newData = new Piece[this.board.Length][];
            for (int i = 0; i < newData.Length; ++i)
            {
                newData[i] = new Piece[this.board[i].Length];
                for (int j = 0; j < newData[i].Length; ++j)
                {
                    newData[i][j] = this.board[i][j];
                }
            }

            newData[move.Row][move.Column] = piece;
            return new TicTacToe<TPlayer>(this.players, (this.currentPlayerIndex + 1) % 2, newData);
        }

        private enum Piece
        {
            Empty = 0,
            X = 1,
            O = 2,
        }
    }

    public sealed class PegMove
    {
        public PegMove(Position from, Position to)
        {
            this.From = from;
            this.To = to;
        }

        public Position From { get; }

        public Position To { get; }

        public sealed class Position
        {
            public Position(int row, int column)
            {
                this.Row = row;
                this.Column = column;
            }

            public int Row { get; }

            public int Column { get; }
        }
    }

    public sealed class PegGame<TPlayer> : IGame<PegMove, TPlayer, PegGame<TPlayer>>
    {
        private readonly TPlayer player;

        private Tuple<bool> row1;

        private Tuple<bool, bool> row2;

        private Tuple<bool, bool, bool> row3;

        private Tuple<bool, bool, bool, bool> row4;

        private Tuple<bool, bool, bool, bool, bool> row5;

        private readonly bool[,] data;

        public PegGame(TPlayer player)
        {
            this.player = player;

            //// TODO randomize these?
            this.row1 = Tuple.Create(false);
            this.row2 = Tuple.Create(true, true);
            this.row3 = Tuple.Create(true, true, true);
            this.row4 = Tuple.Create(true, true, true, true);
            this.row5 = Tuple.Create(true, true, true, true, true);

            this.data = new bool[5, 5];
            this.data[0, 0] = false;
            this.data[1, 0] = true; this.data[1, 1] = true;
            this.data[2, 0] = true; this.data[2, 1] = true; this.data[2, 2] = true;
            this.data[3, 0] = true; this.data[3, 1] = true; this.data[3, 2] = true; this.data[3, 3] = true;
            this.data[4, 0] = true; this.data[4, 1] = true; this.data[4, 2] = true; this.data[4, 3] = true; this.data[4, 4] = true;
        }

        private PegGame(bool[,] data, TPlayer player)
        {
            this.data = data;
            this.player = player;
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.player;
            }
        }

        public IEnumerable<PegMove> LegalMoves
        {
            get
            {
                var totalLength = 5;
                for (int i = 0; i < 5; ++i)
                {
                    var rowLength = i + 1;
                    for (int j = 0; j < rowLength; ++j)
                    {
                        if (data[i, j])
                        {
                            if (i - 1 >= 0 && j - 1 >= 0 && j - 1 < rowLength - 1 &&
                                i - 2 >= 0 && j - 2 >= 0 && j - 2 < rowLength - 2)
                            {
                                if (data[i - 1, j - 1] && !data[i - 2, j - 2])
                                {
                                    yield return new PegMove(new PegMove.Position(i, j), new PegMove.Position(i - 2, j - 2));
                                }
                            }

                            if (i - 1 >= 0 && j < rowLength - 1 &&
                                i - 2 >= 0 && j < rowLength - 2)
                            {
                                if (data[i - 1, j] && !data[i - 2, j])
                                {
                                    yield return new PegMove(new PegMove.Position(i, j), new PegMove.Position(i - 2, j));
                                }
                            }

                            if (j - 1 >= 0 && j - 2 >= 0)
                            {
                                if (data[i, j - 1] && !data[i, j - 2])
                                {
                                    yield return new PegMove(new PegMove.Position(i, j), new PegMove.Position(i, j - 2));
                                }
                            }

                            if (j + 1 < rowLength && j + 2 < rowLength)
                            {
                                if (data[i, j + 1] && !data[i, j + 2])
                                {
                                    yield return new PegMove(new PegMove.Position(i, j), new PegMove.Position(i, j + 2));
                                }
                            }

                            if (i + 1 < totalLength && i + 2 < totalLength)
                            {
                                if (data[i + 1, j] && !data[i + 2, j])
                                {
                                    yield return new PegMove(new PegMove.Position(i, j), new PegMove.Position(i + 2, j));
                                }
                            }

                            if (i + 1 < totalLength && i + 2 < totalLength && j + 1 < rowLength + 1 && j + 2 < rowLength + 2)
                            {
                                if (data[i + 1, j + 1] && !data[i + 2, j + 2])
                                {
                                    yield return new PegMove(new PegMove.Position(i, j), new PegMove.Position(i + 2, j + 2));
                                }
                            }
                        }
                    }
                }

                /*if (this.row1.Item1)
                {
                    if (this.row2.Item1 && !this.row3.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(0, 0), new PegMove.Position(2, 0));
                    }

                    if (this.row2.Item2 && !this.row3.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(0, 0), new PegMove.Position(2, 2));
                    }
                }

                if (this.row2.Item1)
                {
                    if (this.row3.Item1 && !this.row4.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(1, 0), new PegMove.Position(3, 0));
                    }

                    if (this.row3.Item2 && !this.row4.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(1, 0), new PegMove.Position(3, 2));
                    }
                }

                if (this.row2.Item2)
                {
                    if (this.row3.Item2 && !this.row4.Item2)
                    {
                        yield return new PegMove(new PegMove.Position(1, 1), new PegMove.Position(3, 1));
                    }

                    if (this.row3.Item3 && !this.row4.Item4)
                    {
                        yield return new PegMove(new PegMove.Position(1, 1), new PegMove.Position(3, 3));
                    }
                }

                if (this.row3.Item1)
                {
                    if (this.row2.Item1 && !this.row1.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(2, 0), new PegMove.Position(0, 0));
                    }

                    if (this.row3.Item2 && !this.row3.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(2, 0), new PegMove.Position(2, 2));
                    }
                    
                    if (this.row4.Item1 && !this.row5.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(2, 0), new PegMove.Position(4, 0));
                    }

                    if (this.row4.Item2 && !this.row5.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(2, 0), new PegMove.Position(4, 2));
                    }
                }

                if (this.row3.Item2)
                {
                    if (this.row4.Item2 && !this.row5.Item2)
                    {
                        yield return new PegMove(new PegMove.Position(2, 1), new PegMove.Position(4, 1));
                    }

                    if (this.row4.Item3 && !this.row5.Item4)
                    {
                        yield return new PegMove(new PegMove.Position(2, 1), new PegMove.Position(4, 3));
                    }
                }

                if (this.row3.Item3)
                {
                    if (this.row2.Item2 && !this.row1.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(2, 2), new PegMove.Position(0, 0));
                    }

                    if (this.row3.Item2 && !this.row3.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(2, 2), new PegMove.Position(2, 0));
                    }

                    if (this.row4.Item3 && !this.row5.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(2, 2), new PegMove.Position(4, 2));
                    }

                    if (this.row4.Item4 && !this.row5.Item5)
                    {
                        yield return new PegMove(new PegMove.Position(2, 2), new PegMove.Position(4, 4));
                    }
                }

                if (this.row4.Item1)
                {
                    if (this.row3.Item1 && !this.row2.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(3, 0), new PegMove.Position(1, 0));
                    }

                    if (this.row4.Item2 && !this.row4.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(3, 0), new PegMove.Position(3, 2));
                    }
                }

                if (this.row4.Item2)
                {
                    if (this.row3.Item2 && !this.row2.Item2)
                    {
                        yield return new PegMove(new PegMove.Position(3, 1), new PegMove.Position(1, 1));
                    }

                    if (this.row4.Item3 && !this.row4.Item4)
                    {
                        yield return new PegMove(new PegMove.Position(3, 1), new PegMove.Position(3, 3));
                    }
                }

                if (this.row4.Item3)
                {
                    if (this.row3.Item2 && !this.row2.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(3, 2), new PegMove.Position(1, 0));
                    }

                    if (this.row4.Item2 && !this.row4.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(3, 2), new PegMove.Position(3, 0));
                    }
                }

                if (this.row4.Item4)
                {
                    if (this.row3.Item3 && !this.row2.Item2)
                    {
                        yield return new PegMove(new PegMove.Position(3, 3), new PegMove.Position(1, 1));
                    }

                    if (this.row4.Item3 && !this.row4.Item2)
                    {
                        yield return new PegMove(new PegMove.Position(3, 3), new PegMove.Position(3, 1));
                    }
                }

                if (this.row5.Item1)
                {
                    if (this.row4.Item1 && !this.row3.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(4, 0), new PegMove.Position(2, 0));
                    }

                    if (this.row5.Item2 && !this.row5.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(4, 0), new PegMove.Position(4, 2));
                    }
                }

                if (this.row5.Item2)
                {
                    if (this.row4.Item2 && !this.row3.Item2)
                    {
                        yield return new PegMove(new PegMove.Position(4, 1), new PegMove.Position(2, 1));
                    }

                    if (this.row5.Item3 && !this.row5.Item4)
                    {
                        yield return new PegMove(new PegMove.Position(4, 1), new PegMove.Position(4, 3));
                    }
                }

                if (this.row5.Item3)
                {
                    if (this.row4.Item2 && !this.row3.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(4, 2), new PegMove.Position(2, 0));
                    }

                    if (this.row4.Item3 && !this.row3.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(4, 2), new PegMove.Position(2, 2));
                    }

                    if (this.row5.Item2 && !this.row5.Item1)
                    {
                        yield return new PegMove(new PegMove.Position(4, 2), new PegMove.Position(4, 0));
                    }

                    if (this.row5.Item4 && !this.row5.Item5)
                    {
                        yield return new PegMove(new PegMove.Position(4, 2), new PegMove.Position(4, 4));
                    }
                }

                if (this.row5.Item4)
                {
                    if (this.row4.Item3 && !this.row3.Item2)
                    {
                        yield return new PegMove(new PegMove.Position(4, 3), new PegMove.Position(2, 1));
                    }

                    if (this.row5.Item3 && !this.row5.Item2)
                    {
                        yield return new PegMove(new PegMove.Position(4, 3), new PegMove.Position(4, 1));
                    }
                }

                if (this.row5.Item5)
                {
                    if (this.row4.Item4 && !this.row3.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(4, 4), new PegMove.Position(2, 2));
                    }

                    if (this.row5.Item4 && !this.row5.Item3)
                    {
                        yield return new PegMove(new PegMove.Position(4, 4), new PegMove.Position(4, 2));
                    }
                }*/
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                if (this.LegalMoves.Any())
                {
                    return null;
                }

                var totalCount = 0;
                for (int i = 0; i < this.data.GetLength(0); ++i)
                {
                    for ( int j = 0; j < i + 1; ++j)
                    {
                        totalCount += this.data[i, j] ? 1 : 0;
                    }
                }

                /*var totalCount =
                    Count(this.row1.Item1) +
                    Count(this.row2.Item1) + Count(this.row2.Item2) +
                    Count(this.row3.Item1) + Count(this.row3.Item2) + Count(this.row3.Item3) +
                    Count(this.row4.Item1) + Count(this.row4.Item2) + Count(this.row4.Item3) + Count(this.row4.Item4) +
                    Count(this.row5.Item1) + Count(this.row5.Item2) + Count(this.row5.Item3) + Count(this.row5.Item4) + Count(this.row5.Item5);*/
                if (totalCount == 1)
                {
                    return new Outcome<TPlayer>(new[] { this.player }, Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
                }
                else
                {
                    return new Outcome<TPlayer>(Enumerable.Empty<TPlayer>(), new[] { this.player }, Enumerable.Empty<TPlayer>());
                }
            }
        }

        private static int Count(bool value)
        {
            return value ? 1 : 0;
        }

        public void Display()
        {
            Func<bool, int> stuff = (val) => val ? 1 : 0;
            Console.WriteLine("    {0}", stuff(this.data[0, 0]));
            Console.WriteLine("   {0} {1}", stuff(this.data[1, 0]), stuff(this.data[1, 1]));
            Console.WriteLine("  {0} {1} {2}", stuff(this.data[2, 0]), stuff(this.data[2, 1]), stuff(this.data[2, 2]));
            Console.WriteLine(" {0} {1} {2} {3}", stuff(this.data[3, 0]), stuff(this.data[3, 1]), stuff(this.data[3, 2]), stuff(this.data[3, 3]));
            Console.WriteLine("{0} {1} {2} {3} {4}", stuff(this.data[4, 0]), stuff(this.data[4, 1]), stuff(this.data[4, 2]), stuff(this.data[4, 3]), stuff(this.data[4, 4]));
        }

        public PegGame<TPlayer> CommitMove(PegMove move)
        {
            //// TODO check for legality
            var newData = this.data.Clone() as bool[,];
            newData[move.From.Row, move.From.Column] = false;
            newData[move.From.Row + (move.To.Row - move.From.Row) / 2, move.From.Column + (move.To.Column - move.From.Column) / 2] = false;
            newData[move.To.Row, move.To.Column] = true;
            return new PegGame<TPlayer>(newData, this.player);
        }
    }

    public sealed class Outcome<TPlayer>
    {
        public Outcome(IEnumerable<TPlayer> winners, IEnumerable<TPlayer> losers, IEnumerable<TPlayer> draws)
        {
            this.Winners = winners;
            this.Losers = losers;
            this.Draws = draws;
        }

        public IEnumerable<TPlayer> Winners { get; }

        public IEnumerable<TPlayer> Losers { get; }

        public IEnumerable<TPlayer> Draws { get; }
    }

    public interface IGame<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        IEnumerable<TMove> LegalMoves { get; }

        TGame CommitMove(TMove move);

        TPlayer CurrentPlayer { get; }

        Outcome<TPlayer> Outcome { get; }

        void Display();
    }

    public interface IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        TMove SelectMove(TGame game);
    }

    public sealed class RandomStrategy<TMove, TPlayer, TGame> : IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        private readonly Random random;

        public RandomStrategy(Random random)
        {
            this.random = random;
        }

        public TMove SelectMove(TGame game)
        {
            System.Threading.Thread.Sleep(500);
            var movesList = game.LegalMoves.ToList();
            var moveIndex = random.Next(0, movesList.Count);
            return movesList[moveIndex];
        }
    }

    public sealed class ConsoleStrategy<TMove, TPlayer, TGame> : IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        private readonly Func<TMove, string> displayMove;

        public ConsoleStrategy(Func<TMove, string> displayMove)
        {
            this.displayMove = displayMove;
        }

        public TMove SelectMove(TGame game)
        {
            var movesList = game.LegalMoves.ToList();
            for (int i = 0; i < movesList.Count; ++i)
            {
                Console.WriteLine($"{i}: {displayMove(movesList[i])}");
            }

            int index;
            var moveSelected = false;
            do
            {
                Console.Write("Select move number: ");
                var indexInput = Console.ReadLine();
                if (!int.TryParse(indexInput, out index) || index < 0 || index >= movesList.Count)
                {
                    ////Console.WriteLine("Invalid input! Please enter a number corresponding to a move.");
                    moveSelected = true;
                }
                else
                {
                    moveSelected = true;
                }
            }
            while (!moveSelected);

            return movesList[index];
        }
    }

    public sealed class GameRunner<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        private readonly TGame game;

        private readonly IReadOnlyDictionary<TPlayer, IStrategy<TMove, TPlayer, TGame>> strategies;

        public GameRunner(TGame game, IReadOnlyDictionary<TPlayer, IStrategy<TMove, TPlayer, TGame>> strategies)
        {
            this.game = game;
            this.strategies = strategies;
        }

        public Outcome<TPlayer> Start()
        {
            var games = new List<TGame>();
            var game = this.game;
            games.Add(game);
            game.Display();
            while (game.Outcome == null)
            {
                var moves = game.LegalMoves;
                var strategy = this.strategies[game.CurrentPlayer];
                var move = strategy.SelectMove(game);
                game = game.CommitMove(move);
                games.Add(game);
                ////System.Threading.Thread.Sleep(1000);
                game.Display();
            }

            /*Console.WriteLine("starting game...");
            Console.ReadLine();
            foreach (var gamestate in games)
            {
                gamestate.Display();
                System.Threading.Thread.Sleep(1000);
            }*/

            return game.Outcome;
        }
    }
}
