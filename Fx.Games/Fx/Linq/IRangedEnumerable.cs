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
