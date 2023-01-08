namespace Fx.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class RandomStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Random rng;

        public RandomStrategy()
        {
            this.rng = new Random();
        }

        public TMove SelectMove(TGame game)
        {

            var moves = game.Moves.ToList();
            var move = rng.Choose(moves);
            return move;
        }
    }


    public static class RandomExtensions
    {
        public static T Choose<T>(this Random rng, IReadOnlyList<T> items)
        {
            var ix = rng.Next(0, items.Count);
            return items[ix];
        }
    }
}
