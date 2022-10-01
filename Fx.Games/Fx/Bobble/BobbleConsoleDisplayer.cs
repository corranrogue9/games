namespace Fx.Games.Bobble
{
    using System;
    using System.Linq;
    public sealed class BobbleConsoleDisplayer<TPlayer> : IDisplayer<Bobble<TPlayer>, BobbleBoard, BobbleMove, TPlayer>
    {
        private readonly Func<TPlayer, string> playerToString;

        public BobbleConsoleDisplayer(Func<TPlayer, string> playerToString)
        {
            if (playerToString == null)
            {
                throw new ArgumentNullException(nameof(playerToString));
            }

            this.playerToString = playerToString;
        }

        private static char FromPiece(Nullable<BobblePiece> piece)
        {
            if (!piece.HasValue)
            {
                return '_';
            }

            var ix = piece.Value.Color == BobbleColor.Orange ? 'a' : '1';
            return (char)(ix + (int)piece.Value.Size);
        }

        public void DisplayBoard(Bobble<TPlayer> game)
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

        public void DisplayOutcome(Bobble<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            foreach (var winner in game.Outcome.Winners)
            {
                Console.WriteLine($"{this.playerToString(winner)} wins!");
            }
        }

        public void DisplayMoves(Bobble<TPlayer> game)
        {
            Console.WriteLine("Select a move (row, column):");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: {move.Row}, {move.Column}");
            }

            Console.WriteLine();
        }

        public BobbleMove ReadMoveSelection(Bobble<TPlayer> game)
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
