namespace Fx.Games
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class Amazons<TPlayer> : IGame<Amazons<TPlayer>, ChessBoard<AmazonsPiece>, ChessMove, TPlayer>
    {
        private readonly ChessBoard<AmazonsPiece> board;

        public Amazons(int size)
        {
            var pieces = new AmazonsPiece[size][];
            for (int i = 0; i < pieces.Length; ++i)
            {
                pieces[i] = new AmazonsPiece[size];
                for (int j = 0; j < pieces[i].Length; ++j)
                {
                    pieces[i][j] = AmazonsPiece.Empty;
                }
            }

            pieces[0][0] = AmazonsPiece.Player1;
            pieces[size - 1][size - 1] = AmazonsPiece.Player2;

            this.board = new ChessBoard<AmazonsPiece>(pieces);
        }

        public ChessBoard<AmazonsPiece> Board
        {
            get
            {
                return this.board;
            }
        }

        public IEnumerable<ChessMove> Moves
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Amazons<TPlayer> CommitMove(ChessMove move)
        {
            throw new NotImplementedException();
        }
    }
}
