namespace Fx.Games
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Checker
    {
        private Checker(bool black, bool queen)
        {
            this.IsBlack = black;
            this.IsQueen = queen;
        }

        public static Checker BlackQueen { get; } = new Checker(true, true);

        public static Checker Black { get; } = new Checker(true, false);

        public static Checker WhiteQueen { get; } = new Checker(false, true);

        public static Checker White { get; } = new Checker(false, false);

        public bool IsBlack { get; }

        public bool IsQueen { get; }
    }

    public sealed class CheckerBoard : IEnumerable<Position>
    {
        private readonly Checker[,] board;

        public CheckerBoard()
        {
            this.board = new Checker[8, 8];
            for (int i = 0; i < 8; i += 2)
            {
                this.board[0,i] = Checker.White;
            }

            for (int i = 1; i < 8; i += 2)
            {
                this.board[1, i] = Checker.White;
            }

            for (int i = 0; i < 8; i += 2)
            {
                this.board[6, i] = Checker.Black;
            }

            for (int i = 1; i < 8; i += 2)
            {
                this.board[7, i] = Checker.Black;
            }
        }

        private CheckerBoard(Checker[,] board)
        {
            this.board = board;
        }

        public CheckerBoard CommitMove(CheckerMove move)
        {
            var newBoard = this.board.Clone() as Checker[,];
            var movingPiece = newBoard[move.Initial.Row, move.Initial.Column];
            newBoard[move.Initial.Row, move.Initial.Column] = null;
            newBoard[move.Final.Row, move.Final.Column] = movingPiece;
            return new CheckerBoard(newBoard);
        }

        public Checker GetPiece(Position position)
        {
            return this.board[position.Row, position.Column];
        }

        public IEnumerator<Position> GetEnumerator()
        {
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    yield return new Position(i, j);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public sealed class Position
    {
        public Position(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public int Row { get; }

        public int Column { get; }
    }

    public sealed class CheckerMove
    {
        public CheckerMove(Position initial, Position final)
        {
            this.Initial = initial;
            this.Final = final;
        }

        public Position Initial { get; }

        public Position Final { get; }
    }

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

        public Outcome<TPlayer> Outcome
        {
            get
            {
                if (!this.board.Select(this.board.GetPiece).Where(piece => piece != null && piece.IsBlack).Any())
                {
                    return new Outcome<TPlayer>(new[] { this.white });
                }

                if (!this.board.Select(this.board.GetPiece).Where(piece => piece != null && !piece.IsBlack).Any())
                {
                    return new Outcome<TPlayer>(new[] { this.black });
                }

                return null;
            }
        }

        public Checkers<TPlayer> CommitMove(CheckerMove move)
        {
            //// TODO make queens
            return new Checkers<TPlayer>(this.white, this.black, this.board.CommitMove(move));
        }
    }
}
