namespace Fx.Linq
{
    public interface IFakeEnumerable
    {
    }

    public interface IFakeEnumerable<T>
    {
    }

    public static class FakeLinq
    {
        public static TSource Aggregate<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, TSource, TSource> func) { throw new NotImplementedException(); }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this IFakeEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func) { throw new NotImplementedException(); }

        public static TResult Aggregate<TSource, TAccumulate, TResult>(this IFakeEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector) { throw new NotImplementedException(); }

        public static bool All<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static bool Any<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static bool Any<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Append<TSource>(this IFakeEnumerable<TSource> self, TSource element) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> AsEnumerable<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static double Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int> selector) { throw new NotImplementedException(); }

        public static double Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, long> selector) { throw new NotImplementedException(); }

        public static double? Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, double?> selector) { throw new NotImplementedException(); }

        public static float Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, float> selector) { throw new NotImplementedException(); }

        public static double? Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, long?> selector) { throw new NotImplementedException(); }

        public static float? Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, float?> selector) { throw new NotImplementedException(); }

        public static double Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, double> selector) { throw new NotImplementedException(); }

        public static double? Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int?> selector) { throw new NotImplementedException(); }

        public static decimal Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, decimal> selector) { throw new NotImplementedException(); }

        public static decimal? Average<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, decimal?> selector) { throw new NotImplementedException(); }

        public static float? Average(this IFakeEnumerable<float?> self) { throw new NotImplementedException(); }

        public static double? Average(this IFakeEnumerable<long?> self) { throw new NotImplementedException(); }

        public static double? Average(this IFakeEnumerable<int?> self) { throw new NotImplementedException(); }

        public static double? Average(this IFakeEnumerable<double?> self) { throw new NotImplementedException(); }

        public static decimal? Average(this IFakeEnumerable<decimal?> self) { throw new NotImplementedException(); }

        public static double Average(this IFakeEnumerable<long> self) { throw new NotImplementedException(); }

        public static double Average(this IFakeEnumerable<int> self) { throw new NotImplementedException(); }

        public static double Average(this IFakeEnumerable<double> self) { throw new NotImplementedException(); }

        public static decimal Average(this IFakeEnumerable<decimal> self) { throw new NotImplementedException(); }

        public static float Average(this IFakeEnumerable<float> self) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> Cast<TResult>(this IFakeEnumerable self) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource[]> Chunk<TSource>(this IFakeEnumerable<TSource> self, int size) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Concat<TSource>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second) { throw new NotImplementedException(); }

        public static bool Contains<TSource>(this IFakeEnumerable<TSource> self, TSource value, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static bool Contains<TSource>(this IFakeEnumerable<TSource> self, TSource value) { throw new NotImplementedException(); }

        public static int Count<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static int Count<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource?> DefaultIfEmpty<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> DefaultIfEmpty<TSource>(this IFakeEnumerable<TSource> self, TSource defaultValue) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Distinct<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Distinct<TSource>(this IFakeEnumerable<TSource> self, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> DistinctBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> DistinctBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource ElementAt<TSource>(this IFakeEnumerable<TSource> self, Index index) { throw new NotImplementedException(); }

        public static TSource ElementAt<TSource>(this IFakeEnumerable<TSource> self, int index) { throw new NotImplementedException(); }

        public static TSource? ElementAtOrDefault<TSource>(this IFakeEnumerable<TSource> self, Index index) { throw new NotImplementedException(); }

        public static TSource? ElementAtOrDefault<TSource>(this IFakeEnumerable<TSource> self, int index) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> Empty<TResult>() { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Except<TSource>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Except<TSource>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> ExceptBy<TSource, TKey>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TKey> second, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> ExceptBy<TSource, TKey>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource First<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource First<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource? FirstOrDefault<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource FirstOrDefault<TSource>(this IFakeEnumerable<TSource> self, TSource defaultValue) { throw new NotImplementedException(); }

        public static TSource? FirstOrDefault<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource FirstOrDefault<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IFakeEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IFakeEnumerable<TElement>, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TKey, IFakeEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TKey, IFakeEnumerable<TSource>, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IFakeEnumerable<TOuter> outer, IFakeEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IFakeEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IFakeEnumerable<TOuter> outer, IFakeEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IFakeEnumerable<TInner>, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Intersect<TSource>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Intersect<TSource>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> IntersectBy<TSource, TKey>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TKey> second, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> IntersectBy<TSource, TKey>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IFakeEnumerable<TOuter> outer, IFakeEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IFakeEnumerable<TOuter> outer, IFakeEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource Last<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource Last<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource? LastOrDefault<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource LastOrDefault<TSource>(this IFakeEnumerable<TSource> self, TSource defaultValue) { throw new NotImplementedException(); }

        public static TSource? LastOrDefault<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource LastOrDefault<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue) { throw new NotImplementedException(); }

        public static long LongCount<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static long LongCount<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static long Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, long> selector) { throw new NotImplementedException(); }

        public static decimal Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, decimal> selector) { throw new NotImplementedException(); }

        public static double Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, double> selector) { throw new NotImplementedException(); }

        public static int Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int> selector) { throw new NotImplementedException(); }

        public static decimal? Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, decimal?> selector) { throw new NotImplementedException(); }

        public static TSource? Max<TSource>(this IFakeEnumerable<TSource> self, IComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static int? Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int?> selector) { throw new NotImplementedException(); }

        public static long? Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, long?> selector) { throw new NotImplementedException(); }

        public static float? Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, float?> selector) { throw new NotImplementedException(); }

        public static TResult? Max<TSource, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, TResult> selector) { throw new NotImplementedException(); }

        public static double? Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, double?> selector) { throw new NotImplementedException(); }

        public static TSource? Max<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static float Max<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, float> selector) { throw new NotImplementedException(); }

        public static float Max(this IFakeEnumerable<float> self) { throw new NotImplementedException(); }

        public static float? Max(this IFakeEnumerable<float?> self) { throw new NotImplementedException(); }

        public static long? Max(this IFakeEnumerable<long?> self) { throw new NotImplementedException(); }

        public static int? Max(this IFakeEnumerable<int?> self) { throw new NotImplementedException(); }

        public static double? Max(this IFakeEnumerable<double?> self) { throw new NotImplementedException(); }

        public static decimal? Max(this IFakeEnumerable<decimal?> self) { throw new NotImplementedException(); }

        public static long Max(this IFakeEnumerable<long> self) { throw new NotImplementedException(); }

        public static int Max(this IFakeEnumerable<int> self) { throw new NotImplementedException(); }

        public static double Max(this IFakeEnumerable<double> self) { throw new NotImplementedException(); }

        public static decimal Max(this IFakeEnumerable<decimal> self) { throw new NotImplementedException(); }

        public static TSource? MaxBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static TSource? MaxBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static decimal Min(this IFakeEnumerable<decimal> self) { throw new NotImplementedException(); }

        public static TResult? Min<TSource, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, TResult> selector) { throw new NotImplementedException(); }

        public static float Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, float> selector) { throw new NotImplementedException(); }

        public static float? Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, float?> selector) { throw new NotImplementedException(); }

        public static int? Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int?> selector) { throw new NotImplementedException(); }

        public static double? Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, double?> selector) { throw new NotImplementedException(); }

        public static decimal? Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, decimal?> selector) { throw new NotImplementedException(); }

        public static long Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, long> selector) { throw new NotImplementedException(); }

        public static int Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int> selector) { throw new NotImplementedException(); }

        public static decimal Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, decimal> selector) { throw new NotImplementedException(); }

        public static TSource? Min<TSource>(this IFakeEnumerable<TSource> self, IComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static TSource? Min<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static long? Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, long?> selector) { throw new NotImplementedException(); }

        public static double Min<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, double> selector) { throw new NotImplementedException(); }

        public static float Min(this IFakeEnumerable<float> self) { throw new NotImplementedException(); }

        public static float? Min(this IFakeEnumerable<float?> self) { throw new NotImplementedException(); }

        public static long? Min(this IFakeEnumerable<long?> self) { throw new NotImplementedException(); }

        public static int? Min(this IFakeEnumerable<int?> self) { throw new NotImplementedException(); }

        public static double? Min(this IFakeEnumerable<double?> self) { throw new NotImplementedException(); }

        public static decimal? Min(this IFakeEnumerable<decimal?> self) { throw new NotImplementedException(); }

        public static double Min(this IFakeEnumerable<double> self) { throw new NotImplementedException(); }

        public static long Min(this IFakeEnumerable<long> self) { throw new NotImplementedException(); }

        public static int Min(this IFakeEnumerable<int> self) { throw new NotImplementedException(); }

        public static TSource? MinBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource? MinBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> OfType<TResult>(this IFakeEnumerable self) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Prepend<TSource>(this IFakeEnumerable<TSource> self, TSource element) { throw new NotImplementedException(); }

        public static IFakeEnumerable<int> Range(int start, int count) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> Repeat<TResult>(TResult element, int count) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Reverse<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> Select<TSource, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, int, TResult> selector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> Select<TSource, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, TResult> selector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> SelectMany<TSource, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, int, IFakeEnumerable<TResult>> selector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, IFakeEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, int, IFakeEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> SelectMany<TSource, TResult>(this IFakeEnumerable<TSource> self, Func<TSource, IFakeEnumerable<TResult>> selector) { throw new NotImplementedException(); }

        public static bool SequenceEqual<TSource>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second) { throw new NotImplementedException(); }

        public static bool SequenceEqual<TSource>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static TSource Single<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource Single<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource SingleOrDefault<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue) { throw new NotImplementedException(); }

        public static TSource SingleOrDefault<TSource>(this IFakeEnumerable<TSource> self, TSource defaultValue) { throw new NotImplementedException(); }

        public static TSource? SingleOrDefault<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource? SingleOrDefault<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Skip<TSource>(this IFakeEnumerable<TSource> self, int count) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> SkipLast<TSource>(this IFakeEnumerable<TSource> self, int count) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> SkipWhile<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> SkipWhile<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int, bool> predicate) { throw new NotImplementedException(); }

        public static int Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int> selector) { throw new NotImplementedException(); }

        public static long Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, long> selector) { throw new NotImplementedException(); }

        public static decimal? Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, decimal?> selector) { throw new NotImplementedException(); }

        public static long? Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, long?> selector) { throw new NotImplementedException(); }

        public static int? Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int?> selector) { throw new NotImplementedException(); }

        public static double Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, double> selector) { throw new NotImplementedException(); }

        public static float? Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, float?> selector) { throw new NotImplementedException(); }

        public static float Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, float> selector) { throw new NotImplementedException(); }

        public static double? Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, double?> selector) { throw new NotImplementedException(); }

        public static decimal Sum<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, decimal> selector) { throw new NotImplementedException(); }

        public static long? Sum(this IFakeEnumerable<long?> self) { throw new NotImplementedException(); }

        public static float? Sum(this IFakeEnumerable<float?> self) { throw new NotImplementedException(); }

        public static int? Sum(this IFakeEnumerable<int?> self) { throw new NotImplementedException(); }

        public static double? Sum(this IFakeEnumerable<double?> self) { throw new NotImplementedException(); }

        public static decimal? Sum(this IFakeEnumerable<decimal?> self) { throw new NotImplementedException(); }

        public static long Sum(this IFakeEnumerable<long> self) { throw new NotImplementedException(); }

        public static int Sum(this IFakeEnumerable<int> self) { throw new NotImplementedException(); }

        public static double Sum(this IFakeEnumerable<double> self) { throw new NotImplementedException(); }

        public static decimal Sum(this IFakeEnumerable<decimal> self) { throw new NotImplementedException(); }

        public static float Sum(this IFakeEnumerable<float> self) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Take<TSource>(this IFakeEnumerable<TSource> self, Range range) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Take<TSource>(this IFakeEnumerable<TSource> self, int count) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> TakeLast<TSource>(this IFakeEnumerable<TSource> self, int count) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> TakeWhile<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> TakeWhile<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int, bool> predicate) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource[] ToArray<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector) where TKey : notnull { throw new NotImplementedException(); }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) where TKey : notnull { throw new NotImplementedException(); }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) where TKey : notnull { throw new NotImplementedException(); }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer) where TKey : notnull { throw new NotImplementedException(); }

        public static HashSet<TSource> ToHashSet<TSource>(this IFakeEnumerable<TSource> self, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static HashSet<TSource> ToHashSet<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static List<TSource> ToList<TSource>(this IFakeEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw new NotImplementedException(); }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IFakeEnumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static bool TryGetNonEnumeratedCount<TSource>(this IFakeEnumerable<TSource> self, out int count) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Union<TSource>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Union<TSource>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> UnionBy<TSource, TKey>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> UnionBy<TSource, TKey>(this IFakeEnumerable<TSource> first, IFakeEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Where<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TSource> Where<TSource>(this IFakeEnumerable<TSource> self, Func<TSource, int, bool> predicate) { throw new NotImplementedException(); }

        public static IFakeEnumerable<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(this IFakeEnumerable<TFirst> first, IFakeEnumerable<TSecond> second, IFakeEnumerable<TThird> third) { throw new NotImplementedException(); }

        public static IFakeEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IFakeEnumerable<TFirst> first, IFakeEnumerable<TSecond> second) { throw new NotImplementedException(); }

        public static IFakeEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IFakeEnumerable<TFirst> first, IFakeEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector) { throw new NotImplementedException(); }
    }
}
