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
            this.board = newBoard;
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.players[this.currentPlayer];
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
                        if (this.board.Grid[i, j] == TicTacToePiece.Empty)
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
                return this.board;
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                //// TODO doesn't deal with draws
                /*
                 * *|*|*
*|*|*
*|*|*
Select a move (row, column):
0: 0, 0
1: 0, 1
2: 0, 2
3: 1, 0
4: 1, 1
5: 1, 2
6: 2, 0
7: 2, 1
8: 2, 2

4
*|*|*
*|X|*
*|*|*
Select a move (row, column):
0: 0, 0
1: 0, 1
2: 0, 2
3: 1, 0
4: 1, 2
5: 2, 0
6: 2, 1
7: 2, 2

0
O|*|*
*|X|*
*|*|*
Select a move (row, column):
0: 0, 1
1: 0, 2
2: 1, 0
3: 1, 2
4: 2, 0
5: 2, 1
6: 2, 2

0
O|X|*
*|X|*
*|*|*
Select a move (row, column):
0: 0, 2
1: 1, 0
2: 1, 2
3: 2, 0
4: 2, 1
5: 2, 2

4
O|X|*
*|X|*
*|O|*
Select a move (row, column):
0: 0, 2
1: 1, 0
2: 1, 2
3: 2, 0
4: 2, 2

3
O|X|*
*|X|*
X|O|*
Select a move (row, column):
0: 0, 2
1: 1, 0
2: 1, 2
3: 2, 2

0
O|X|O
*|X|*
X|O|*
Select a move (row, column):
0: 1, 0
1: 1, 2
2: 2, 2

0
O|X|O
X|X|*
X|O|*
Select a move (row, column):
0: 1, 2
1: 2, 2

0
O|X|O
X|X|O
X|O|*
Select a move (row, column):
0: 2, 2

0
O|X|O
X|X|O
X|O|X
Select a move (row, column):
                 */
                for (int i = 0; i < 3; ++i)
                {
                    if (this.board.Grid[i, 0] != TicTacToePiece.Empty)
                    {
                        bool win = true;
                        for (int j = 1; j < 3; ++j)
                        {
                            win = win && this.board.Grid[i, 0] == this.board.Grid[i, j];
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[i, 0]) });
                        }
                    }
                }

                for (int j = 0; j < 3; ++j)
                {
                    if (this.board.Grid[0, j] != TicTacToePiece.Empty)
                    {
                        bool win = true;
                        for (int i = 1; i < 3; ++i)
                        {
                            win = win && this.board.Grid[0, j] == this.board.Grid[i, j];
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[0, j]) });
                        }
                    }
                }

                if (this.board.Grid[0, 0] != TicTacToePiece.Empty && this.board.Grid[0, 0] == this.board.Grid[1, 1] && this.board.Grid[0, 0] == this.board.Grid[2, 2])
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[1, 1]) });
                }

                if (this.board.Grid[2, 0] != TicTacToePiece.Empty && this.board.Grid[2, 0] == this.board.Grid[1, 1] && this.board.Grid[2, 0] == this.board.Grid[0, 2])
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[1, 1]) });
                }

                return null!;
            }
        }

        public TicTacToe<TPlayer> CommitMove(TicTacToeMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (this.board.Grid[move.Row, move.Column] != TicTacToePiece.Empty)
            {
                throw new IllegalMoveExeption("TODO");
            }

            // TODO: does clone work here?
            var newBoard = this.board.Grid.Copy();
            newBoard[move.Row, move.Column] = (TicTacToePiece)(this.currentPlayer + 1);

            return new TicTacToe<TPlayer>(this.players, (this.currentPlayer + 1) % 2, new TicTacToeBoard(newBoard));
        }

        private TPlayer GetPlayerFromPiece(TicTacToePiece piece)
        {
            return this.players[(int)(piece) - 1];
        }
    }
}
