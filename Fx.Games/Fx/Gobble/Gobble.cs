﻿namespace Fx.Games.Gobble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Gobble<TPlayer> : IGame<Gobble<TPlayer>, GobbleBoard, GobbleMove, TPlayer>
    {
        private readonly TPlayer[] players;

        private readonly int currentPlayer;

        private readonly GobbleBoard board;

        public Gobble(TPlayer exes, TPlayer ohs)
            : this(new[] { EnsureInline.NotNull(exes, nameof(exes)), EnsureInline.NotNull(ohs, nameof(ohs)) }, 0, new GobbleBoard())
        {
        }

        private Gobble(TPlayer[] players, int currentPlayer, GobbleBoard newBoard)
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

        public IEnumerable<GobbleMove> Moves
        {
            get
            {
                // TODO: keep track of the number of pieces a player has played of each size.
                // currently it doesn't so a player can play more than 3 of a size. 
                for (uint i = 0; i < 3; ++i)
                {
                    for (uint j = 0; j < 3; ++j)
                    {
                        var current = this.board.Grid[i, j];
                        var size = current.HasValue ? (int)current.Value.Size : -1;
                        for (var s = size; s < 2; s++)
                        {
                            yield return new GobbleMove(i, j, (GobbleSize)(s + 1));
                        }
                    }
                }
            }
        }

        public GobbleBoard Board
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
                    if (this.board.Grid[i, 0].HasValue)
                    {
                        bool win = true;
                        for (int j = 1; j < 3; ++j)
                        {
                            win = win && ArePiecesSamePlayer(this.board.Grid[i, 0], this.board.Grid[i, j]);
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[i, 0].Value) });
                        }
                    }
                }

                for (int j = 0; j < 3; ++j)
                {
                    if (this.board.Grid[0, j].HasValue)
                    {
                        bool win = true;
                        for (int i = 1; i < 3; ++i)
                        {
                            win = win && ArePiecesSamePlayer(this.board.Grid[0, j], this.board.Grid[i, j]);
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[0, j].Value) });
                        }
                    }
                }

                if (this.board.Grid[0, 0].HasValue && ArePiecesSamePlayer(this.board.Grid[0, 0], this.board.Grid[1, 1]) && ArePiecesSamePlayer(this.board.Grid[0, 0], this.board.Grid[2, 2]))
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[1, 1].Value) });
                }

                if (this.board.Grid[2, 0].HasValue && ArePiecesSamePlayer(this.board.Grid[2, 0], this.board.Grid[1, 1]) && ArePiecesSamePlayer(this.board.Grid[2, 0], this.board.Grid[0, 2]))
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(this.board.Grid[1, 1].Value) });
                }

                return this.Moves.Any() ? null : new Outcome<TPlayer>(Enumerable.Empty<TPlayer>());
            }
        }

        public Gobble<TPlayer> CommitMove(GobbleMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (this.board.Grid[move.Row, move.Column].HasValue && this.board.Grid[move.Row, move.Column].Value.Size >= move.Size)
            {
                throw new IllegalMoveExeption("TODO");
            }

            // TODO: does clone work here?
            var newBoard = this.board.Grid.Clone() as GobblePiece?[,];
            // TODO ensure that GobbleColor cast works
            newBoard[move.Row, move.Column] = new GobblePiece(move.Size, (GobbleColor)currentPlayer);

            return new Gobble<TPlayer>(this.players, (this.currentPlayer + 1) % 2, new GobbleBoard(newBoard));
        }

        private TPlayer GetPlayerFromPiece(GobblePiece piece)
        {
            return this.players[(int)(piece.Color)];
        }

        private static bool ArePiecesSamePlayer(GobblePiece? first, GobblePiece? second)
        {
            return second.HasValue && first.Value.Color == second.Value.Color;
        }
    }
}