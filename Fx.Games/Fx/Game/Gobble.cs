namespace Fx.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Game.Gobble;

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
            board = newBoard;
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return players[currentPlayer];
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
                        var current = board.Grid[i, j];
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
                return board;
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (board.Grid[i, 0].HasValue)
                    {
                        bool win = true;
                        for (int j = 1; j < 3; ++j)
                        {
                            win = win && ArePiecesSamePlayer(board.Grid[i, 0], board.Grid[i, j]);
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(board.Grid[i, 0].Value) });
                        }
                    }
                }

                for (int j = 0; j < 3; ++j)
                {
                    if (board.Grid[0, j].HasValue)
                    {
                        bool win = true;
                        for (int i = 1; i < 3; ++i)
                        {
                            win = win && ArePiecesSamePlayer(board.Grid[0, j], board.Grid[i, j]);
                        }

                        if (win)
                        {
                            return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(board.Grid[0, j].Value) });
                        }
                    }
                }

                if (board.Grid[0, 0].HasValue && ArePiecesSamePlayer(board.Grid[0, 0], board.Grid[1, 1]) && ArePiecesSamePlayer(board.Grid[0, 0], board.Grid[2, 2]))
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(board.Grid[1, 1].Value) });
                }

                if (board.Grid[2, 0].HasValue && ArePiecesSamePlayer(board.Grid[2, 0], board.Grid[1, 1]) && ArePiecesSamePlayer(board.Grid[2, 0], board.Grid[0, 2]))
                {
                    return new Outcome<TPlayer>(new[] { GetPlayerFromPiece(board.Grid[1, 1].Value) });
                }

                return Moves.Any() ? null : new Outcome<TPlayer>(Enumerable.Empty<TPlayer>());
            }
        }

        public Gobble<TPlayer> CommitMove(GobbleMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board.Grid[move.Row, move.Column].HasValue && board.Grid[move.Row, move.Column].Value.Size >= move.Size)
            {
                throw new IllegalMoveExeption("TODO");
            }

            // TODO: does clone work here?
            var newBoard = board.Grid.Clone() as GobblePiece?[,];
            // TODO ensure that GobbleColor cast works
            newBoard[move.Row, move.Column] = new GobblePiece(move.Size, (GobbleColor)currentPlayer);

            return new Gobble<TPlayer>(players, (currentPlayer + 1) % 2, new GobbleBoard(newBoard));
        }

        private TPlayer GetPlayerFromPiece(GobblePiece piece)
        {
            return players[(int)piece.Color];
        }

        private static bool ArePiecesSamePlayer(GobblePiece? first, GobblePiece? second)
        {
            return second.HasValue && first.Value.Color == second.Value.Color;
        }
    }
}
