namespace Fx.Todo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Game;
    using Fx.Tree;
    using Fx.TreeFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public static class Extension
    {

        public static Func<Void> ToFunc(Action action)
        {
            return () =>
            {
                action();
                return new Void();
            };
        }

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
