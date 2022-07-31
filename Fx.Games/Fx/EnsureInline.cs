namespace Fx
{
    using System;
    using System.Runtime.CompilerServices;

    public static class EnsureInline
    {
        /// <summary>
        /// Ensures that <paramref name="value"/> is not null
        /// </summary>
        /// <typeparam name="T">The type of the value being validated</typeparam>
        /// <param name="value">The value to ensure is not null</param>
        /// <param name="name">The name of the parameter whose provided argument was <paramref name="value"/></param>
        /// <returns><paramref name="value"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NotNull<T>([ValidatedNotNull] T value, string name)
        {
            Ensure.NotNull(value, name);

            return value;
        }
    }
}
