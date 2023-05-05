namespace Fx.Todo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Game;
    using Fx.Tree;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public static class Extension
    {
        public static void Enumerate<T>(this IEnumerable<T> source)
        {
            foreach (var element in source)
            {
            }
        }

        public static Func<Void> ToFunc(Action action)
        {
            return () =>
            {
                action();
                return new Void();
            };
        }

        public static T[][] ToArray<T>(this T[,] source)
        {
            var result = new T[source.GetLength(0)][];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = new T[source.GetLength(1)];
                for (int j = 0; j < result[i].Length; ++j)
                {
                    result[i][j] = source[i, j];
                }
            }

            return result;
        }

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
