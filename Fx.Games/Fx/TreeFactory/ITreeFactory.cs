namespace Fx.TreeFactory
{
    using System.Collections.Generic;

    using Fx.Tree;

    public interface ITreeFactory //// TODO is there something to the fact that these methods have the same signature as the fold funcs?
    {
        ITree<T> CreateLeaf<T>(T value);

        ITree<T> CreateInner<T>(T value, IEnumerable<ITree<T>> children);
    }
}
