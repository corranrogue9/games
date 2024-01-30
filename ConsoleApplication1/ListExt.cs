using System.IO.Compression;
using System.Net.Http.Headers;

namespace ConsoleApplication2
{
    public static class ListExt
    {
        public static void DoWork()
        {
            var left = Value
                .Create(1)
                .Add(3)
                .Add(4)
                .Add(-1);
            var right = Value
                .Create("asdf")
                .Add("qwer")
                .Add("1234")
                .Add("zxcv");
            //// TODO uncomment block comments and get ti implemented
            ////var zipped = left.Zip(right);
        }

        /*public static ValueNode<(TValueLeft, TValueRight), Leaf> Zip<TValueLeft, TValueRight>(this ValueLeaf<TValueLeft> left, ValueLeaf<TValueRight> right)
        {
            //// TODO in your node and value definitions you are certainly not having gauranteed type safety; please fix that
            return Value.Create((left.Value, right.Value));
        }

        public static ValueNode<(TValueLeft, TValueRight), TStructure> Zip<TValueLeft, TValueRight, TStructure>(
            this ValueNode<TValueLeft, Inner<TStructure>> left,
            ValueNode<TValueRight, Inner<TStructure>> right)
            where TStructure : Node
        {
            //// TODO in your node and value definitions you are certainly not having gauranteed type safety; please fix that

            if (left is ValueInner<TValueLeft, TStructure, ValueNode<TValueLeft, TStructure>> leftInner)
            {
            }

            return null;
        }*/
    }

    public abstract class Node
    {
    }

    public sealed class Leaf : Node
    {
    }

    public sealed class Inner<TNode> : Node where TNode : Node
    {
        public Inner(TNode node)
        {
            this.Node = node;
        }

        public TNode Node { get; }
    }

    public static class Value
    {
        public static ValueLeaf<TValue> Create<TValue>(TValue value)
        {
            return new ValueLeaf<TValue>(value);
        }

        public static ValueInner<TValue, ValueLeaf<TValue>> Add<TValue>(this ValueLeaf<TValue> leaf, TValue value)
        {
            return new ValueInner<TValue, ValueLeaf<TValue>>(value, leaf);
        }

        public static ValueInner<TValue, ValueInner<TValue, TValueNodeStart>> Add<TValue, TValueNodeStart>(
            this ValueInner<TValue, TValueNodeStart> inner,
            TValue value)
            where TValueNodeStart : ValueNode<TValue>
        {
            return new ValueInner<TValue, ValueInner<TValue, TValueNodeStart>>(value, inner);
        }
    }

    public abstract class ValueNode<TValue>
    {
        public abstract TValue Value { get; }
    }

    public sealed class ValueLeaf<TValue> : ValueNode<TValue>
    {
        public ValueLeaf(TValue value)
        {
            this.Value = value;
        }

        public override TValue Value { get; }
    }

    public sealed class ValueInner<TValue, TValueNode> : ValueNode<TValue> where TValueNode : ValueNode<TValue>
    {
        public ValueInner(TValue value, TValueNode node)
        {
            this.Value = value;
            this.Node = node;
        }

        public override TValue Value { get; }

        public TValueNode Node { get; }

        /*public static ValueInner<(TValue, TValueRight), TValueNode2> Zip<TValueRight, TValueNode2>()
            where TValueNode2 : ValueNode<(TValue, TValueRight)>
        {
        }*/
    }
}
