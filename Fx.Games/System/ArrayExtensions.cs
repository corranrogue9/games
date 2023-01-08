namespace System
{
    public static class ArrayExtensions
    {
        public static T[][] ToArray<T>(this T[,] source)
        {
            var result = new T[source.GetLength(0)][];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = new T[source.GetLength(1)];
                for (int j = 0; j < result[i].Length; ++j)
                {
                    result[i][j] = source[i, j];
                }
            }

            return result;
        }
    }
}
