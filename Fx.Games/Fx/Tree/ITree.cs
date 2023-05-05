namespace Fx.Tree
{
    using System;
    using System.Collections.Generic;

    public interface ITree<out TValue>
    {
        TResult Fold<TResult>(Func<TValue, TResult> whenLeaf, Func<TValue, IEnumerable<TResult>, TResult> whenInner);

        ////TResult Fold<TResult>(Func<TValue, int, TResult> whenLeaf, Func<TValue, IEnumerable<TResult>, int, TResult> whenInner, int depth);

        TValue Value { get; }

        IEnumerable<ITree<TValue>> Children { get; }
    }
}
