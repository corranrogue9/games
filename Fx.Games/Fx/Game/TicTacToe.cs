namespace Fx.Games.TicTacToe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class TicTacToe<TPlayer> : IGame<TicTacToe<TPlayer>, TicTacToeBoard, TicTacToeMove, TPlayer>
    {
        private readonly TPlayer[] players;

        private readonly int currentPlayer;

        private readonly TicTacToeBoard board;

        public TicTacToe(TPlayer exes, TPlayer ohs)
            : this(new[] { EnsureInline.NotNull(exes, nameof(exes)), EnsureInline.NotNull(ohs, nameof(ohs)) }, 0, new TicTacToeBoard())
        {
        }

        private TicTacToe(TPlayer[] players, int currentPlayer, TicTacToeBoard newBoard)
        {
            this.players = players;
            this.currentPlayer = currentPlayer;
            board = newBoard;
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return players[currentPlayer];
            }
        }

        public IEnumerable<TicTacToeMove> Moves
        {
            get
            {
                for (uint i = 0; i < 3; ++i)
                {
                    for (uint j = 0; j < 3; ++j)
                    {
                        if (board.Grid[i, j] == TicTacToePiece.Empty)
                        {
                            yield return new TicTacToeMove(i, j);
                        }
                    }
                }
            }
        }

        public TicTacToeBoard Board
        {
            get
            {
                return board;
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (board.Grid[i, 0] != TicTacToePiece.Empty)
                    {
                        bool win = true;
                        for (int j = 1; j < 3; ++j)
                        {
                            win = win && board.Grid[i, 0] == board.Grid[i, j];
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(board.Grid[i, 0]) });
                        }
                    }
                }

                for (int j = 0; j < 3; ++j)
                {
                    if (board.Grid[0, j] != TicTacToePiece.Empty)
                    {
                        bool win = true;
                        for (int i = 1; i < 3; ++i)
                        {
                            win = win && board.Grid[0, j] == board.Grid[i, j];
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(board.Grid[0, j]) });
                        }
                    }
                }

                if (board.Grid[0, 0] != TicTacToePiece.Empty && board.Grid[0, 0] == board.Grid[1, 1] && board.Grid[0, 0] == board.Grid[2, 2])
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(board.Grid[1, 1]) });
                }

                if (board.Grid[2, 0] != TicTacToePiece.Empty && board.Grid[2, 0] == board.Grid[1, 1] && board.Grid[2, 0] == board.Grid[0, 2])
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(board.Grid[1, 1]) });
                }

                foreach (var piece in board.Grid)
                {
                    if (piece == TicTacToePiece.Empty)
                    {
                        // no one has won the game, and there are empty spaces, so the game is continuing
                        return null;
                    }
                }

                // no one has won the game, and there are no empty spaces, so the game is a draw
                return new Outcome<TPlayer>(Enumerable.Empty<TPlayer>());
            }
        }

        public TicTacToe<TPlayer> CommitMove(TicTacToeMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board.Grid[move.Row, move.Column] != TicTacToePiece.Empty)
            {
                throw new IllegalMoveExeption("TODO");
            }

            // TODO: does clone work here?
            var newBoard = board.Grid.Clone() as TicTacToePiece[,];
            newBoard[move.Row, move.Column] = (TicTacToePiece)(currentPlayer + 1);

            return new TicTacToe<TPlayer>(players, (currentPlayer + 1) % 2, new TicTacToeBoard(newBoard));
        }

        private TPlayer GetPlayerFromPiece(TicTacToePiece piece)
        {
            return players[(int)piece - 1];
        }
    }
}
