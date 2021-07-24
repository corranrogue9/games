namespace Fx.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class UndoableGame<TGame, TBoard, TMove, TPlayer> : IGame<TGame, TBoard, UndoableMove<TMove>, TPlayer> where TGame : IGame<TGame, TBoard, UndoableMove<TMove>, TPlayer>
    {
        private readonly TGame currentGame;

        private readonly TGame previousGame;

        public UndoableGame(TGame currentGame, TGame previousGame)
        {
            this.currentGame = currentGame;
            this.previousGame = previousGame;
        }

        public TBoard Board
        {
            get
            {
                return this.currentGame.Board;
            }
        }

        public IEnumerable<UndoableMove<TMove>> Moves
        {
            get
            {
                return this.currentGame.Moves.Concat(new[] { UndoableMove<TMove>.Undo });
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                return this.currentGame.Outcome;
            }
        }

        public TGame CommitMove(UndoableMove<TMove> move)
        {
            if (move.IsUndo)
            {
                return this.previousGame;
            }

            return new UndoableGame<TGame, TBoard, TMove, TPlayer>(this.currentGame.CommitMove(move), this.currentGame);
        }
    }
}
