namespace Fx.Strategy
{
    using System;

    using Fx.Displayer;
    using Fx.Game;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class UserInterfaceStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly IDisplayer<TGame, TBoard, TMove, TPlayer> displayer;

        public UserInterfaceStrategy(IDisplayer<TGame, TBoard, TMove, TPlayer> displayer)
        {
            if (displayer == null)
            {
                throw new ArgumentNullException(nameof(displayer));
            }

            this.displayer = displayer;
        }

        public TMove SelectMove(TGame game)
        {
            return this.displayer.ReadMoveSelection(game);
        }
    }
}
