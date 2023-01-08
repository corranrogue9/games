namespace Fx.Tree
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO figure out how to remove Value and Children, then document this interface
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface ITree<out TValue>
    {
        TResult Fold<TResult>(Func<TValue, TResult> whenLeaf, Func<TValue, IEnumerable<TResult>, TResult> whenInner);

        TValue Value { get; }

        IEnumerable<ITree<TValue>> Children { get; }
    }
}
