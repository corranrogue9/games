namespace Fx.Games
{
    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class UndoableMove<TMove>
    {
        private readonly bool isUndo;

        private readonly TMove move;

        public UndoableMove(TMove move)
        {
            this.isUndo = false;
            this.move = move;
        }

        private UndoableMove()
        {
            this.isUndo = true;
        }

        public static UndoableMove<TMove> Undo { get; } = new UndoableMove<TMove>();

        public TMove Move
        {
            get
            {
                return this.move;
            }
        }

        public bool IsUndo
        {
            get
            {
                return this.isUndo;
            }
        }
    }
}
