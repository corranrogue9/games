namespace Fx.Games.TicTacToe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class TicTacToe<TPlayer> : IGame<TicTacToe<TPlayer>, TicTacToeBoard, TicTacToeMove, TPlayer>
    {
        private readonly TPlayer exes;

        private readonly TPlayer ohs;

        private readonly TicTacToePiece currentPiece;

        private readonly TicTacToeBoard board;

        public TicTacToe(TPlayer exes, TPlayer ohs)
            : this(EnsureInline.NotNull(exes, nameof(exes)), EnsureInline.NotNull(ohs, nameof(ohs)), TicTacToePiece.Ex, new TicTacToeBoard())
        {
        }

        private TicTacToe(TPlayer exes, TPlayer ohs, TicTacToePiece newPiece, TicTacToeBoard newBoard)
        {
            this.exes = exes;
            this.ohs = ohs;
            this.currentPiece = newPiece;
            this.board = newBoard;
        }

        public IEnumerable<TicTacToeMove> Moves
        {
            get
            {
                for (uint i = 0; i < 3; ++i)
                {
                    for (uint j = 0; j < 3; ++j)
                    {
                        if (this.board.Board[i, j] == TicTacToePiece.Empty)
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
                for (int i = 0; i < 3; ++i)
                {
                    if (this.board.Board[i, 0] != TicTacToePiece.Empty)
                    {
                        bool win = true;
                        for (int j = 1; j < 3; ++j)
                        {
                            win = win && this.board.Board[i, 0] == this.board.Board[i, j];
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Board[i, 0]) });
                        }
                    }
                }

                for (int j = 0; j < 3; ++j)
                {
                    if (this.board.Board[0, j] != TicTacToePiece.Empty)
                    {
                        bool win = true;
                        for (int i = 1; i < 3; ++i)
                        {
                            win = win && this.board.Board[0, j] == this.board.Board[i, j];
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Board[0, j]) });
                        }
                    }
                }

                if (this.board.Board[0, 0] != TicTacToePiece.Empty && this.board.Board[0, 0] == this.board.Board[1, 1] && this.board.Board[0, 0] == this.board.Board[2, 2])
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Board[1, 1]) });
                }

                if (this.board.Board[2, 0] != TicTacToePiece.Empty && this.board.Board[0, 0] == this.board.Board[1, 1] && this.board.Board[0, 0] == this.board.Board[0, 2])
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Board[1, 1]) });
                }

                return null;
            }
        }

        public TicTacToe<TPlayer> CommitMove(TicTacToeMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (this.board.Board[move.Row, move.Column] != TicTacToePiece.Empty)
            {
                throw new IllegalMoveExeption("TODO");
            }

            var newBoard = this.board.Board;
            newBoard[move.Row, move.Column] = this.currentPiece;

            return new TicTacToe<TPlayer>(this.exes, this.ohs, this.currentPiece == TicTacToePiece.Ex ? TicTacToePiece.Oh : TicTacToePiece.Ex, new TicTacToeBoard(newBoard));
        }

        private TPlayer GetPlayerFromPiece(TicTacToePiece piece)
        {
            switch (piece)
            {
                case TicTacToePiece.Ex:
                    return this.exes;
                case TicTacToePiece.Oh:
                    return this.ohs;
            }

            throw new InvalidOperationException("TODO");
        }
    }
}
