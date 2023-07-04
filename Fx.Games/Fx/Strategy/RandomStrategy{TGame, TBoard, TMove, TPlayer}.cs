namespace Fx.Strategy
{
    using System;
    using System.Linq;

    using Fx.Game;

    public sealed class RandomStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Random rng;

        public RandomStrategy()
            : this(new Random())
        {
        }

        public RandomStrategy(Random random)
        {
            this.rng = random;
        }

        public TMove SelectMove(TGame game)
        {

            var moves = game.Moves.ToList();
            var move = rng.Choose(moves);
            return move;
        }
    }
}
