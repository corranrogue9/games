namespace Fx.Tree
{
    using Fx.TreeFactory;

    public static class Tree
    {

        public static ITree<T> CreateFromBranches<T>(IEnumerable<IEnumerable<T>> branches, ITreeFactory treeFactory)
        {
            //// TODO this should be an extension on ITreeFactory

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

            //// TODO calls to Any and First are wasteful, plus calls to groupby probably

            var subbranches = branches.Select(b => b.Skip(1)).Where(b => b.Any()).GroupBy(b => b.First());

            return treeFactory.CreateInner(value, subbranches.Select(b => CreateFromBranches(b, treeFactory)));
        }
    }
}
