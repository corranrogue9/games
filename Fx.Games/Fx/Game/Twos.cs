using Fx.Game;
using Fx.Game;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public sealed class RandomStrategy<TMove, TPlayer, TGame> : IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        private readonly Random random;

        public RandomStrategy(Random random)
        {
            this.random = random;
        }

        public TMove SelectMove(TGame game)
        {
            ////System.Threading.Thread.Sleep(500);
            var movesList = game.LegalMoves.ToList();
            var moveIndex = random.Next(0, movesList.Count);
            return movesList[moveIndex];
        }
    }

    public static class Main
    {
        private static double Heuristic1(Twos<string> game)
        {
            return game.LegalMoves.Count();
        }

        private static double Heuristic2(Twos<string> game)
        {
            return game.Max;
        }

        private static double Heuristic3(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            return max * 100 + free;
        }

        private static double Heuristic4(Twos<string> game)
        {
            //// best so far
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            return (game.Board[0][0] == max ? 100000 : 0) + max * 100 + free;
        }

        private static double Heuristic5(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            var corner =
                game.Board[0][0] == max ||
                game.Board[0][3] == max ||
                game.Board[3][0] == max ||
                game.Board[3][3] == max;

            return (corner ? 100000 : 0) + max * 100 + free;
        }

        private static double Heuristic6(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            var corner =
                game.Board[0][0] == max ||
                game.Board[0][3] == max ||
                game.Board[3][0] == max ||
                game.Board[3][3] == max;

            return (corner ? 100000 : 0) + free * 100 + max;
        }

        private static double Heuristic7(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            return free * 100 + max;
        }

        private static double Heuristic8(Twos<string> game)
        {
            // this is intentionally bad
            var sum = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    sum += column;
                }
            }

            return sum;
        }

        private static double Heuristic9(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            var combine = 0;
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    var value = game.Board[i][j];
                    if (value > max)
                    {
                        max = value;
                    }

                    if (value == 0)
                    {
                        ++free;
                    }

                    if (j != 0)
                    {
                        if (game.Board[i][j - 1] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }

                    if (i != 0)
                    {
                        if (game.Board[i - 1][j] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }
                }
            }

            return free * 10000 + combine * 100 + max;
        }

        private static double Heuristic10(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            var combine = 0;
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    var value = game.Board[i][j];
                    if (value > max)
                    {
                        max = value;
                    }

                    if (value == 0)
                    {
                        ++free;
                    }

                    if (j != 0)
                    {
                        if (game.Board[i][j - 1] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }

                    if (i != 0)
                    {
                        if (game.Board[i - 1][j] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }
                }
            }

            return combine * 10000 + free * 100 + max;
        }

        private static double Heuristic11(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            var combine = 0;
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    var value = game.Board[i][j];
                    if (value > max)
                    {
                        max = value;
                    }

                    if (value == 0)
                    {
                        ++free;
                    }

                    if (j != 0)
                    {
                        if (game.Board[i][j - 1] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }

                    if (i != 0)
                    {
                        if (game.Board[i - 1][j] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }
                }
            }

            return max * 10000 + free * 100 + combine;
        }

        private static double Heuristic12(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            var combine = 0;
            var position = (0, 0);
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    var value = game.Board[i][j];
                    if (value > max)
                    {
                        max = value;
                        position = (i, j);
                    }

                    if (value == 0)
                    {
                        ++free;
                    }

                    if (j != 0)
                    {
                        if (game.Board[i][j - 1] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }

                    if (i != 0)
                    {
                        if (game.Board[i - 1][j] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }
                }
            }

            return max * (position.Item1 + 1) * (position.Item2 + 1) * 10000 + free * 100 + combine;
        }

        public static void DoWork()
        {
            //// TODO do other display mechanism to formalize "board state"
            //// TODO do game of amazons to fix decision tree with monte carlo simulation
            //// TODO do more games
            Func<Direction, string> displayMove = (move) =>
            {
                return move.ToString();
            };
            var ticks = Environment.TickCount;
            ////var ticks = 551040140;
            ////var ticks = 1071300156;
            File.WriteAllText(@"c:\users\gdebruin\desktop\ticks.txt", ticks.ToString());
            var random = new Random(ticks);
            var consolestat = new ConsoleStrategy<Direction, string, Twos<string>>(displayMove);
            var results = new GameRunner<Direction, string, Twos<string>>(
                new Twos<string>("gdebruin", new Random(ticks), 12),
                new Dictionary<string, IStrategy<Direction, string, Twos<string>>>
                {
                    ////{ "gdebruin", new ConsoleStrategy<Direction, string, Twos<string>>(displayMove)},
                    ////{ "brett", new BrettStrategy(consolestat) },
                    { "gdebruin", new GarrettGameTreeDepthStrategy<Twos<string>, Direction, string>(Heuristic12, Fx.Tree.Node.TreeFactory, "gdebruin", StringComparer.OrdinalIgnoreCase)},
                    ////{ "gdebruin", new RandomStrategy<Direction, string, Twos<string>>(random) }
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

            /*foreach (var loser in results.Losers)
            {
                Console.WriteLine($"{loser} loses...");
            }*/

            foreach (var winner in results.Winners)
            {
                Console.WriteLine($"{winner} wins!");
            }

            Console.WriteLine(ticks);
            Console.ReadLine();
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
                /*for (int i = 0; i < this.board.Length; ++i)
                {
                    for (int j = 0; j < this.board[i].Length; ++j)
                    {
                        if (this.board[i][j] == this.win)
                        {
                            return new Outcome<TPlayer>(new[] { this.player }, Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
                        }
                    }
                }*/

                if (!this.LegalMoves.Any())
                {
                    return new Outcome<TPlayer>(Enumerable.Empty<TPlayer>());
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
                        Console.Write("   0 ");
                    }
                    else
                    {
                        Console.Write($"{1 << this.board[i][j], 4} ");
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("----------------");
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

    

    public interface IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        TMove SelectMove(TGame game);
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