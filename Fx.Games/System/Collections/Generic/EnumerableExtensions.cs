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

                value = default;
                return false;
            }
        }

        public static void Enumerate<T>(this IEnumerable<T> source)
        {
            foreach (var element in source)
            {
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
    }
}
