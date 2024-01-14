namespace Fx.Displayer
{
    using System;
    using System.Linq;

    using Fx.Game;

    public sealed class TwosConsoleDisplayer<TPlayer> : IDisplayer<Twos<TPlayer>, int[][], TwosDirection, TPlayer>
    {
        private readonly Func<TPlayer, string> playerToString;

        public TwosConsoleDisplayer(Func<TPlayer, string> playerToString)
        {
            if (playerToString == null)
            {
                throw new ArgumentNullException(nameof(playerToString));
            }

            this.playerToString = playerToString;
        }

        public void DisplayBoard(Twos<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            for (int i = 0; i < game.Board.Length; ++i)
            {
                for (int j = 0; j < game.Board[i].Length; ++j)
                {
                    if (game.Board[i][j] == 0)
                    {
                        Console.Write("   0 ");
                    }
                    else
                    {
                        Console.Write($"{1 << game.Board[i][j],4} ");
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("----------------");
        }

        public void DisplayOutcome(Twos<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            if (game.WinnersAndLosers.Losers.Any())
            {
                Console.WriteLine($"{game.WinnersAndLosers.Losers.First()} lost the game; what a loser!");
            }
            else if (game.WinnersAndLosers.Winners.Any())
            {
                Console.WriteLine($"{game.WinnersAndLosers.Winners.First()} won the game; that's amazing because this game never ends!");
            }
        }

        public void DisplayMoves(Twos<TPlayer> game)
        {
            var movesList = game.LegalMoves.ToList();
            for (int i = 0; i < movesList.Count; ++i)
            {
                Console.WriteLine($"{i}: {movesList[i].ToString()}");
            }

            Console.WriteLine();
        }

        public TwosDirection ReadMoveSelection(Twos<TPlayer> game)
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

                return moves[selectedMove];
            }
        }
    }
}
