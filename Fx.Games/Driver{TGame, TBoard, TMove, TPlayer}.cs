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
        private readonly IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies;

        private readonly IDisplayer<TGame, TBoard, TMove, TPlayer> displayer;

        public Driver(IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies, IDisplayer<TGame, TBoard, TMove, TPlayer> displayer)
        {
            //// TODO create dictionary implementation that allows for null keys
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
            this.strategies = strategies.ToDictionary();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
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
}
