namespace Fx.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Tree;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public static class Extension
    {
        /*internal static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            if (game.Moves.Any())
            {
                return Node.CreateTree(game, game.Moves.Select(move => game.CommitMove(move).ToTree()));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }*/

        internal static IGame<TGame, TBoard, TMove, TPlayer> AsGame<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return game;
        }

        internal static void ApplyToEmptyOrPopulated<T>(this IEnumerable<T> source, Action empty, Action<T> populated)
        {
            //// TODO this is a good example for either, maybe; how to go from ienumerable to either

            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(empty, nameof(empty));
            Ensure.NotNull(populated, nameof(populated));

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    empty();
                    return;
                }

                do
                {
                    populated(enumerator.Current);
                }
                while (enumerator.MoveNext());
            }
        }

        public static T Minimum<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException("TODO");
                }

                var minElement = enumerator.Current;
                var minValue = selector(minElement);
                while (enumerator.MoveNext())
                {
                    var currentElement = enumerator.Current;
                    var currentValue = selector(currentElement);
                    if (currentValue < minValue)
                    {
                        minElement = currentElement;
                        minValue = currentValue;
                    }
                }

                return minElement;
            }
        }

        public static T Maximum<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            return Minimum(source, (element) => selector(element) * -1);
        }
    }
}
