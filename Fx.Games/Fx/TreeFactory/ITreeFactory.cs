namespace Fx.Tree
{
    using System.Collections.Generic;

    public interface ITreeFactory
    {
        ITree<T> CreateLeaf<T>(T value);

        ITree<T> CreateInner<T>(T value, IEnumerable<ITree<T>> children);
    }
}
