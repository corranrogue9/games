namespace Fx.Games
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    internal static class Extension
    {
        internal static T[,] Copy<T>(this T[,] source)
        {
            Ensure.NotNull(source, nameof(source));

            var clone = new T[source.GetLength(0), source.GetLength(1)];
            for (int i = 0; i < source.GetLength(0); ++i)
            {
                for (int j = 0; j < source.GetLength(1); ++j)
                {
                    clone[i, j] = source[i, j];
                }
            }

            return clone;
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
