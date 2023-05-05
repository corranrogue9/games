namespace Fx.Game
{
    using System;

    using Fx.Strategy;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public static class MaximizeMovesStrategy
    {
        public static MaximizeMovesStrategy<TGame, TBoard, TMove, TPlayer> Default<TGame, TBoard, TMove, TPlayer>() where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return MaximizeMovesStrategy<TGame, TBoard, TMove, TPlayer>.Default;
        }
    }
}
