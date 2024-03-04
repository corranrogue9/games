using System.Linq;

namespace Fx.Game
{
    public sealed class UndoableGame<TGame, TBoard, TMove, TPlayer> : IGame<UndoableGame<TGame, TBoard, TMove, TPlayer>, TBoard, UndoableMove<TMove>, TPlayer>
        where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly TGame game;

        private readonly UndoableGame<TGame, TBoard, TMove, TPlayer>? previous;

        public UndoableGame(TGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            this.game = game;
            this.previous = null; //// TODO it should really undo until the player's last turn
        }

        private UndoableGame(TGame game, UndoableGame<TGame, TBoard, TMove, TPlayer> previous)
        {
            this.game = game;
            this.previous = previous;
        }

        public TGame Game
        {
            get
            {
                return this.game;
            }
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.game.CurrentPlayer;
            }
        }

        public IEnumerable<UndoableMove<TMove>> Moves
        {
            get
            {
                /*return this
                    .game
                    .Moves
                    .Select(move => new UndoableMove<TMove>.Native(move))
                    .Prepend(UndoableMove<TMove>.Undo.Instance);*/ //// TODO never ran into this before...
                if (this.previous != null)
                {
                    yield return UndoableMove<TMove>.Undo.Instance;
                }

                foreach (var element in this.game.Moves.Select(move => new UndoableMove<TMove>.Native(move)))
                {
                    yield return element;
                }
            }
        }

        public TBoard Board
        {
            get
            {
                return this.game.Board;
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                return this.game.Outcome;
            }
        }

        public UndoableGame<TGame, TBoard, TMove, TPlayer> CommitMove(UndoableMove<TMove> move)
        {
            if (move is UndoableMove<TMove>.Native native)
            {
                return new UndoableGame<TGame, TBoard, TMove, TPlayer>(this.game.CommitMove(native.Value), this);
            }
            else
            {
                if (this.previous == null)
                {
                    throw new IllegalMoveExeption("Attempted to undo the first move of the game");
                }

                return this.previous;
            }
        }
    }

    public abstract class UndoableMove<TMove>
    {
        private UndoableMove()
        {
        }

        public sealed class Undo : UndoableMove<TMove>
        {
            private Undo()
            {
            }

            public static Undo Instance { get; } = new Undo();
        }

        public sealed class Native : UndoableMove<TMove>
        {
            public Native(TMove value)
            {
                this.Value = value;
            }

            public TMove Value { get; }
        }
    }
}
