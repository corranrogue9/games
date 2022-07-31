namespace Fx.Games
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true"/>
    public static class Driver
    {
        public static Driver<TGame, TBoard, TMove, TPlayer> Create<TGame, TBoard, TMove, TPlayer>(
            IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies,
            IDisplayer<TGame, TBoard, TMove, TPlayer> displayer)
            where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new Driver<TGame, TBoard, TMove, TPlayer>(strategies, displayer);
        }
    }
}
