

using System.Diagnostics.CodeAnalysis;

namespace Games;

public static class EnumerableExtensions
{
    public static bool TryFind<T>(this IEnumerable<T> items, Func<T, bool> pred, [MaybeNullWhen(false)] out T found)
    {
        foreach (var item in items)
        {
            if (pred(item))
            {
                found = item; return true;
            }
        }
        found = default;
        return false;
    }
}