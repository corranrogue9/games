namespace Fx.Todo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fx.Game;
    using Fx.Tree;
    using Fx.TreeFactory;

    internal static class Extension
    {

        


        internal static T Choose<T>(this IEnumerable<T> source, Func<T, bool> preference, Func<T, bool> fallback)
        {
            return source
                .Aggregate(
                    (0, default(T)),
                    (aggregation, current) => aggregation.Item1 == 2 ? aggregation : preference(current) ? (2, current) : aggregation.Item1 == 1 ? aggregation : fallback(current) ? (1, current) : (0, current))
                .Item2;
        }

        public static ITree<T> CreateFromBranches<T>(IEnumerable<IEnumerable<T>> branches, ITreeFactory treeFactory)
        {
            if (!branches.Any())
            {
                throw new ArgumentException("TODO");
            }

            var branch = branches.First();
            if (!branch.Any())
            {
                throw new ArgumentException("TODO");
            }

            var value = branch.First();


            var subbranches = branches.Select(b => b.Skip(1)).Where(b => b.Any()).GroupBy(b => b.First());

            return treeFactory.CreateInner(value, subbranches.Select(b => CreateFromBranches(b, treeFactory)));
        }
    }
}
