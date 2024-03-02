namespace Fx.Driver
{
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Displayer;
    using Fx.Game;
    using Fx.Strategy;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class Driver<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies;

        private readonly IDisplayer<TGame, TBoard, TMove, TPlayer> displayer;

        public Driver(IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies, IDisplayer<TGame, TBoard, TMove, TPlayer> displayer)
        {
            this.strategies = strategies.ToDictionary();
            this.displayer = displayer;
        }

        public TGame Run(TGame game)
        {
            while (game.Outcome == null)
            {
                var strategy = strategies[game.CurrentPlayer];
                displayer.DisplayBoard(game);
                displayer.DisplayMoves(game);
                var move = strategy.SelectMove(game);

                if (this.displayer is LonghornConsoleDisplay<TPlayer> longhornConsoleDisplayer)
                {
                    Console.WriteLine($"{game.CurrentPlayer} selected {longhornConsoleDisplayer.TranscribeMove(game as Longhorn<TPlayer>, move as LonghornMove)}");
                }
                else
                {
                    Console.WriteLine($"{game.CurrentPlayer} selected {move}"); //// TODO move.tostring cannot be relied on
                }

                game = game.CommitMove(move);
                if (game.Outcome != null)
                {
                    break;
                }
            }

            displayer.DisplayBoard(game);
            displayer.DisplayOutcome(game);
            return game;
        }
    }
}
