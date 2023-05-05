namespace Fx.TreeFactory
{
    using System.Collections.Generic;

    using Fx.Tree;

    public interface ITreeFactory
    {
        ITree<T> CreateLeaf<T>(T value);

        ITree<T> CreateInner<T>(T value, IEnumerable<ITree<T>> children);
    }
}
