namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source)
        {
            //// TODO other overloads IEnumerable<KVP> for example
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
