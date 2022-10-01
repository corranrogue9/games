namespace Fx.Games.Bobble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Bobble<TPlayer> : IGame<Bobble<TPlayer>, BobbleBoard, BobbleMove, TPlayer>
    {
        private readonly TPlayer[] players;

        private readonly int currentPlayer;

        private readonly BobbleBoard board;

        public Bobble(TPlayer exes, TPlayer ohs)
            : this(new[] { EnsureInline.NotNull(exes, nameof(exes)), EnsureInline.NotNull(ohs, nameof(ohs)) }, 0, new BobbleBoard())
        {
        }

        private Bobble(TPlayer[] players, int currentPlayer, BobbleBoard newBoard)
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

        public IEnumerable<BobbleMove> Moves
        {
            get
            {
                for (uint i = 0; i < 3; ++i)
                {
                    for (uint j = 0; j < 3; ++j)
                    {
                        var current = this.board.Grid[i, j];
                        //for var size = 
                        //if (this.board.Grid[i, j] == BobblePiece.Empty)
                        //{
                        //    yield return new BobbleMove(i, j);
                        //}
                    }
                }
            }
        }

        public BobbleBoard Board
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
                for (int i = 0; i < 3; ++i)
                {
                    if (this.board.Grid[i, 0] != BobblePiece.Empty)
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
                    if (this.board.Grid[0, j] != BobblePiece.Empty)
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

                if (this.board.Grid[0, 0] != BobblePiece.Empty && this.board.Grid[0, 0] == this.board.Grid[1, 1] && this.board.Grid[0, 0] == this.board.Grid[2, 2])
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[1, 1]) });
                }

                if (this.board.Grid[2, 0] != BobblePiece.Empty && this.board.Grid[2, 0] == this.board.Grid[1, 1] && this.board.Grid[2, 0] == this.board.Grid[0, 2])
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[1, 1]) });
                }

                return null;
            }
        }

        public Bobble<TPlayer> CommitMove(BobbleMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (this.board.Grid[move.Row, move.Column] != BobblePiece.Empty)
            {
                throw new IllegalMoveExeption("TODO");
            }

            var newBoard = this.board.Grid.Clone() as BobblePiece[,];
            newBoard[move.Row, move.Column] = (BobblePiece)(this.currentPlayer + 1);

            return new Bobble<TPlayer>(this.players, (this.currentPlayer + 1) % 2, new BobbleBoard(newBoard));
        }

        private TPlayer GetPlayerFromPiece(BobblePiece piece)
        {
            return this.players[(int)(piece) - 1];
        }
    }
}
