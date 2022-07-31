namespace Fx.Games.TicTacToe
{
    using System;

    public sealed class TicTacToeConsoleDisplayer<TPlayer> : IDisplayer<TicTacToeBoard, TPlayer>
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

        public void DisplayBoard(TicTacToeBoard board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    Console.Write($"{FromPiece(board.Board[i, j], i == 2)}|");
                }

                Console.Write($"{FromPiece(board.Board[i, 2], i == 2)}");

                Console.WriteLine();
            }
        }

        public void DisplayOutcome(Outcome<TPlayer> outcome)
        {
            if (outcome == null)
            {
                throw new ArgumentNullException(nameof(outcome));
            }

            foreach (var winner in outcome.Winners)
            {
                Console.WriteLine($"{this.playerToString(winner)} wins!");
            }
        }

        private static char FromPiece(TicTacToePiece piece, bool bottom)
        {
            switch (piece)
            {
                case TicTacToePiece.Empty:
                    return bottom ? ' ' : '_';
                case TicTacToePiece.Ex:
                    return 'X';
                case TicTacToePiece.Oh:
                    return 'O';
            }

            throw new InvalidOperationException("TODO");
        }
    }
}
