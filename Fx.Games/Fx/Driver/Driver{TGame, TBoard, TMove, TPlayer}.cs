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
            while (!game.IsGameOver)
            {
                var strategy = strategies[game.CurrentPlayer];
                displayer.DisplayBoard(game);
                displayer.DisplayMoves(game);
                Console.WriteLine($"{DateTime.UtcNow}");
                var move = strategy.SelectMove(game);
                Console.WriteLine($"{DateTime.UtcNow}");
                game = game.CommitMove(move);
                if (game.IsGameOver)
                {
                    break;
                }
            }

            displayer.DisplayBoard(game);
            displayer.DisplayOutcome(game); //// TODO outcomes now have winners and lsoers
            return game;
        }
    }
}
