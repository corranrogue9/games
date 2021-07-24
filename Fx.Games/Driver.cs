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
            IEnumerable<IStrategy<TGame, TBoard, TMove, TPlayer>> strategies,
            IDisplayer<TBoard, TPlayer> displayer)
            where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new Driver<TGame, TBoard, TMove, TPlayer>(strategies, displayer);
        }
    }
}
