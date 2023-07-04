namespace Fx.Game.Chess
{
    using System;
    using System.Collections.Generic;

    public sealed class ChessMove { }


    /// <threadsafety static="true" instance="true"/>
    public sealed class Chess<TPlayer> : IGame<Chess<TPlayer>, ChessBoard, ChessMove, TPlayer>
    {
        public Chess(TPlayer white, TPlayer black)
        {
            players = new[] { white, black };

            Board = new ChessBoard();
            CurrentPlayer = players[(int)ChessColor.White];
        }

        private readonly TPlayer[] players;

        public ChessBoard Board { get; }


        public TPlayer CurrentPlayer { get; }

        public IEnumerable<ChessMove> Moves => throw new NotImplementedException();


        public Outcome<TPlayer> Outcome => throw new NotImplementedException();

        public Chess<TPlayer> CommitMove(ChessMove move)
        {
            throw new NotImplementedException();
        }

    }
}
