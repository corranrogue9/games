using System.Collections;
using System.Data;
using System.Linq;

namespace Fx.Linq
{
    public static class RangedEnumerableExtensions
    {
        public static IRangedEnumerable<TResult> Select<TSource, TResult>(
            this IRangedEnumerable<TSource> self, 
            Func<TSource, TResult> selector)
        {
            return RangedEnumerable.Create(self.Select(selector), self.Range);
        }

        public static IRangedEnumerable<T> Concat<T>(this IRangedEnumerable<T> first, IRangedEnumerable<T> second)
        {
            return RangedEnumerable.Create(first.Concat(second), first.Range.Concat(second.Range));
        }

        public static IRangedEnumerable<T> Where<T>(this IRangedEnumerable<T> self, Func<T, bool> predicate)
        {
            return RangedEnumerable.Create(self.Where(predicate), new Range(0, self.Range.Maximum));
        }

        //// TODO start of non ranged enumerable extensions
        public static IRangedEnumerable<T> Where<T>(this IReadOnlyCollection<T> self, Func<T, bool> predicate)
        {
            return RangedEnumerable.Create(self.Where(predicate), new Range(0, self.Count));
        }
        //// TODO end of non ranged enumerable extensions

        public static bool Any<T>(this IRangedEnumerable<T> self)
        {
            return self.Range.Minimum > 0;
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
                // we are going to start the capacity at the minimum of the ranged enumerable, and then increase capacity by 4 the first time, then double each subsequent time
                // 
                /*
                range = maximum - minimum
                n = number of buffers
                range - sum(1,n)(4 ^ i) >= 0
                according to https://mathworld.wolfram.com/ExponentialSumFormulas.html,
                    sum(1,n)(4 ^ i) = -1 + sum(0,n-1)(4 ^ i) + 4 ^ n = -1 + (1 - 4 ^ n)/(1 - 4) + 4 ^ n = -1 - (1 - 4 ^ n)/3 + 4 ^ n
                so,
                range + 1 + (1 - 4 ^ n)/3 - 4 ^ n >= 0
                range + 1 + 1/3 - 4 ^ n / 3 - 4 ^ n >= 0
                range + 1 + 1/3 >= 4 ^ n / 3 + 4 ^ n = 4 ^ n * (1/3 + 1)
                range + 4/3 >= 4 ^ n * (4/3)
                (3 * range + 4) / 3 >= 4 ^ n * (4/3)
                3 * (3 * range + 4) / 3 / 4 >= 4 ^ n
                (3 * range + 4) / 4 >= 4 ^ n
                n <= log4(3 * range / 4 + 1)

                n <= ceiling(log4((3 * range) / 4 + 1))

                TODO test these:
                0, 3 => log4(2 + 1) = log4(3) = 1
                0, 4
                0, 5
                0, 15
                0, 16
                0, 17
                0, 31
                0, 32
                0, 33

                TODO test these:
                0, 1
                0, 2
                0, 4
                0, 13
                0, 14
                0, 16


range - (1 - 4^i) / (1 - 4) <= 0
                range <= (1 - 4^i) / (1 - 4)
                range - range * 4 <= 1 - 4 ^ i
                4 ^ i <= 1 - range + range * 4 = 1 + range * 3
                i <= log4(1 + range * 3)
                 */
                var buffers = new T[][];
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
                }
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
        public static Range Concat(this Range first, Range second)
        {
            return new Range(first.Minimum + second.Minimum, first.Maximum + second.Maximum);
        }
    }

    public sealed class Range
    {
        public Range(int minimum, int maximum)
            : this(
                  minimum < 0 ? throw new ArgumentOutOfRangeException(nameof(minimum)) : minimum, 
                  maximum < 0 ? throw new ArgumentOutOfRangeException(nameof(maximum)) : maximum)
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
