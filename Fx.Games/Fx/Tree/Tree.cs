namespace Fx.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Node
    {
        public static Node<T> CreateBinaryTree<T>(T value, params T[] values)
        {
            return CreateBinaryTree(value, values);
        }


        public static Node<T> CreateBinaryTree<T>(T value, IReadOnlyList<T> values)
        {
            return null; //// TODO
        }

        public static Node<T> CreateTree<T>(T value)
        {
            return new Node<T>(value);
        }

        public static Node<T> CreateTree<T>(T value, params T[] values)
        {
            return CreateTree(value, values.AsEnumerable());
        }

        public static Node<T> CreateTree<T>(T value, IEnumerable<T> values)
        {
            return new Node<T>(value, values.Select(_ => new Node<T>(_)));
        }

        public static Node<T> CreateTree<T>(T value, params Node<T>[] values)
        {
            return CreateTree(value, values.AsEnumerable());
        }

        public static Node<T> CreateTree<T>(T value, IEnumerable<ITree<T>> values)
        {
            return new Node<T>(value, values);
        }

        public static ITreeFactory TreeFactory { get; } = new Factory();

        private sealed class Factory : ITreeFactory
        {
            public ITree<T> CreateLeaf<T>(T value)
            {
                return Node.CreateTree(value);
            }

            public ITree<T> CreateInner<T>(T value, IEnumerable<ITree<T>> children)
            {
                return Node.CreateTree(value, children);
            }
        }
    }

    public interface ITreeFactory
    {
        ITree<T> CreateLeaf<T>(T value);

        ITree<T> CreateInner<T>(T value, IEnumerable<ITree<T>> children);
    }

    public interface ITree<out TValue>
    {
        TResult Fold<TResult>(Func<TValue, TResult> whenLeaf, Func<TValue, IEnumerable<TResult>, TResult> whenInner);

        ////TResult Fold<TResult>(Func<TValue, int, TResult> whenLeaf, Func<TValue, IEnumerable<TResult>, int, TResult> whenInner, int depth);

        TValue Value { get; }

        IEnumerable<ITree<TValue>> Children { get; }
    }

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
            if (!this.Children.Any())
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

    public static class TreeExtensions
    {
        public static int NodeCount<T>(this ITree<T> tree)
        {
            return tree.Fold(nodeValue => 1, (nodeValue, nodes) => 1 + nodes.Sum());
        }

        public static int LeafCount<T>(this ITree<T> tree)
        {
            return tree.Fold(nodeValue => 1, (nodeValue, nodes) => nodes.Sum());
        }

        public static int Sum(this ITree<int> tree)
        {
            return tree.Fold((nodeValue) => nodeValue, (nodeValue, nodes) => nodeValue + nodes.Sum());
        }

        public static int Depth<T>(this ITree<T> tree)
        {
            return tree.Fold(nodeValue => 0, (nodeValue, nodes) => nodes.Max());
        }

        public static ITree<int> DepthTree<T>(this ITree<T> tree, ITreeFactory treeFactory)
        {
            return tree.DepthTree(treeFactory, 0);
        }

        private static ITree<int> DepthTree<T>(this ITree<T> tree, ITreeFactory treeFactory, int depth)
        {
            return treeFactory.CreateInner(depth, tree.Children.Select(child => child.DepthTree(treeFactory, depth + 1)));
        }

        public static IEnumerable<T> Leaves<T>(this ITree<T> tree)
        {
            return tree.Fold(value => new[] { value }.AsEnumerable(), (value, values) => values.SelectMany(_ => _));
        }

        public static ITree<TResult> Select<TValue, TResult>(
            this ITree<TValue> tree,
            Func<TValue, TResult> selector, 
            ITreeFactory treeFactory)
        {
            /*return tree.Fold(
                (nodeValue) => treeFactory.CreateLeaf(selector(nodeValue)),
                (nodeValue, nodes) => treeFactory.CreateInner(selector(nodeValue), nodes));*/
            return tree.Select(selector, (value, children) => selector(value), treeFactory);
        }

        public static ITree<TResult> Select<TValue, TResult>(
            this ITree<TValue> tree,
            Func<TValue, TResult> leafSelector,
            Func<TValue, IEnumerable<TResult>, TResult> childSelector,
            ITreeFactory treeFactory)
        {
            return tree.Fold(
                (nodeValue) => treeFactory.CreateLeaf(leafSelector(nodeValue)),
                (nodeValue, nodes) => treeFactory.CreateInner(childSelector(nodeValue, nodes.Select(node => node.Value)), nodes));
        }

        public static ITree<TResult> Merge<TFirst, TSecond, TResult>(this ITree<TFirst> first, ITree<TSecond> second, Func<TFirst, TSecond, TResult> selector, ITreeFactory treeFactory)
        {
            return treeFactory.CreateInner(
                selector(first.Value, second.Value),
                first.Children.Zip(second.Children).Select(child => child.First.Merge(child.Second, selector, treeFactory)));
        }

        /*public static Node<S> Visit<T, S>(this Node<T> tree, IVisitor<T, S>)
        {
        TODO
        }*/
    }
}
