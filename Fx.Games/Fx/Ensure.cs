namespace Fx
{
    using System;
    using System.Runtime.CompilerServices;

    public static class Ensure
    {
        /// <summary>
        /// Ensures that <paramref name="value"/> is not null
        /// </summary>
        /// <typeparam name="T">The type of the value being validated</typeparam>
        /// <param name="value">The value to ensure is not null</param>
        /// <param name="name">The name of the parameter whose provided argument was <paramref name="value"/></param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNull<T>([ValidatedNotNull] T value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}