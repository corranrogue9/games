using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;

namespace Fx.Linq
{
    public static class RangedEnumerableExtensions
    {
        public static bool All<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self.Range.Maximum == 0)
            {
                return true;
            }

            return self.AsEnumerable().All(predicate);
        }

        public static bool Any<TSource>(this IRangedEnumerable<TSource> self)
        {
            if (self.Range.Minimum > 0)
            {
                return true;
            }
            else if (self.Range.Maximum == 0)
            {
                return false;
            }

            return self.AsEnumerable().Any();
        }

        public static bool Any<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self.Range.Maximum == 0)
            {
                return false;
            }

            return self.AsEnumerable().Any(predicate);
        }

        public static IRangedEnumerable<TSource> Append<TSource>(this IRangedEnumerable<TSource> self, TSource element)
        {
            return RangedEnumerable.Create(self.AsEnumerable().Append(element), self.Range.Append());
        }

        public static IRangedEnumerable<TSource[]> Chunk<TSource>(this IRangedEnumerable<TSource> self, int size)
        {
            return RangedEnumerable.Create(self.AsEnumerable().Chunk(size), self.Range.Chunk(size));
        }

        public static IRangedEnumerable<TSource> Concat<TSource>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second)
        {
            return RangedEnumerable.Create(first.AsEnumerable().Concat(second), first.Range.Concat(second.Range));
        }

        public static int Count<TSource>(this IRangedEnumerable<TSource> self)
        {
            if (self.Range.Minimum == self.Range.Maximum)
            {
                return (int)self.Range.Minimum;
            }

            return self.AsEnumerable().Count();
        }

        public static IRangedEnumerable<TSource?> DefaultIfEmpty<TSource>(this IRangedEnumerable<TSource> self)
        {
            if (self.Range.Maximum == 0)
            {
                return RangedEnumerable.Create(self.AsEnumerable().DefaultIfEmpty(), new Range(1, 1));
            }

            return RangedEnumerable.Create(self.AsEnumerable().DefaultIfEmpty(), self.Range.DefaultIfEmpty());
        }

        public static IRangedEnumerable<TSource> DefaultIfEmpty<TSource>(this IRangedEnumerable<TSource> self, TSource defaultValue)
        {
            throw new NotImplementedException();
        }

        public static IRangedEnumerable<TSource> Distinct<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Distinct<TSource>(this IRangedEnumerable<TSource> self, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> DistinctBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> DistinctBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource ElementAt<TSource>(this IRangedEnumerable<TSource> self, Index index) { throw new NotImplementedException(); }

        public static TSource ElementAt<TSource>(this IRangedEnumerable<TSource> self, int index) { throw new NotImplementedException(); }

        public static TSource? ElementAtOrDefault<TSource>(this IRangedEnumerable<TSource> self, Index index) { throw new NotImplementedException(); }

        public static TSource? ElementAtOrDefault<TSource>(this IRangedEnumerable<TSource> self, int index) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> Empty<TResult>() { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Except<TSource>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Except<TSource>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> ExceptBy<TSource, TKey>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TKey> second, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> ExceptBy<TSource, TKey>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource First<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource First<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource? FirstOrDefault<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource FirstOrDefault<TSource>(this IRangedEnumerable<TSource> self, TSource defaultValue) { throw new NotImplementedException(); }

        public static TSource? FirstOrDefault<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource FirstOrDefault<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IRangedEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IRangedEnumerable<TElement>, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TKey, IRangedEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TKey, IRangedEnumerable<TSource>, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IRangedEnumerable<TOuter> outer, IRangedEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IRangedEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IRangedEnumerable<TOuter> outer, IRangedEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IRangedEnumerable<TInner>, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Intersect<TSource>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Intersect<TSource>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> IntersectBy<TSource, TKey>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TKey> second, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> IntersectBy<TSource, TKey>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IRangedEnumerable<TOuter> outer, IRangedEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IRangedEnumerable<TOuter> outer, IRangedEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource Last<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource Last<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource? LastOrDefault<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource LastOrDefault<TSource>(this IRangedEnumerable<TSource> self, TSource defaultValue) { throw new NotImplementedException(); }

        public static TSource? LastOrDefault<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource LastOrDefault<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue) { throw new NotImplementedException(); }

        public static long LongCount<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static long LongCount<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static long Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, long> selector) { throw new NotImplementedException(); }

        public static decimal Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, decimal> selector) { throw new NotImplementedException(); }

        public static double Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, double> selector) { throw new NotImplementedException(); }

        public static int Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, int> selector) { throw new NotImplementedException(); }

        public static decimal? Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, decimal?> selector) { throw new NotImplementedException(); }

        public static TSource? Max<TSource>(this IRangedEnumerable<TSource> self, IComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static int? Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, int?> selector) { throw new NotImplementedException(); }

        public static long? Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, long?> selector) { throw new NotImplementedException(); }

        public static float? Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, float?> selector) { throw new NotImplementedException(); }

        public static TResult? Max<TSource, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, TResult> selector) { throw new NotImplementedException(); }

        public static double? Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, double?> selector) { throw new NotImplementedException(); }

        public static TSource? Max<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static float Max<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, float> selector) { throw new NotImplementedException(); }

        public static float Max(this IRangedEnumerable<float> self) { throw new NotImplementedException(); }

        public static float? Max(this IRangedEnumerable<float?> self) { throw new NotImplementedException(); }

        public static long? Max(this IRangedEnumerable<long?> self) { throw new NotImplementedException(); }

        public static int? Max(this IRangedEnumerable<int?> self) { throw new NotImplementedException(); }

        public static double? Max(this IRangedEnumerable<double?> self) { throw new NotImplementedException(); }

        public static decimal? Max(this IRangedEnumerable<decimal?> self) { throw new NotImplementedException(); }

        public static long Max(this IRangedEnumerable<long> self) { throw new NotImplementedException(); }

        public static int Max(this IRangedEnumerable<int> self) { throw new NotImplementedException(); }

        public static double Max(this IRangedEnumerable<double> self) { throw new NotImplementedException(); }

        public static decimal Max(this IRangedEnumerable<decimal> self) { throw new NotImplementedException(); }

        public static TSource? MaxBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static TSource? MaxBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static decimal Min(this IRangedEnumerable<decimal> self) { throw new NotImplementedException(); }

        public static TResult? Min<TSource, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, TResult> selector) { throw new NotImplementedException(); }

        public static float Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, float> selector) { throw new NotImplementedException(); }

        public static float? Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, float?> selector) { throw new NotImplementedException(); }

        public static int? Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, int?> selector) { throw new NotImplementedException(); }

        public static double? Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, double?> selector) { throw new NotImplementedException(); }

        public static decimal? Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, decimal?> selector) { throw new NotImplementedException(); }

        public static long Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, long> selector) { throw new NotImplementedException(); }

        public static int Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, int> selector) { throw new NotImplementedException(); }

        public static decimal Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, decimal> selector) { throw new NotImplementedException(); }

        public static TSource? Min<TSource>(this IRangedEnumerable<TSource> self, IComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static TSource? Min<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static long? Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, long?> selector) { throw new NotImplementedException(); }

        public static double Min<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, double> selector) { throw new NotImplementedException(); }

        public static float Min(this IRangedEnumerable<float> self) { throw new NotImplementedException(); }

        public static float? Min(this IRangedEnumerable<float?> self) { throw new NotImplementedException(); }

        public static long? Min(this IRangedEnumerable<long?> self) { throw new NotImplementedException(); }

        public static int? Min(this IRangedEnumerable<int?> self) { throw new NotImplementedException(); }

        public static double? Min(this IRangedEnumerable<double?> self) { throw new NotImplementedException(); }

        public static decimal? Min(this IRangedEnumerable<decimal?> self) { throw new NotImplementedException(); }

        public static double Min(this IRangedEnumerable<double> self) { throw new NotImplementedException(); }

        public static long Min(this IRangedEnumerable<long> self) { throw new NotImplementedException(); }

        public static int Min(this IRangedEnumerable<int> self) { throw new NotImplementedException(); }

        public static TSource? MinBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource? MinBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Prepend<TSource>(this IRangedEnumerable<TSource> self, TSource element) { throw new NotImplementedException(); }

        public static IRangedEnumerable<int> Range(int start, int count) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> Repeat<TResult>(TResult element, int count) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Reverse<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> Select<TSource, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, int, TResult> selector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> Select<TSource, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, TResult> selector)
        {
            return RangedEnumerable.Create(self.AsEnumerable().Select(selector), self.Range);
        }

        public static IRangedEnumerable<TResult> SelectMany<TSource, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, int, IRangedEnumerable<TResult>> selector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, IRangedEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, int, IRangedEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> SelectMany<TSource, TResult>(this IRangedEnumerable<TSource> self, Func<TSource, IRangedEnumerable<TResult>> selector) { throw new NotImplementedException(); }

        public static bool SequenceEqual<TSource>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second) { throw new NotImplementedException(); }

        public static bool SequenceEqual<TSource>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static TSource Single<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource Single<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static TSource SingleOrDefault<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue) { throw new NotImplementedException(); }

        public static TSource SingleOrDefault<TSource>(this IRangedEnumerable<TSource> self, TSource defaultValue) { throw new NotImplementedException(); }

        public static TSource? SingleOrDefault<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static TSource? SingleOrDefault<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Skip<TSource>(this IRangedEnumerable<TSource> self, int count) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> SkipLast<TSource>(this IRangedEnumerable<TSource> self, int count) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> SkipWhile<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> SkipWhile<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, int, bool> predicate) { throw new NotImplementedException(); }

        public static int Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, int> selector) { throw new NotImplementedException(); }

        public static long Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, long> selector) { throw new NotImplementedException(); }

        public static decimal? Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, decimal?> selector) { throw new NotImplementedException(); }

        public static long? Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, long?> selector) { throw new NotImplementedException(); }

        public static int? Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, int?> selector) { throw new NotImplementedException(); }

        public static double Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, double> selector) { throw new NotImplementedException(); }

        public static float? Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, float?> selector) { throw new NotImplementedException(); }

        public static float Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, float> selector) { throw new NotImplementedException(); }

        public static double? Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, double?> selector) { throw new NotImplementedException(); }

        public static decimal Sum<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, decimal> selector) { throw new NotImplementedException(); }

        public static long? Sum(this IRangedEnumerable<long?> self) { throw new NotImplementedException(); }

        public static float? Sum(this IRangedEnumerable<float?> self) { throw new NotImplementedException(); }

        public static int? Sum(this IRangedEnumerable<int?> self) { throw new NotImplementedException(); }

        public static double? Sum(this IRangedEnumerable<double?> self) { throw new NotImplementedException(); }

        public static decimal? Sum(this IRangedEnumerable<decimal?> self) { throw new NotImplementedException(); }

        public static long Sum(this IRangedEnumerable<long> self) { throw new NotImplementedException(); }

        public static int Sum(this IRangedEnumerable<int> self) { throw new NotImplementedException(); }

        public static double Sum(this IRangedEnumerable<double> self) { throw new NotImplementedException(); }

        public static decimal Sum(this IRangedEnumerable<decimal> self) { throw new NotImplementedException(); }

        public static float Sum(this IRangedEnumerable<float> self) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Take<TSource>(this IRangedEnumerable<TSource> self, Range range) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Take<TSource>(this IRangedEnumerable<TSource> self, int count) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> TakeLast<TSource>(this IRangedEnumerable<TSource> self, int count) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> TakeWhile<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> TakeWhile<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, int, bool> predicate) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static TSource[] ToArray<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector) where TKey : notnull { throw new NotImplementedException(); }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) where TKey : notnull { throw new NotImplementedException(); }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) where TKey : notnull { throw new NotImplementedException(); }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer) where TKey : notnull { throw new NotImplementedException(); }

        public static HashSet<TSource> ToHashSet<TSource>(this IRangedEnumerable<TSource> self, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static HashSet<TSource> ToHashSet<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static List<TSource> ToList<TSource>(this IRangedEnumerable<TSource> self) { throw new NotImplementedException(); }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw new NotImplementedException(); }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IRangedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static bool TryGetNonEnumeratedCount<TSource>(this IRangedEnumerable<TSource> self, out int count) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Union<TSource>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Union<TSource>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second, IEqualityComparer<TSource>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> UnionBy<TSource, TKey>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second, Func<TSource, TKey> keySelector) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> UnionBy<TSource, TKey>(this IRangedEnumerable<TSource> first, IRangedEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TSource> Where<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, bool> predicate)
        {
            return RangedEnumerable.Create(self.AsEnumerable().Where(predicate), new Range(0, self.Range.Maximum));
        }

        public static IRangedEnumerable<TSource> Where<TSource>(this IRangedEnumerable<TSource> self, Func<TSource, int, bool> predicate) { throw new NotImplementedException(); }

        public static IRangedEnumerable<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(this IRangedEnumerable<TFirst> first, IRangedEnumerable<TSecond> second, IRangedEnumerable<TThird> third) { throw new NotImplementedException(); }

        public static IRangedEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IRangedEnumerable<TFirst> first, IRangedEnumerable<TSecond> second) { throw new NotImplementedException(); }

        public static IRangedEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IRangedEnumerable<TFirst> first, IRangedEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector) { throw new NotImplementedException(); }

        //// TODO start of non ranged enumerable extensions
        public static IRangedEnumerable<T> Where<T>(this IReadOnlyCollection<T> self, Func<T, bool> predicate)
        {
            return RangedEnumerable.Create(self.Where(predicate), new Range(0, self.Count));
        }
        //// TODO end of non ranged enumerable extensions

        public static void Test()
        {
            var tests = new[]
            {
                (new Fx.Linq.Range(0, 1), 1),
                (new Fx.Linq.Range(0, 2), 1),
                (new Fx.Linq.Range(0, 3), 1),
                (new Fx.Linq.Range(0, 4), 1),
                (new Fx.Linq.Range(0, 5), 2),
                (new Fx.Linq.Range(0, 6), 2),
                (new Fx.Linq.Range(0, 7), 2),
                (new Fx.Linq.Range(0, 8), 2),
                (new Fx.Linq.Range(0, 9), 2),
                (new Fx.Linq.Range(0, 10), 2),
                (new Fx.Linq.Range(0, 11), 2),
                (new Fx.Linq.Range(0, 12), 2),
                (new Fx.Linq.Range(0, 13), 3),
                (new Fx.Linq.Range(0, 14), 3),
                (new Fx.Linq.Range(0, 15), 3),
                (new Fx.Linq.Range(0, 16), 3),
                (new Fx.Linq.Range(0, 17), 3),
                (new Fx.Linq.Range(0, 18), 3),
                (new Fx.Linq.Range(0, 19), 3),
                (new Fx.Linq.Range(0, 20), 3),
                (new Fx.Linq.Range(0, 21), 3),
                (new Fx.Linq.Range(0, 22), 3),
                (new Fx.Linq.Range(0, 23), 3),
                (new Fx.Linq.Range(0, 24), 3),
                (new Fx.Linq.Range(0, 25), 3),
                (new Fx.Linq.Range(0, 26), 3),
                (new Fx.Linq.Range(0, 27), 3),
                (new Fx.Linq.Range(0, 28), 3),
                (new Fx.Linq.Range(0, 29), 4),
                (new Fx.Linq.Range(0, 30), 4),
                (new Fx.Linq.Range(0, 31), 4),
                (new Fx.Linq.Range(0, 32), 4),
                (new Fx.Linq.Range(0, 33), 4),
                (new Fx.Linq.Range(0, 34), 4),
                (new Fx.Linq.Range(0, 35), 4),
                (new Fx.Linq.Range(0, 36), 4),
                (new Fx.Linq.Range(0, 37), 4),
                (new Fx.Linq.Range(0, 38), 4),
                (new Fx.Linq.Range(0, 39), 4),
            };
            var errors = new List<string>();
            foreach (var test in tests)
            {
                var actual = Fx.Linq.RangedEnumerableExtensions.BufferCount(test.Item1);
                var expected = BufferCountCorrect(test.Item1);
                if (test.Item2 != expected)
                {
                    throw new Exception();
                }
                if (test.Item2 != actual)
                {
                    errors.Add($"[{test.Item1.Minimum}, {test.Item1.Maximum}] => {actual} instead of {test.Item2}");
                }
            }

            Console.WriteLine(string.Join(Environment.NewLine, errors));
        }

        public static int BufferCount(Range range)
        {
            // https://math.stackexchange.com/questions/2697334/what-is-the-formula-for-finding-the-summation-of-an-exponential-function
            /*
sum(0, n)(r ^ i) = (r^(n+1) - 1)/(r - 1)



count - 4 * (r^(n+1) - 1)/(r - 1) - 1 <= 0
count - 4 * (2^(n+1) - 1) - 1 <= 0
count - 1 <= 4 * (2^(n+1) - 1)
count + 3 <= 2^(n + 3)
n >= log2(count + 3) - 3
             */


            // n >= log2(count + 3) - 3
            return BitOperations.Log2((range.Maximum - range.Minimum) + 3) - 1;
        }

        public static int BufferCountCorrect(Range range)
        {
            var totalCount = (int)range.Maximum - range.Minimum;
            int i = 1;
            var bufferLength = 4;
            while (true)
            {
                totalCount -= bufferLength;
                if (totalCount <= 0)
                {
                    return i;
                }

                ++i;
                bufferLength *= 2;
            }
        }

        public static T[] ToArray<T>(this IRangedEnumerable<T> self, bool overallocate)
        {
            if (overallocate)
            {
                var array = new T[self.Range.Maximum];
                int i = 0;
                foreach (var element in self)
                {
                    array[i++] = element;
                }

                var result = new T[i];
                if (i != array.Length)
                {
                    Array.Copy(array, result, i);
                }

                return result;
            }
            else
            {
                var buffers = new T[BufferCount(self.Range) + 1][];
                var buffer = new T[self.Range.Minimum];
                buffers[0] = buffer;
                var count = 0;
                var nextBufferCount = 4;
                using (var enumerator = self.GetEnumerator())
                {
                    for (int j = 0; j < buffers.Length; ++j)
                    {
                        for (int i = 0; i < buffer.Length; ++i)
                        {
                            if (enumerator.MoveNext())
                            {
                                ++count;
                                buffer[i] = enumerator.Current;
                            }
                            else
                            {
                                var result = new T[count];
                                for (int k = 0; k < j; ++k)
                                {
                                    Array.Copy(buffers[k], result, buffers[k].Length);
                                }

                                Array.Copy(buffers[j], result, i); //// TODO i - 1? there's probably a million off by one errors in this code

                                return result;
                            }
                        }

                        buffer = new T[nextBufferCount];
                        buffers[j + 1] = buffer;
                        nextBufferCount *= 2;
                    }
                }

                return Array.Empty<T>(); //// TODO is this always the correct thing to do here?

                // we are going to start the capacity at the minimum of the ranged enumerable, and then increase capacity by 4 the first time, then double each subsequent time
                // 
                /*
                range = maximum - minimum
                n = number of buffers
                range - sum(1,n)(4 ^ i) >= 0
                according to https://mathworld.wolfram.com/ExponentialSumFormulas.html,
                    sum(1,n)(4 ^ i) = -1 + sum(0,n-1)(4 ^ i) + 4 ^ n = -1 + (4 ^ n - 1)/(4 - 1) + 4 ^ n = -1 + (4 ^ n - 1)/3 + 4 ^ n
                so,
                range + 1 - (1 - 4 ^ n)/3 - 4 ^ n >= 0
                range + 1 - 1/3 + 4 ^ n / 3 - 4 ^ n >= 0
                4 ^ n / 3 - 4 ^ n >= -range - 2/3
                4 ^ n * (1/3 - 1) >= -range - 2/3
                4 ^ n * (-2/3) >= -range - 2/3
                4 ^ n <= (-range - 2/3) * -3/2 = 3 * range / 2 + 1
                n <= log4(3 * range / 2 + 1)

                n <= ceiling(log4((3 * range - 1) / 4 + 2))

                TODO test these:
                0, 1 => log4(2) = 1
                0, 2 => log4(1 + 2) = log4(3) = 1
                0, 3 => log4(2 + 2) = log4(4) = 1
                0, 4 => log4(2 + 2) = log4(4) = 1
                0, 5 => log4(3 + 2) = log4(5) = 2
                0, 6 => log4(3 + 2) = log4(5) = 2
                0, 7
                0, 8
                0, 9
                0, 10
                0, 11
                0, 12
                0, 13
                0, 14
                0, 15
                0, 16
                0, 17
                0, 18
                0, 19
                0, 20
                0, 21
                0, 22
                0, 23
                0, 24
                0, 25
                0, 26
                0, 27
                0, 28
                0, 29
                0, 30
                0, 31
                0, 32
                0, 33
                0, 34
                0, 35
                0, 36
                0, 37


range - (1 - 4^i) / (1 - 4) <= 0
                range <= (1 - 4^i) / (1 - 4)
                range - range * 4 <= 1 - 4 ^ i
                4 ^ i <= 1 - range + range * 4 = 1 + range * 3
                i <= log4(1 + range * 3)
                 */
                /*var buffers = new T[][];
                buffers[0] = new T[self.Range.Minimum];
                using (var enumerator = self.GetEnumerator())
                {
                    for (int i = 0; i < array.Length; ++i)
                    {
                        if (enumerator.MoveNext())
                        {
                            array[i] = enumerator.Current;
                        }
                    }
                }*/
            }
        }

        private static class RangedEnumerable
        {
            public static RangedEnumerable<T> Create<T>(IEnumerable<T> enumerable, Range range)
            {
                return new RangedEnumerable<T>(enumerable, range);
            }
        }

        private sealed class RangedEnumerable<T> : IRangedEnumerable<T>
        {
            private readonly IEnumerable<T> enumerable;

            private readonly Range range;

            public RangedEnumerable(IEnumerable<T> enumerable, Range range)
            {
                //// TODO doing it this way means that you must allocate a new Range every time an extension is called, but the caller doesn't necessarily use the range, so what you couild do is have a class per extension and then you only return the range if needed (and it can be cached since it's idempotent); this would also let the classes be structs and reduce those allocations too
                this.enumerable = enumerable;
                this.range = range;
            }

            public Range Range => this.range;

            public IEnumerator<T> GetEnumerator() => this.enumerable.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }

    public interface IRangedEnumerable<T> : IEnumerable<T>
    {
        Range Range { get; }
    }

    public static class RangeExtensions
    {
        public static Range Append(this Range self)
        {
            return new Range(self.Minimum + 1, self.Maximum + 1);
        }

        public static Range Chunk(this Range self, int size)
        {
            //// TODO do ireadonly collection too
            return new Range(((self.Minimum - 1) / (uint)size) + 1, ((self.Maximum - 1) / (uint)size) + 1);
        }

        public static Range Concat(this Range first, Range second)
        {
            return new Range(first.Minimum + second.Minimum, first.Maximum + second.Maximum);
        }

        public static Range DefaultIfEmpty(this Range self)
        {
            return new Range(Math.Max(1, self.Minimum), Math.Max(1, self.Maximum));
        }
    }

    public sealed class Range
    {
        public Range(int minimum, int maximum)
            : this(
                  minimum < 0 ? throw new ArgumentOutOfRangeException(nameof(minimum)) : (uint)minimum,
                  maximum < 0 ? throw new ArgumentOutOfRangeException(nameof(maximum)) : (uint)maximum)
        {
        }

        public Range(uint minimum, uint maximum)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public uint Minimum { get; }

        public uint Maximum { get; }
    }
}
