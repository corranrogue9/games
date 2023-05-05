namespace Fx.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.TreeFactory;

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
