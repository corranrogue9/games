using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source)
            where TKey: notnull
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
