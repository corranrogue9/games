namespace Fx.Game
{
    using System.Collections;
    using System.Collections.Generic;

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
}
