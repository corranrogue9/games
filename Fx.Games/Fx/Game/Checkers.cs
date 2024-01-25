namespace Fx.Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class Checkers<TPlayer> : IGame<Checkers<TPlayer>, CheckerBoard, CheckerMove, TPlayer>
    {
        private readonly CheckerBoard board;

        private readonly TPlayer white;

        private readonly TPlayer black;

        public Checkers(TPlayer white, TPlayer black)
            : this(white, black, new CheckerBoard())
        {
        }

        private Checkers(TPlayer white, TPlayer black, CheckerBoard board)
        {
            this.white = white;
            this.black = black;
            this.board = board;
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.white; //// TODO this isn't right
            }
        }

        public CheckerBoard Board
        {
            get
            {
                return this.board;
            }
        }

        public IEnumerable<CheckerMove> Moves
        {
            get
            {
                //// TODO what about double jumps?
                foreach (var piece in this.board.Select(position => new { Position = position, Piece = this.board.GetPiece(position) }).Where(piece => piece.Piece != null))
                {
                    //// TODO what about queens?
                    if (piece.Position.Row > 0)
                    {
                        yield return new CheckerMove(piece.Position, new Position(piece.Position.Row - 1, piece.Position.Column + 1));
                    }

                    if (piece.Position.Row < 7)
                    {
                        yield return new CheckerMove(piece.Position, new Position(piece.Position.Row + 1, piece.Position.Column + 1));
                    }

                    //// TODO shouldn't be allowed to move on top of your own piece
                    //// TODO what about jumps?
                }
            }
        }

        public WinnersAndLosers<TPlayer> WinnersAndLosers
        {
            get
            {
                if (!this.board.Select(this.board.GetPiece).Where(piece => piece != null && piece.IsBlack).Any())
                {
                    return new WinnersAndLosers<TPlayer>(new[] { this.white }, new[] { this.black }, Enumerable.Empty<TPlayer>());
                }

                if (!this.board.Select(this.board.GetPiece).Where(piece => piece != null && !piece.IsBlack).Any())
                {
                    return new WinnersAndLosers<TPlayer>(new[] { this.black }, new[] { this.white }, Enumerable.Empty<TPlayer>());
                }

                return new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
            }
        }

        public bool IsGameOver
        {
            get
            {
                return this.WinnersAndLosers.Winners.Any() || this.WinnersAndLosers.Losers.Any() || this.WinnersAndLosers.Drawers.Any();
            }
        }

        public Checkers<TPlayer> CommitMove(CheckerMove move)
        {
            //// TODO make queens
            return new Checkers<TPlayer>(this.white, this.black, this.board.CommitMove(move));
        }
    }
}
