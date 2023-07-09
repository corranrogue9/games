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

    public sealed class HiddenDriver<TGame, TBoard, TMove, TPlayer> where TGame : IGameWithHiddenInformation<TGame, TBoard, TMove, TPlayer, Distribution<TGame>>
    {
        private readonly IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies;

        private readonly IDisplayer<TGame, TBoard, TMove, TPlayer> displayer;

        private readonly Random random;

        public HiddenDriver(IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies, IDisplayer<TGame, TBoard, TMove, TPlayer> displayer, Random random)
        {
            this.strategies = strategies.ToDictionary();
            this.displayer = displayer;
            this.random = random;
        }

        public TGame Run(TGame game)
        {
            while (game.Outcome == null)
            {
                var strategy = strategies[game.CurrentPlayer];
                displayer.DisplayBoard(game);
                displayer.DisplayMoves(game);
                var move = strategy.SelectMove(game);
                game = game.CommitSpecificMove(move, this.random);
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
