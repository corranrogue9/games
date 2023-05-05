namespace Fx.Tree
{
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
}
