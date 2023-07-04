namespace Fx.Displayer
{
    using System;
    using System.Linq;

    using Fx.Game;

    public sealed class MiniPokerConsoleDisplayer<TPlayer> : IDisplayer<MiniPoker<TPlayer>, MiniPokerBoard, MiniPokerMove, TPlayer>
    {
        private readonly Func<TPlayer, string> playerToString;

        public MiniPokerConsoleDisplayer(Func<TPlayer, string> playerToString)
        {
            if (playerToString == null)
            {
                throw new ArgumentNullException(nameof(playerToString));
            }

            this.playerToString = playerToString;
        }

        public void DisplayBoard(MiniPoker<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            if (game.Board is MiniPokerBoard.ABoard aBoard)
            {
                Console.WriteLine($"The card is {aBoard.Card}");
            }
            else if (game.Board is MiniPokerBoard.BBoard bBoard)
            {
                Console.Clear();
                ////Console.WriteLine("The card has not been revealed yet");
            }
            else if (game.Board is MiniPokerBoard.PublicBoard publicBoard)
            {
                Console.WriteLine($"The card is {publicBoard.Card}; A's purse is now at {publicBoard.AsPurse}");
            }
        }

        public void DisplayOutcome(MiniPoker<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            game.Outcome.Winners.ApplyToEmptyOrPopulated(() => Console.WriteLine("The game was a draw..."), winner => Console.WriteLine($"{this.playerToString(winner)} wins!"));
        }

        public void DisplayMoves(MiniPoker<TPlayer> game)
        {
            var movesList = game.Moves.ToList();
            for (int i = 0; i < movesList.Count; ++i)
            {
                Console.WriteLine($"{i}: {movesList[i].ToString()}");
            }

            Console.WriteLine();
        }

        public MiniPokerMove ReadMoveSelection(MiniPoker<TPlayer> game)
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
