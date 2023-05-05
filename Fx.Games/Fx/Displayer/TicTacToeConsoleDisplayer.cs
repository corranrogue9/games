namespace Fx.Displayer
{
    using System;
    using System.Linq;

    using Fx.Game;
    using Fx.Todo;

    public sealed class TicTacToeConsoleDisplayer<TPlayer> : IDisplayer<TicTacToe<TPlayer>, TicTacToeBoard, TicTacToeMove, TPlayer>
    {
        private readonly Func<TPlayer, string> playerToString;

        public TicTacToeConsoleDisplayer(Func<TPlayer, string> playerToString)
        {
            if (playerToString == null)
            {
                throw new ArgumentNullException(nameof(playerToString));
            }

            this.playerToString = playerToString;
        }

        private static char FromPiece(TicTacToePiece piece)
        {
            switch (piece)
            {
                case TicTacToePiece.Empty:
                    return '*';
                case TicTacToePiece.Ex:
                    return 'X';
                case TicTacToePiece.Oh:
                    return 'O';
            }

            throw new InvalidOperationException("TODO");
        }

        public void DisplayBoard(TicTacToe<TPlayer> game)
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

        public void DisplayOutcome(TicTacToe<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            game.Outcome.Winners.ApplyToEmptyOrPopulated(() => Console.WriteLine("The game was a draw..."), winner => Console.WriteLine($"{this.playerToString(winner)} wins!"));
        }

        public void DisplayMoves(TicTacToe<TPlayer> game)
        {
            Console.WriteLine("Select a move (row, column):");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: {move.Row}, {move.Column}");
            }

            Console.WriteLine();
        }

        public TicTacToeMove ReadMoveSelection(TicTacToe<TPlayer> game)
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
