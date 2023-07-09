namespace Fx.Driver
{
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using Fx.Displayer;
    using Fx.Game;
    using Fx.Strategy;

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

        public static HiddenDriver<TGame, TBoard, TMove, TPlayer> CreateHidden<TGame, TBoard, TMove, TPlayer>(
            IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies,
            IDisplayer<TGame, TBoard, TMove, TPlayer> displayer,
            Random random)
            where TGame : IGameWithHiddenInformation<TGame, TBoard, TMove, TPlayer, Distribution<TGame>>
        {
            //// TODO try to run the new sample code with commit specific move
            return new HiddenDriver<TGame, TBoard, TMove, TPlayer>(strategies, displayer, random);
        }
    }
}
