namespace Fx.Games
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class Driver<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly IEnumerable<IStrategy<TGame, TBoard, TMove, TPlayer>> strategies;

        private readonly IDisplayer<TBoard, TPlayer> displayer;

        public Driver(IEnumerable<IStrategy<TGame, TBoard, TMove, TPlayer>> strategies, IDisplayer<TBoard, TPlayer> displayer)
        {
            this.strategies = strategies.ToList();
            this.displayer = displayer;
        }

        public TGame Run(TGame game)
        {
            while (game.Outcome == null)
            {
                foreach (var strategy in this.strategies)
                {
                    displayer.DisplayBoard(game.Board);
                    var move = strategy.SelectMove(game);
                    game = game.CommitMove(move);
                }
            }

            displayer.DisplayBoard(game.Board);
            displayer.DisplayOutcome(game.Outcome);
            return game;
        }
    }
}
