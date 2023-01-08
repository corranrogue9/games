namespace Fx.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Node<T> : ITree<T> //// TODO this should be an interface so that you can have both an array backed tree *and* a node backed tree
    {
        public Node(T value)
            : this(value, Enumerable.Empty<Node<T>>())
        {
        }

        public Node(T value, IEnumerable<ITree<T>> nodes)
        {
            this.Value = value;
            this.Children = nodes;
        }

        public T Value { get; } //// TODO make readonly

        public IEnumerable<ITree<T>> Children { get; }
        
        public S Fold<S>(Func<T, S> whenLeaf, Func<T, IEnumerable<S>, S> whenInner)
        {
            if (!this.Children.Any()) //// TODO use a custom enumerator so that the call to Any isn't wasteful
            {
                return whenLeaf(this.Value);
            }
            else
            {
                return whenInner(this.Value, this.Children.Select(child => child.Fold(whenLeaf, whenInner)));
            }

            /*using (var enumerator = this.Nodes.GetEnumerator())
             {
                 if (!enumerator.MoveNext())
                 {
                     return ifEmpty();
                 }
                 else
                 {
                     return ifPopulated(enumerator);
                 }
             }*/
        }

        /*public TResult Fold<TResult>(Func<T, int, TResult> whenLeaf, Func<T, IEnumerable<TResult>, int, TResult> whenInner, int depth)
        {
            if (!this.Children.Any())
            {
                return whenLeaf(this.Value, depth);
            }
            else
            {
                return whenInner(this.Value, this.Children.Select(child => child.Fold(whenLeaf, whenInner, depth + 1)), depth);
            }
        }*/

        /*private sealed class Enumerator : IEnumerator<T>
        {
            private readonly T value;

            private readonly IEnumerator<T> enumerator;

            public Enumerator(T value, IEnumerator<T> enumerator)
            {
                this.value = value;
                this
            }

            public T Current => throw new NotImplementedException();

            object System.Collections.IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }*/
    }
}
