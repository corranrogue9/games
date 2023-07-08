using System.Diagnostics.CodeAnalysis;
using Fx;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static bool TryFirst<T>(this IEnumerable<T> source, out T value)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    value = enumerator.Current;
                    return true;
                }

                value = default!;
                return false;
            }
        }

        internal static T Choose<T>(this IEnumerable<T> source, Func<T, bool> preference, Func<T, bool> fallback)
        {
            return source
                .Aggregate(
                    (0, default(T)),
                    (aggregation, current) => aggregation.Item1 == 2 ? aggregation : preference(current) ? (2, current) : aggregation.Item1 == 1 ? aggregation : fallback(current) ? (1, current) : (0, current))
                .Item2;
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



        public static bool TryGetSingle<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, [MaybeNullWhen(false)] out TSource single)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    TSource result = e.Current;
                    if (predicate(result))
                    {
                        while (e.MoveNext())
                        {
                            if (predicate(e.Current))
                            {
                                throw new InvalidOperationException("Sequence contains more than one element");

                            }
                        }
                        single = result;
                        return true;
                    }
                }
            }

            single = default;
            return false;
        }
    }
}
