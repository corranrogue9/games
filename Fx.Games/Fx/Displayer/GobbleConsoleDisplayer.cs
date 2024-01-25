namespace Fx.Displayer
{
    using System;
    using System.Linq;

    using Fx.Game;

    public sealed class GobbleConsoleDisplayer<TPlayer> : IDisplayer<Gobble<TPlayer>, GobbleBoard, GobbleMove, TPlayer>
    {
        private readonly Func<TPlayer, string> playerToString;

        public GobbleConsoleDisplayer(Func<TPlayer, string> playerToString)
        {
            if (playerToString == null)
            {
                throw new ArgumentNullException(nameof(playerToString));
            }

            this.playerToString = playerToString;
        }

        private static char FromPiece(Nullable<GobblePiece> piece)
        {
            if (!piece.HasValue)
            {
                return '_';
            }

            var ix = piece.Value.Color == GobbleColor.Orange ? 'a' : '1';
            return (char)(ix + (int)piece.Value.Size);
        }

        public void DisplayBoard(Gobble<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    Console.Write($"{FromPiece(game.Board.Grid[i, j])}|");
                }

                Console.Write($"{FromPiece(game.Board.Grid[i, 2])}");

                Console.WriteLine();
            }
        }

        public void DisplayOutcome(Gobble<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            foreach (var winner in game.WinnersAndLosers.Winners)
            {
                Console.WriteLine($"{this.playerToString(winner)} wins!");
            }
        }

        public void DisplayMoves(Gobble<TPlayer> game)
        {
            Console.WriteLine("Select a move (row, column, size):");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: {move.Row}, {move.Column}, {move.Size}");
            }

            Console.WriteLine();
        }

        public GobbleMove ReadMoveSelection(Gobble<TPlayer> game)
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
