namespace Fx.Games
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public static class MinimizeMovesStrategy
    {
        public static MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer> Default<TGame, TBoard, TMove, TPlayer>() where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer>.Default;
        }
    }
}
