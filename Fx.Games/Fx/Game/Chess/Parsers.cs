namespace Fx.Games.Chess
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using Fx.Game.Chess;

    public delegate bool Parser<T>(
           ReadOnlySpan<char> input,
           [MaybeNullWhen(false)] out ReadOnlySpan<char> remainder,
           [MaybeNullWhen(false)] out T value
       );


    public static class Parsers
    {
        public static readonly Parser<string> Whitespace = Regex(@"\s*");

        public static readonly Parser<int> Number = Regex("[0-9]+").Select<string, int>(int.TryParse);

        public static Parser<char> Char(char ch)
        {
            bool Parse(ReadOnlySpan<char> input, [MaybeNullWhen(false)] out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out char value)
            {
                if (input.Length > 0 && input[0] == ch)
                {
                    value = ch;
                    remainder = input[1..];
                    return true;
                }
                remainder = default;
                value = default;
                return false;
            }
            return Parse;
        }

        public static Parser<string> String(string str)
        {
            bool Parse(ReadOnlySpan<char> input, [MaybeNullWhen(false)] out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out string value)
            {
                if (input.StartsWith(str))
                {
                    value = str;
                    remainder = input[str.Length..];
                    return true;
                }
                remainder = default;
                value = default;
                return false;
            }
            return Parse;
        }

        public static Parser<string> Regex(string pattern)
        {
            Regex regex = new(@"\G" + pattern, RegexOptions.Compiled);

            bool Parse(ReadOnlySpan<char> input, [MaybeNullWhen(false)] out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out string value)
            {
                var match = regex.Match(input.ToString());
                if (match.Success)
                {
                    remainder = input[match.Length..];
                    value = match.Value;
                    return true;
                }
                remainder = default;
                value = default;
                return false;
            }
            return Parse;
        }

        public static Parser<S> Terminated<S, T>(Parser<S> first, Parser<T> second)
        {
            // return Tuple(first, second).Select(p => p.Item1);
            return from a in first from b in second select a;
        }

        public static Parser<T> Precceeded<S, T>(Parser<S> first, Parser<T> second)
        {
            return from a in first from b in second select b;
        }

        public static Parser<(S, T)> Tuple<S, T>(Parser<S> first, Parser<T> second)
        {
            return from a in first from b in second select (a, b);
        }

        public static Parser<(S, T, U)> Tuple<S, T, U>(Parser<S> first, Parser<T> second, Parser<U> third)
        {
            return from a in first from b in second from c in third select (a, b, c);
        }

        public static Parser<T> Select<S, T>(this Parser<S> first, Func<S, T> selector)
        {
            bool Parse(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out T value)
            {
                if (first(input, out var rem1, out var value1))
                {
                    value = selector(value1);
                    remainder = rem1;
                    return true;
                }
                value = default;
                remainder = default;
                return false;
            }
            return Parse;
        }

        public static Parser<U> SelectMany<S, T, U>(this Parser<S> first, Func<S, Parser<T>> second, Func<S, T, U> selector)
        {
            bool Parse(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out U value)
            {
                if (first(input, out var rem1, out var value1))
                {
                    if (second(value1)(rem1, out var rem2, out var value2))
                    {
                        value = selector(value1, value2);
                        remainder = rem2;
                        return true;
                    }
                }
                value = default;
                remainder = default;
                return false;
            }
            return Parse;
        }

        public delegate bool Try<S, T>(S input, out T value);

        public static Parser<T> Select<S, T>(this Parser<S> first, Try<S, T> trySelect)
        {
            bool Parse(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out T value)
            {
                if (first(input, out var rem1, out var value1) && trySelect(value1, out value))
                {
                    remainder = rem1;
                    return true;
                }
                value = default;
                remainder = default;
                return false;
            }
            return Parse;
        }

        public static Parser<IReadOnlyList<T>> Many<T>(this Parser<T> item)
        {
            bool Parse(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out IReadOnlyList<T> value)
            {
                var result = new List<T>();
                while (item(input, out var rem1, out var value1))
                {
                    result.Add(value1);
                    input = rem1;
                }
                value = result;
                remainder = input;
                return true;
            }
            return Parse;
        }

        public static Parser<Nullable<T>> Optional<T>(this Parser<T> item)
            where T : struct
        {
            bool ParseOptional(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out Nullable<T> value)
            {
                if (item(input, out var rem1, out var value1))
                {
                    value = value1;
                    remainder = rem1;
                    return true;
                }
                value = default;
                remainder = input;
                return true;
            }
            return ParseOptional;
        }

        public static Parser<T?> Maybe<T>(this Parser<T> item)
            where T : class
        {
            bool Parse(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out T? value)
            {
                if (item(input, out var rem1, out var value1))
                {
                    value = value1;
                    remainder = rem1;
                    return true;
                }
                value = default;
                remainder = input;
                return true;
            }
            return Parse;
        }

        public static Parser<T> Alternatives<T>(params Parser<T>[] parsers)
        {
            bool ParseAlternatives(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out T value)
            {
                foreach (var parser in parsers)
                {
                    if (parser(input, out remainder, out value))
                    {
                        return true;
                    }
                }
                value = default;
                remainder = input;
                return false;
            }
            return ParseAlternatives;
        }

        public static Parser<T> EOI<T>(this Parser<T> parser)
        {
            bool ParseEoi(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder, [MaybeNullWhen(false)] out T value)
            {
                if (parser(input, out remainder, out value))
                {
                    if (remainder.Length == 0)
                    {
                        return true;
                    }
                }
                value = default;
                remainder = input;
                return false;
            }
            return ParseEoi;
        }

    }
}